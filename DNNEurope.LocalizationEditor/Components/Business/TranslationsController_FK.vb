Imports System

Imports DNNEurope.Modules.LocalizationEditor.Data
Imports System.Collections.Generic

Namespace Business

 Partial Public Class TranslationsController

  Public Shared Function GetTranslationsByText(ByVal TextId As Int32 ) As List(Of TranslationInfo)

   Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of TranslationInfo)(DataProvider.Instance().GetTranslationsByText(TextId, 0, 1, ""))

  End Function

  Public Shared Function GetTranslationsByText(ByVal TextId As Int32 , StartRowIndex As Integer, MaximumRows As Integer, OrderBy As String) As List(Of TranslationInfo)

   Return DotNetNuke.Common.Utilities.CBO.FillCollection(Of TranslationInfo)(DataProvider.Instance().GetTranslationsByText(TextId, StartRowIndex, MaximumRows, OrderBy))

  End Function


 End Class
End Namespace

