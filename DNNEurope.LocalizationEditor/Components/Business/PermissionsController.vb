' 
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
Imports DNNEurope.Modules.LocalizationEditor.Data
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common.Utilities

Namespace DNNEurope.Modules.LocalizationEditor.Business

#Region " PermissionsController "

    Public Class PermissionsController
        Public Shared Function GetPermission(ByVal ObjectId As Integer, ByVal UserId As Integer, ByVal Locale As String, _
                                              ByVal ModuleId As Integer) As PermissionInfo
            Return _
                CType( _
                    CBO.FillObject(DataProvider.Instance().GetPermission(ObjectId, UserId, Locale, ModuleId), _
                                    GetType(PermissionInfo)), PermissionInfo)
        End Function

        Public Shared Function GetPermissions(ByVal ModuleId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetPermissions(ModuleId), GetType(PermissionInfo))
        End Function

        'Public Shared Function GetPermissionsByUserObject(ByVal ObjectId As Integer, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal ModuleId As Integer) As ArrayList
        ' Return CBO.FillCollection(Data.DataProvider.Instance().GetPermissionsByUserObject(ObjectId, UserId, PortalId, ModuleId), GetType(PermissionInfo))
        'End Function

        Public Shared Sub AddPermission(ByVal objPermission As PermissionInfo)
            DataProvider.Instance().AddPermission(objPermission.ObjectId, objPermission.UserId, objPermission.Locale, _
                                                   objPermission.ModuleId)
        End Sub

        Public Shared Sub UpdatePermission(ByVal objPermission As PermissionInfo)
            DataProvider.Instance().UpdatePermission(objPermission.PermissionId, objPermission.ObjectId, _
                                                      objPermission.UserId, objPermission.Locale, objPermission.ModuleId)
        End Sub

        Public Shared Sub DeletePermission(ByVal PermissionId As Integer)
            DataProvider.Instance().DeletePermission(PermissionId)
        End Sub

        Public Shared Function HasAccess(ByVal user As UserInfo, ByVal AdminRole As String, ByVal ModuleId As Integer, _
                                          ByVal ObjectId As Integer, ByVal Locale As String) As Boolean
            If user.IsSuperUser Or user.IsInRole(AdminRole) Then Return True
            Using ir As IDataReader = DataProvider.Instance().GetPermission(ObjectId, user.UserID, Locale, ModuleId)
                If ir.Read Then
                    Return True
                End If
            End Using
            Return False
        End Function
    End Class

#End Region
End Namespace
