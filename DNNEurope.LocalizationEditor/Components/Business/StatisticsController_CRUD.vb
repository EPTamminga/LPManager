Imports System
Imports DotNetNuke.Common.Utilities
Imports DNNEurope.Modules.LocalizationEditor.Data

Namespace Business

 Partial Public Class StatisticsController

 Public Shared Function GetStatistic(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32) As StatisticInfo
	
  Return CType(CBO.FillObject(DataProvider.Instance().GetStatistic(TextId, Locale, UserId), GetType(StatisticInfo)), StatisticInfo)

 End Function

  Public Shared Sub DeleteStatistic(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32)

   DataProvider.Instance().DeleteStatistic(TextId, Locale, UserId)

  End Sub

 End Class
End Namespace

