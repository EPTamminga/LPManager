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
Imports DotNetNuke.Common.Utilities
Imports DNNEurope.Modules.LocalizationEditor.Data

Namespace Entities.Partners

 Partial Public Class PartnersController

  Public Shared Function GetPartner(ByVal PartnerId As Int32) As PartnerInfo

   Return CType(CBO.FillObject(DataProvider.Instance().GetPartner(PartnerId), GetType(PartnerInfo)), PartnerInfo)

  End Function

  Public Shared Function AddPartner(ByVal objPartner As PartnerInfo) As Integer

  Return CType(DataProvider.Instance().AddPartner(objPartner.AllowDirectDownload, objPartner.ModuleId, objPartner.AllowRedistribute, objPartner.CubeUrl, objPartner.DownloadAffiliates, objPartner.LastCube, objPartner.PackUrl, objPartner.PartnerName, objPartner.PartnerUrl, objPartner.PartnerUsername), Integer)

  End Function

  Public Shared Sub UpdatePartner(ByVal objPartner As PartnerInfo)

  DataProvider.Instance().UpdatePartner(objPartner.AllowDirectDownload, objPartner.ModuleId, objPartner.AllowRedistribute, objPartner.CubeUrl, objPartner.DownloadAffiliates, objPartner.LastCube, objPartner.PackUrl, objPartner.PartnerId, objPartner.PartnerName, objPartner.PartnerUrl, objPartner.PartnerUsername)

  End Sub

  Public Shared Sub DeletePartner(ByVal PartnerId As Int32)

   DataProvider.Instance().DeletePartner(PartnerId)

  End Sub

 End Class
End Namespace

