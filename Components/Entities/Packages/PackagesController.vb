Imports System
Imports System.Data
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Services.Tokens

Imports DNNEurope.Modules.LocalizationEditor.Data
Imports DNNEurope.Modules.LocalizationEditor.Entities.Objects

Namespace Entities.Packages

 Partial Public Class PackagesController

  Public Shared Function GetObjectsByPackage(ByVal packageObjectId As Integer, ByVal version As String) As List(Of ObjectInfo)

   Return CBO.FillCollection(Of ObjectInfo)(DataProvider.Instance().GetObjectsByPackage(packageObjectId, version))

  End Function

  Public Shared Sub RegisterPackageItem(ByVal ParentObjectId As Integer, ByVal ParentVersion As String, ByVal ChildObjectId As Integer, ByVal ChildVersion As String)

   DataProvider.Instance().RegisterPackageItem(ParentObjectId, ParentVersion, ChildObjectId, ChildVersion)

  End Sub

  Public Shared Function GetParentObjects(ByVal objectId As Integer, ByVal version As String) As List(Of ObjectInfo)

   Return CBO.FillCollection(Of ObjectInfo)(DataProvider.Instance().GetParentObjects(objectId, version))

  End Function

  Public Shared Function GetPackages(moduleId As Integer, objectId As Integer) As List(Of PackInfo)

   Dim res As New List(Of PackInfo)
   Using cubeReader As IDataReader = Data.DataProvider.Instance.GetCube(moduleId)
    While cubeReader.Read
     If CInt(cubeReader.Item("ObjectId")) = objectId Then
      Dim newPack As New PackInfo
      newPack.Fill(cubeReader)
      res.Add(newPack)
     End If
    End While
   End Using
   Return res

  End Function

  Public Shared Function GetPackagesByVersion(moduleId As Integer, objectId As Integer, version As String) As List(Of PackInfo)

   Dim res As New List(Of PackInfo)
   Using cubeReader As IDataReader = Data.DataProvider.Instance.GetCube(moduleId)
    While cubeReader.Read
     If CInt(cubeReader.Item("ObjectId")) = objectId AndAlso version = CStr(cubeReader.Item("Version")) Then
      Dim newPack As New PackInfo
      newPack.Fill(cubeReader)
      res.Add(newPack)
     End If
    End While
   End Using
   Return res

  End Function

  Public Shared Function GetPackagesByLocale(moduleId As Integer, objectId As Integer, locale As String) As List(Of PackInfo)

   Dim res As New List(Of PackInfo)
   Using cubeReader As IDataReader = Data.DataProvider.Instance.GetCube(moduleId)
    While cubeReader.Read
     If CInt(cubeReader.Item("ObjectId")) = objectId AndAlso locale.ToLower.StartsWith(CStr(cubeReader.Item("Locale")).ToLower) Then
      Dim newPack As New PackInfo
      newPack.Fill(cubeReader)
      res.Add(newPack)
     End If
    End While
   End Using
   Return res

  End Function

  Public Shared Function GetPackageByLocaleAndVersion(moduleId As Integer, objectId As Integer, locale As String, version As String) As PackInfo

   Dim res As PackInfo = Nothing
   Using cubeReader As IDataReader = Data.DataProvider.Instance.GetCube(moduleId)
    While cubeReader.Read
     If CInt(cubeReader.Item("ObjectId")) = objectId AndAlso locale = CStr(cubeReader.Item("Locale")) AndAlso version = CStr(cubeReader.Item("Version")) Then
      res = New PackInfo
      res.Fill(cubeReader)
      Exit While
     End If
    End While
   End Using
   Return res

  End Function

 End Class
End Namespace

