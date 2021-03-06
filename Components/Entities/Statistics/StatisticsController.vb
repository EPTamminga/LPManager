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
Imports DNNEurope.Modules.LocalizationEditor.Data
Imports System.Collections.Generic

Namespace Entities.Statistics

 Public Structure Statistic
  Public UserName As String
  Public TotalScore As Integer
 End Structure

 Public Class StatisticsController

  Public Shared Sub RecordStatistic(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32, ByVal Statistic As Integer)

   Dim st As StatisticInfo = GetStatistic(TextId, Locale, UserId)
   If st Is Nothing Then
    st = New StatisticInfo
    st.UserId = UserId
    st.TextId = TextId
    st.Locale = Locale
    st.Total = Statistic
    SetStatistic(st)
   Else
    st.Total += Statistic
    SetStatistic(st)
   End If

  End Sub

  Public Shared Function GetPackStatistics(ByVal ObjectId As Int32, ByVal Version As String, ByVal Locale As String) As List(Of Statistic)

   Dim res As New List(Of Statistic)
   Using ir As IDataReader = DataProvider.Instance().GetPackStatistics(ObjectId, Version, Locale)
    Do While ir.Read
     Dim s As New Statistic
     s.UserName = CStr(ir.Item("UserName"))
     s.TotalScore = CInt(ir.Item("Total"))
     res.Add(s)
    Loop
   End Using
   Return res

  End Function

  Public Shared Sub SetStatistic(ByVal objStatistic As StatisticInfo)

   DataProvider.Instance().SetStatistic(objStatistic.Locale, objStatistic.TextId, objStatistic.Total, objStatistic.UserId)

  End Sub

 End Class
End Namespace

