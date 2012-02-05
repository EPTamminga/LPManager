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

   Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of ObjectInfo)(DataProvider.Instance().GetObjectsByPackage(packageObjectId, version))

  End Function

  Public Shared Sub RegisterPackageItem(ByVal ParentObjectId As Integer, ByVal ParentVersion As String, ByVal ChildObjectId As Integer, ByVal ChildVersion As String)

   DataProvider.Instance().RegisterPackageItem(ParentObjectId, ParentVersion, ChildObjectId, ChildVersion)

  End Sub

 End Class
End Namespace

