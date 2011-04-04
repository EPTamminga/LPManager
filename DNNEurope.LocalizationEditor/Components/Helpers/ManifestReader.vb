﻿' 
' Copyright (c) 2004-2009 DNN-Europe, http://www.dnn-europe.net
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this 
' software and associated documentation files (the "Software"), to deal in the Software 
' without restriction, including without limitation the rights to use, copy, modify, merge, 
' publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
' to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or 
' substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
' INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
' PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
' FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
' ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
' 

Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports DNNEurope.Modules.LocalizationEditor.Business
Imports ICSharpCode.SharpZipLib.Zip

Public Class ManifestReader

 ''' <summary>
 ''' A module definition to import
 ''' </summary>
 ''' <remarks></remarks>
 Private Class ManifestModuleInfo
  Public ObjectName As String
  Public FriendlyName As String = ""
  Public FolderName As String = ""
  Public Version As String = "0"
  Public PackageType As String = "Module"
  Public ResourceFiles As New SortedList
 End Class

 ''' <summary>
 ''' Import an object from package. The zip is extracted to a temporary directory and searched for resource files. The object is added to the database and its resource files are read.
 ''' </summary>
    ''' <param name="moduleContent">Package content as stream</param>
    ''' <param name="HomeDirectoryMapPath">HomeDirectoryMapPath of this portal (needed for the temp path)</param>
    ''' <param name="ModuleId">To be used for an installed module</param>
 ''' <remarks></remarks>
 Public Shared Sub ImportModulePackage(ByVal moduleContent As IO.Stream, ByVal HomeDirectoryMapPath As String, ByVal ModuleId As Integer)
  ' Create a temporary directory to unpack the package
  Dim tempDirectory As String = HomeDirectoryMapPath & "LocalizationEditor\~tmp" & Now.ToString("yyyyMMdd-hhmmss") & "-" & (CInt(Rnd() * 1000)).ToString

  ' Check if the temporary directory already exists
  If Not Directory.Exists(tempDirectory) Then
   Directory.CreateDirectory(tempDirectory)
  Else
   Throw New IOException("New directory " & tempDirectory & " already exists")
  End If

  ' Unzip file contents
  ZipHelper.Unzip(moduleContent, tempDirectory)

  ' Find the DNN manifest file
  Dim dnnFiles As String() = Directory.GetFiles(tempDirectory, "*.dnn")
  If dnnFiles.Length = 0 Then
   If Directory.GetFiles(tempDirectory, "Default.aspx").Length = 0 Then
    Throw New FileNotFoundException("No DNN Manifest file found, nor a core distribution")
   End If
  End If
  ' Multiple manifests are allowed (.dnn and .dnn5)
  ' If dnnFiles.Length > 1 Then Throw New IOException("Multiple DNN Manifest files found")

  ' Now process what has been uploaded
  Dim manifestModules As New List(Of ManifestModuleInfo)

  If dnnFiles.Length = 0 Then ' we're processing the core

   ' we need to 'import' modules/providers etc.
   For Each d As DirectoryInfo In (New DirectoryInfo(tempDirectory & "\Install")).GetDirectories
    For Each obj As FileInfo In d.GetFiles("*.zip")
     Using objStream As New IO.FileStream(obj.FullName, FileMode.Open, FileAccess.Read)
      'Try
      ImportModulePackage(objStream, HomeDirectoryMapPath, ModuleId)
      'Catch ex As Exception
      '      End Try
     End Using
    Next
   Next

   Dim core As New ManifestModuleInfo
   With core
    .FriendlyName = Globals.glbCoreFriendlyName
    .ObjectName = Globals.glbCoreName
    .Version = GetAssemblyVersion(tempDirectory & "\bin\DotNetNuke.dll")
    .FolderName = ""
    .PackageType = "Core"
   End With
   ReadResourceFiles(core, "", tempDirectory)
   manifestModules.Add(core)

  Else ' we're processing a package with a manifest

   Dim dnnManifestFile As String = dnnFiles(0)

   ' Parse DNN manifest file
   Dim manifest As New XmlDocument
   manifest.Load(dnnManifestFile)
   Dim manifestVersion As Integer = 0

   ' Determine version of manifest file
   Dim mainNodes As XmlNodeList
   mainNodes = manifest.SelectNodes("dotnetnuke/packages/package")
   If mainNodes.Count > 0 Then
    manifestVersion = 5
   Else
    mainNodes = manifest.SelectNodes("dotnetnuke/folders/folder")
    If mainNodes.Count > 0 Then
     manifestVersion = 3
    End If
   End If

   If manifestVersion = 5 Then
    ' Remark about DNN 5 manifest file: it is assumed that only one <desktopModule> node per <package> node exists.

    ' Create a module for each package
    For Each packageNode As XmlNode In mainNodes

     Dim manifestModule As New ManifestModuleInfo()

     ' Determine the version
     If Not packageNode.SelectSingleNode("@version") Is Nothing Then
      manifestModule.Version = FormatVersion(packageNode.SelectSingleNode("@version").InnerText.Trim)
      If String.IsNullOrEmpty(manifestModule.Version) Then
       manifestModule.Version = "0"
       '2009-06-26 Janga:  Default version.
      End If
     Else : Throw New Exception("Could not retrieve version information in DNN Manifest file")
     End If

     ' Determine the object name
     If Not packageNode.SelectSingleNode("@name") Is Nothing Then
      manifestModule.ObjectName = packageNode.SelectSingleNode("@name").InnerText.Trim
     Else
      Throw New Exception("Could not retrieve package name in DNN Manifest file")
     End If

     ' Determine the friendly name
     If Not packageNode("friendlyName") Is Nothing Then
      manifestModule.FriendlyName = packageNode("friendlyName").InnerText
     Else
      manifestModule.FriendlyName = manifestModule.ObjectName
     End If

     manifestModule.PackageType = packageNode.SelectSingleNode("@type").InnerText

     Select Case manifestModule.PackageType.ToLower

      Case "module"

       ' Determine the desktop module
       Dim moduleNodes As XmlNodeList
       moduleNodes = packageNode.SelectNodes("components/component[@type='Module']/desktopModule")
       If moduleNodes.Count = 0 Then
        Throw New Exception("Could not retrieve desktop module information in DNN Manifest file")
       End If
       If moduleNodes.Count > 1 Then
        Throw New Exception("Multiple desktop modules found in DNN Manifest file")
       End If

       For Each dmNode As XmlNode In moduleNodes
        ' Determine the folder name
        If Not dmNode("foldername") Is Nothing Then
         manifestModule.FolderName = dmNode("foldername").InnerText.Replace("/"c, "\"c)
        Else
         Throw New Exception("Could not retrieve folder information in DNN Manifest file")
        End If
       Next

       ' Find the resource files using the manifest xml
       For Each fileGroupNode As XmlNode In packageNode.SelectNodes("components/component[@type='File']/files")
        Dim resourceFiles As String = ""
        Dim basePath As String = Path.Combine("DesktopModules", manifestModule.FolderName)
        If fileGroupNode("basePath") Is Nothing Then
         basePath = fileGroupNode("basePath").InnerText.Replace("/"c, "\"c).Trim("\"c)
        End If
        For Each node As XmlNode In fileGroupNode.SelectNodes("file")
         Dim resFile As String = ""
         Dim resDir As String = ""
         If Not node("name") Is Nothing Then resFile = node("name").InnerText
         If Not node("path") Is Nothing Then resDir = node("path").InnerText
         If resFile.ToLower.EndsWith("ascx.resx") OrElse resFile.ToLower.EndsWith("aspx.resx") Then
          Dim resPath As String = Path.Combine(Path.Combine(tempDirectory, resDir), resFile)
          If Not node("sourceFileName") Is Nothing Then resPath = Path.Combine(Path.Combine(tempDirectory, resDir), node("sourceFileName").InnerText)
          Dim resKey As String = Path.Combine(Path.Combine(basePath, resDir), resFile)
          manifestModule.ResourceFiles.Add(resKey, New FileInfo(resPath))
         End If
        Next
       Next

      Case "provider"

       ' Find the resource files using the manifest xml
       For Each fileGroupNode As XmlNode In packageNode.SelectNodes("components/component[@type='File']/files")
        Dim resourceFiles As String = ""
        Dim basePath As String = ""
        If fileGroupNode("basePath") Is Nothing Then
         basePath = fileGroupNode("basePath").InnerText.Replace("/"c, "\"c).Trim("\"c)
        End If
        For Each node As XmlNode In fileGroupNode.SelectNodes("file")
         Dim resFile As String = ""
         Dim resDir As String = ""
         If Not node("name") Is Nothing Then resFile = node("name").InnerText
         If Not node("path") Is Nothing Then resDir = node("path").InnerText
         If resFile.ToLower.EndsWith("ascx.resx") OrElse resFile.ToLower.EndsWith("aspx.resx") Then
          Dim resPath As String = Path.Combine(Path.Combine(tempDirectory, resDir), resFile)
          If Not node("sourceFileName") Is Nothing Then resPath = Path.Combine(Path.Combine(tempDirectory, resDir), node("sourceFileName").InnerText)
          Dim resKey As String = Path.Combine(Path.Combine(basePath, resDir), resFile)
          manifestModule.ResourceFiles.Add(resKey, New FileInfo(resPath))
         End If
        Next
       Next

      Case "skin"

       ' Find the resource files using the manifest xml
       For Each fileGroupNode As XmlNode In packageNode.SelectNodes("components/component[@type='Skin']/skinFiles")
        Dim resourceFiles As String = ""
        Dim basePath As String = ""
        If fileGroupNode("basePath") Is Nothing Then
         basePath = fileGroupNode("basePath").InnerText.Replace("/"c, "\"c).Trim("\"c)
        End If
        For Each node As XmlNode In fileGroupNode.SelectNodes("skinFile")
         Dim resFile As String = ""
         Dim resDir As String = ""
         If Not node("name") Is Nothing Then resFile = node("name").InnerText
         If Not node("path") Is Nothing Then resDir = node("path").InnerText
         If resFile.ToLower.EndsWith("ascx.resx") OrElse resFile.ToLower.EndsWith("aspx.resx") Then
          Dim resPath As String = Path.Combine(Path.Combine(tempDirectory, resDir), resFile)
          If Not node("sourceFileName") Is Nothing Then resPath = Path.Combine(Path.Combine(tempDirectory, resDir), node("sourceFileName").InnerText)
          Dim resKey As String = Path.Combine(Path.Combine(basePath, resDir), resFile)
          manifestModule.ResourceFiles.Add(resKey, New FileInfo(resPath))
         End If
        Next
       Next

      Case "container"

       ' Find the resource files using the manifest xml
       For Each fileGroupNode As XmlNode In packageNode.SelectNodes("components/component[@type='Container']/containerFiles")
        Dim resourceFiles As String = ""
        Dim basePath As String = ""
        If fileGroupNode("basePath") Is Nothing Then
         basePath = fileGroupNode("basePath").InnerText.Replace("/"c, "\"c).Trim("\"c)
        End If
        For Each node As XmlNode In fileGroupNode.SelectNodes("containerFile")
         Dim resFile As String = ""
         Dim resDir As String = ""
         If Not node("name") Is Nothing Then resFile = node("name").InnerText
         If Not node("path") Is Nothing Then resDir = node("path").InnerText
         If resFile.ToLower.EndsWith("ascx.resx") OrElse resFile.ToLower.EndsWith("aspx.resx") Then
          Dim resPath As String = Path.Combine(Path.Combine(tempDirectory, resDir), resFile)
          If Not node("sourceFileName") Is Nothing Then resPath = Path.Combine(Path.Combine(tempDirectory, resDir), node("sourceFileName").InnerText)
          Dim resKey As String = Path.Combine(Path.Combine(basePath, resDir), resFile)
          manifestModule.ResourceFiles.Add(resKey, New FileInfo(resPath))
         End If
        Next
       Next

     End Select

     ' Handle resource files
     Dim i As Integer = 0
     For Each resFileNode As XmlNode In packageNode.SelectNodes("components/component[@type='ResourceFile']")
      Dim basePath As String = resFileNode.SelectSingleNode("resourceFiles/basePath").InnerText.Replace("/"c, "\"c).Trim("\"c)
      Dim resFile As String = resFileNode.SelectSingleNode("resourceFiles/resourceFile/name").InnerText
      ZipHelper.Unzip(tempDirectory & "\" & resFile, tempDirectory & "\ResourceFiles" & i.ToString)
      ReadResourceFiles(manifestModule, basePath, tempDirectory & "\ResourceFiles" & i.ToString)
      i += 1
     Next

     ' Add the manifest module to the collection
     manifestModules.Add(manifestModule)
    Next

   ElseIf manifestVersion = 3 Then

    ' Create a module for each folder
    For Each folderNode As XmlNode In mainNodes
     Dim manifestModule As New ManifestModuleInfo()

     ' Determine the module name
     If Not folderNode("modulename") Is Nothing Then
      manifestModule.ObjectName = folderNode("modulename").InnerText
     ElseIf Not folderNode("friendlyname") Is Nothing Then
      manifestModule.ObjectName = folderNode("friendlyname").InnerText
     ElseIf Not folderNode("name") Is Nothing Then
      manifestModule.ObjectName = folderNode("name").InnerText
     Else : Throw New Exception("Could not retrieve module name in DNN Manifest file")
     End If

     manifestModule.PackageType = manifest.SelectSingleNode("dotnetnuke/@type").InnerText

     ' Determine the friendly name
     If Not folderNode("friendlyname") Is Nothing Then
      manifestModule.FriendlyName = folderNode("friendlyname").InnerText
     Else : manifestModule.FriendlyName = manifestModule.ObjectName
     End If

     ' Determine the folder name
     If Not folderNode("foldername") Is Nothing Then
      manifestModule.FolderName = folderNode("foldername").InnerText.Replace("/"c, "\"c)
     Else : Throw New Exception("Could not retrieve folder information in DNN Manifest file")
     End If

     ' Determine the version
     If Not folderNode("version") Is Nothing Then
      manifestModule.Version = folderNode("version").InnerText.Trim
      If String.IsNullOrEmpty(manifestModule.Version) Then
       manifestModule.Version = "0"
       '2009-06-26 Janga:  Default version.
      End If
     Else : Throw New Exception("Could not retrieve version information in DNN Manifest file")
     End If

     ' Find the resource files using the manifest xml
     Dim resourceFiles As String = ""
     For Each node As XmlNode In folderNode.SelectNodes("files/file")
      Dim resFile As String = ""
      Dim resDir As String = ""
      If Not node("name") Is Nothing Then resFile = node("name").InnerText
      If Not node("path") Is Nothing Then resDir = node("path").InnerText

      'TODO Support resource files which are already localized
      If resFile.ToLower.EndsWith("ascx.resx") OrElse resFile.ToLower.EndsWith("aspx.resx") Then
       ' Determine the resource directory and key for the module
       Dim resPath As String = Path.Combine(tempDirectory, resFile)
       Dim resKey As String = Path.Combine(Path.Combine(Path.Combine("DesktopModules", manifestModule.FolderName), resDir), resFile)

       manifestModule.ResourceFiles.Add(resKey, New FileInfo(resPath))
      End If
     Next

     ' Find the resource file
     Dim resFileNode As XmlNode = folderNode.SelectSingleNode("resourcefile")
     If Not folderNode("resourcefile") Is Nothing Then
      Dim basePath As String = "DesktopModules\" & manifestModule.FolderName
      Dim resFile As String = folderNode("resourcefile").InnerText
      ZipHelper.Unzip(tempDirectory & "\" & resFile, tempDirectory & "\ResourceFiles")
      ReadResourceFiles(manifestModule, basePath, tempDirectory & "\ResourceFiles")
     End If

     ' Add the manifest module to the collection
     manifestModules.Add(manifestModule)
    Next
   Else : Throw New Exception("Could not determine version of manifest file")
   End If

  End If ' whether core or package

  'TODO Use transactions
  ' DNN Manifest file or core parsed succesfully, now process each module from the manifest
  For Each manifestModule As ManifestModuleInfo In manifestModules
   If manifestModule.ObjectName IsNot Nothing Then
    If manifestModule.ResourceFiles.Count > 0 Then

     ' Check if the module is already imported
     Dim objObjectInfo As ObjectInfo = ObjectController.GetObjectByObjectName(manifestModule.ObjectName)
     If objObjectInfo Is Nothing Then
      ' Create a new translate module
      objObjectInfo = New ObjectInfo(0, manifestModule.ObjectName, manifestModule.FriendlyName, manifestModule.FolderName, ModuleId, manifestModule.PackageType)
      objObjectInfo.ObjectId = ObjectController.AddObject(objObjectInfo)
     End If

     ' Import or update resource files for this module
     LocalizationController.ProcessResourceFiles(manifestModule.ResourceFiles, tempDirectory, objObjectInfo, manifestModule.Version)

    End If
   End If
  Next

  ' Try to clean up
  Try
   IO.Directory.Delete(tempDirectory, True)
  Catch
  End Try

 End Sub

 Private Shared Sub ReadResourceFiles(ByRef manifestModule As ManifestModuleInfo, ByVal keyBasePath As String, ByVal path As String)

  If keyBasePath <> "" Then
   keyBasePath &= "\"
  End If

  For Each f As FileInfo In (New DirectoryInfo(path)).GetFiles("*.as?x.resx")
   If manifestModule.ResourceFiles(keyBasePath & f.Name) Is Nothing Then
    manifestModule.ResourceFiles.Add(keyBasePath & f.Name, f)
   End If
  Next
  For Each f As FileInfo In (New DirectoryInfo(path)).GetFiles("*Resources.resx")
   If manifestModule.ResourceFiles(keyBasePath & f.Name) Is Nothing Then
    manifestModule.ResourceFiles.Add(keyBasePath & f.Name, f)
   End If
  Next

  For Each d As String In Directory.GetDirectories(path)
   ReadResourceFiles(manifestModule, keyBasePath & Mid(d, d.LastIndexOf("\") + 2), d)
  Next

 End Sub

 Private Shared Function GetAssemblyVersion(ByVal path As String) As String

  Try
   Dim v As String = System.Diagnostics.FileVersionInfo.GetVersionInfo(path).FileVersion()
   Return FormatVersion(v)
  Catch ex As Exception
   Return "0"
  End Try

 End Function

 Private Shared Function FormatVersion(ByVal version As String) As String

  Dim m As Match = Regex.Match(version, "(\d+)\.(\d+)\.(\d+)\.?(\d*)")
  If m.Success Then
   version = CInt(m.Groups(1).Value).ToString("00")
   version &= "." & CInt(m.Groups(2).Value).ToString("00")
   version &= "." & CInt(m.Groups(3).Value).ToString("00")
  End If
  Return version

 End Function
End Class
