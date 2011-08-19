Imports System

Imports DotNetNuke.Common.Utilities

Imports DNNEurope.Modules.LocalizationEditor.Data

Namespace Business

 Partial Public Class TranslationsController

 Public Shared Function GetTranslation(ByVal TextId As Int32, ByVal Locale As String) As TranslationInfo
	
  Return CType(CBO.FillObject(DataProvider.Instance().GetTranslation(TextId, Locale), GetType(TranslationInfo)), TranslationInfo)

 End Function

  Public Shared Sub DeleteTranslation(ByVal TextId As Int32, ByVal Locale As String)

   DataProvider.Instance().DeleteTranslation(TextId, Locale)

  End Sub

 End Class
End Namespace

