﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace getAccess
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="checkaccessBinding", [Namespace]:="http://msg.org")>  _
    Partial Public Class checkaccess
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private CallcheckaccessOperationCompleted As System.Threading.SendOrPostCallback
        
        Private syncaccountOperationCompleted As System.Threading.SendOrPostCallback
        
        Private syncdetailOperationCompleted As System.Threading.SendOrPostCallback
        
        Private getCompanyTypeOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.My.MySettings.Default.AgoraLegacy_getAccess_checkaccess
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event CallcheckaccessCompleted As CallcheckaccessCompletedEventHandler
        
        '''<remarks/>
        Public Event syncaccountCompleted As syncaccountCompletedEventHandler
        
        '''<remarks/>
        Public Event syncdetailCompleted As syncdetailCompletedEventHandler
        
        '''<remarks/>
        Public Event getCompanyTypeCompleted As getCompanyTypeCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("http://localhost/interface/procurement_interface/getAccess.php/checkaccess", RequestNamespace:="http://soapinterop.org/", ResponseNamespace:="http://soapinterop.org/")>  _
        Public Function Callcheckaccess(ByVal compid As String) As <System.Xml.Serialization.SoapElementAttribute("access")> String
            Dim results() As Object = Me.Invoke("Callcheckaccess", New Object() {compid})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub CallcheckaccessAsync(ByVal compid As String)
            Me.CallcheckaccessAsync(compid, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub CallcheckaccessAsync(ByVal compid As String, ByVal userState As Object)
            If (Me.CallcheckaccessOperationCompleted Is Nothing) Then
                Me.CallcheckaccessOperationCompleted = AddressOf Me.OnCallcheckaccessOperationCompleted
            End If
            Me.InvokeAsync("Callcheckaccess", New Object() {compid}, Me.CallcheckaccessOperationCompleted, userState)
        End Sub
        
        Private Sub OnCallcheckaccessOperationCompleted(ByVal arg As Object)
            If (Not (Me.CallcheckaccessCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent CallcheckaccessCompleted(Me, New CallcheckaccessCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("http://localhost/interface/procurement_interface/getAccess.php/syncaccount", RequestNamespace:="http://soapinterop.org/", ResponseNamespace:="http://soapinterop.org/")>  _
        Public Function syncaccount(ByVal compid As String, ByVal password As String) As <System.Xml.Serialization.SoapElementAttribute("sync")> String
            Dim results() As Object = Me.Invoke("syncaccount", New Object() {compid, password})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub syncaccountAsync(ByVal compid As String, ByVal password As String)
            Me.syncaccountAsync(compid, password, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub syncaccountAsync(ByVal compid As String, ByVal password As String, ByVal userState As Object)
            If (Me.syncaccountOperationCompleted Is Nothing) Then
                Me.syncaccountOperationCompleted = AddressOf Me.OnsyncaccountOperationCompleted
            End If
            Me.InvokeAsync("syncaccount", New Object() {compid, password}, Me.syncaccountOperationCompleted, userState)
        End Sub
        
        Private Sub OnsyncaccountOperationCompleted(ByVal arg As Object)
            If (Not (Me.syncaccountCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent syncaccountCompleted(Me, New syncaccountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("http://localhost/interface/procurement_interface/getAccess.php/syncdetail", RequestNamespace:="http://soapinterop.org/", ResponseNamespace:="http://soapinterop.org/")>  _
        Public Function syncdetail(ByVal compid As String) As <System.Xml.Serialization.SoapElementAttribute("detail")> String
            Dim results() As Object = Me.Invoke("syncdetail", New Object() {compid})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub syncdetailAsync(ByVal compid As String)
            Me.syncdetailAsync(compid, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub syncdetailAsync(ByVal compid As String, ByVal userState As Object)
            If (Me.syncdetailOperationCompleted Is Nothing) Then
                Me.syncdetailOperationCompleted = AddressOf Me.OnsyncdetailOperationCompleted
            End If
            Me.InvokeAsync("syncdetail", New Object() {compid}, Me.syncdetailOperationCompleted, userState)
        End Sub
        
        Private Sub OnsyncdetailOperationCompleted(ByVal arg As Object)
            If (Not (Me.syncdetailCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent syncdetailCompleted(Me, New syncdetailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapRpcMethodAttribute("http://localhost/interface/procurement_interface/getAccess.php/getCompanyType", RequestNamespace:="http://soapinterop.org/", ResponseNamespace:="http://soapinterop.org/")>  _
        Public Function getCompanyType(ByVal compid As String) As <System.Xml.Serialization.SoapElementAttribute("comptype")> String
            Dim results() As Object = Me.Invoke("getCompanyType", New Object() {compid})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub getCompanyTypeAsync(ByVal compid As String)
            Me.getCompanyTypeAsync(compid, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub getCompanyTypeAsync(ByVal compid As String, ByVal userState As Object)
            If (Me.getCompanyTypeOperationCompleted Is Nothing) Then
                Me.getCompanyTypeOperationCompleted = AddressOf Me.OngetCompanyTypeOperationCompleted
            End If
            Me.InvokeAsync("getCompanyType", New Object() {compid}, Me.getCompanyTypeOperationCompleted, userState)
        End Sub
        
        Private Sub OngetCompanyTypeOperationCompleted(ByVal arg As Object)
            If (Not (Me.getCompanyTypeCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent getCompanyTypeCompleted(Me, New getCompanyTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")>  _
    Public Delegate Sub CallcheckaccessCompletedEventHandler(ByVal sender As Object, ByVal e As CallcheckaccessCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CallcheckaccessCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")>  _
    Public Delegate Sub syncaccountCompletedEventHandler(ByVal sender As Object, ByVal e As syncaccountCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class syncaccountCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")>  _
    Public Delegate Sub syncdetailCompletedEventHandler(ByVal sender As Object, ByVal e As syncdetailCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class syncdetailCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0")>  _
    Public Delegate Sub getCompanyTypeCompletedEventHandler(ByVal sender As Object, ByVal e As getCompanyTypeCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4161.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class getCompanyTypeCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
End Namespace