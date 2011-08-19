
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
Imports System.Collections.Generic

Namespace Business

 Partial Public Class StatisticsController

  Public Shared Function GetStatisticsByUser(ByVal UserID As Int32 ) As List(Of StatisticInfo)

   Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of StatisticInfo)(DataProvider.Instance().GetStatisticsByUser(UserID, 0, 1, ""))

  End Function

  Public Shared Function GetStatisticsByUser(ByVal UserID As Int32 , StartRowIndex As Integer, MaximumRows As Integer, OrderBy As String) As List(Of StatisticInfo)

   Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of StatisticInfo)(DataProvider.Instance().GetStatisticsByUser(UserID, StartRowIndex, MaximumRows, OrderBy))

  End Function


 End Class
End Namespace

