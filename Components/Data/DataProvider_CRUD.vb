' 
' Copyright (c) 2004-2011 DNN-Europe, http://www.dnn-europe.net
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
Imports System

Namespace Data

 Partial Public MustInherit Class DataProvider

#Region " Shared/Static Methods "

  ' singleton reference to the instantiated object 
  Private Shared _objProvider As DataProvider = Nothing

  ' constructor
  Shared Sub New()
   CreateProvider()
  End Sub

  ' dynamically create provider
  Private Shared Sub CreateProvider()
   _objProvider = CType(DotNetNuke.Framework.Reflection.CreateObject("data", "DNNEurope.Modules.LocalizationEditor.Data", ""), DataProvider)
  End Sub

  ' return the provider
  Public Shared Shadows Function Instance() As DataProvider
   Return _objProvider
  End Function

#End Region

#Region " General Methods "
  Public MustOverride Function GetNull(ByVal field As Object) As Object
#End Region


#Region " ObjectCoreVersion Methods "
#End Region


#Region " Object Methods "
  Public MustOverride Function GetObject(ByVal ObjectId As Int32) As IDataReader
  Public MustOverride Function AddObject(ByVal FriendlyName As String, ByVal ModuleId As Int32, ByVal InstallPath As String, ByVal ObjectName As String, ByVal PackageType As String) As Integer
  Public MustOverride Sub DeleteObject(ByVal ObjectId As Int32)
#End Region


#Region " PartnerPack Methods "
  Public MustOverride Function GetPartnerPack(ByVal PartnerId As Int32, ByVal ObjectId As Int32, ByVal Version As String, ByVal Locale As String) As IDataReader
  Public MustOverride Sub AddPartnerPack(ByVal LastModified As Date, ByVal Locale As String, ByVal ObjectId As Int32, ByVal PartnerId As Int32, ByVal PercentComplete As Single, ByVal RemoteObjectId As Int32, ByVal Version As String)
  Public MustOverride Sub UpdatePartnerPack(ByVal LastModified As Date, ByVal Locale As String, ByVal ObjectId As Int32, ByVal PartnerId As Int32, ByVal PercentComplete As Single, ByVal RemoteObjectId As Int32, ByVal Version As String)
  Public MustOverride Sub DeletePartnerPack(ByVal PartnerId As Int32, ByVal ObjectId As Int32, ByVal Version As String, ByVal Locale As String)
#End Region


#Region " Partner Methods "
  Public MustOverride Function GetPartner(ByVal PartnerId As Int32) As IDataReader
	Public MustOverride Function AddPartner(ByVal AllowDirectDownload As Boolean, ByVal ModuleId As Int32, ByVal AllowRedistribute As Boolean, ByVal CubeUrl As String, ByVal DownloadAffiliates As Boolean, ByVal LastCube As String, ByVal PackUrl As String, ByVal PartnerName As String, ByVal PartnerUrl As String, ByVal PartnerUsername As String) As Integer	
	Public MustOverride Sub UpdatePartner(ByVal AllowDirectDownload As Boolean, ByVal ModuleId As Int32, ByVal AllowRedistribute As Boolean, ByVal CubeUrl As String, ByVal DownloadAffiliates As Boolean, ByVal LastCube As String, ByVal PackUrl As String, ByVal PartnerId As Int32, ByVal PartnerName As String, ByVal PartnerUrl As String, ByVal PartnerUsername As String)	
  Public MustOverride Sub DeletePartner(ByVal PartnerId As Int32)
#End Region


#Region " Permission Methods "
  Public MustOverride Function AddPermission(ByVal ModuleId As Int32, ByVal Locale As String, ByVal UserId As Int32) As Integer
  Public MustOverride Sub DeletePermission(ByVal PermissionId As Int32)
#End Region


#Region " Statistic Methods "
  Public MustOverride Function GetStatistic(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32) As IDataReader
  Public MustOverride Sub DeleteStatistic(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32)
#End Region


#Region " Text Methods "
  Public MustOverride Function GetText(ByVal TextId As Int32) As IDataReader
  Public MustOverride Function AddText(ByVal DeprecatedIn As String, ByVal FilePath As String, ByVal ObjectId As Int32, ByVal OriginalValue As String, ByVal TextKey As String, ByVal Version As String) As Integer
  Public MustOverride Sub UpdateText(ByVal DeprecatedIn As String, ByVal FilePath As String, ByVal ObjectId As Int32, ByVal OriginalValue As String, ByVal TextId As Int32, ByVal TextKey As String, ByVal Version As String)
  Public MustOverride Sub DeleteText(ByVal TextId As Int32)
#End Region


#Region " Translation Methods "
  Public MustOverride Function GetTranslation(ByVal TextId As Int32, ByVal Locale As String) As IDataReader
  Public MustOverride Sub DeleteTranslation(ByVal TextId As Int32, ByVal Locale As String)
#End Region


 End Class

End Namespace

