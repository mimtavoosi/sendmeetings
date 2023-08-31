﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmsSender
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SmsSender.FastSendSoap")]
    public interface FastSendSoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/AutoSendCode", ReplyAction="*")]
        System.Threading.Tasks.Task<SmsSender.AutoSendCodeResponse> AutoSendCodeAsync(SmsSender.AutoSendCodeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SendMessageWithCode", ReplyAction="*")]
        System.Threading.Tasks.Task<SmsSender.SendMessageWithCodeResponse> SendMessageWithCodeAsync(SmsSender.SendMessageWithCodeRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CheckSendCode", ReplyAction="*")]
        System.Threading.Tasks.Task<SmsSender.CheckSendCodeResponse> CheckSendCodeAsync(SmsSender.CheckSendCodeRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AutoSendCodeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AutoSendCode", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.AutoSendCodeRequestBody Body;
        
        public AutoSendCodeRequest()
        {
        }
        
        public AutoSendCodeRequest(SmsSender.AutoSendCodeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AutoSendCodeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Password;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ReciptionNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Footer;
        
        public AutoSendCodeRequestBody()
        {
        }
        
        public AutoSendCodeRequestBody(string Username, string Password, string ReciptionNumber, string Footer)
        {
            this.Username = Username;
            this.Password = Password;
            this.ReciptionNumber = ReciptionNumber;
            this.Footer = Footer;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class AutoSendCodeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="AutoSendCodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.AutoSendCodeResponseBody Body;
        
        public AutoSendCodeResponse()
        {
        }
        
        public AutoSendCodeResponse(SmsSender.AutoSendCodeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class AutoSendCodeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public long AutoSendCodeResult;
        
        public AutoSendCodeResponseBody()
        {
        }
        
        public AutoSendCodeResponseBody(long AutoSendCodeResult)
        {
            this.AutoSendCodeResult = AutoSendCodeResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendMessageWithCodeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendMessageWithCode", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.SendMessageWithCodeRequestBody Body;
        
        public SendMessageWithCodeRequest()
        {
        }
        
        public SendMessageWithCodeRequest(SmsSender.SendMessageWithCodeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendMessageWithCodeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Password;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ReciptionNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Code;
        
        public SendMessageWithCodeRequestBody()
        {
        }
        
        public SendMessageWithCodeRequestBody(string Username, string Password, string ReciptionNumber, string Code)
        {
            this.Username = Username;
            this.Password = Password;
            this.ReciptionNumber = ReciptionNumber;
            this.Code = Code;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class SendMessageWithCodeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="SendMessageWithCodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.SendMessageWithCodeResponseBody Body;
        
        public SendMessageWithCodeResponse()
        {
        }
        
        public SendMessageWithCodeResponse(SmsSender.SendMessageWithCodeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class SendMessageWithCodeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public long SendMessageWithCodeResult;
        
        public SendMessageWithCodeResponseBody()
        {
        }
        
        public SendMessageWithCodeResponseBody(long SendMessageWithCodeResult)
        {
            this.SendMessageWithCodeResult = SendMessageWithCodeResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CheckSendCodeRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CheckSendCode", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.CheckSendCodeRequestBody Body;
        
        public CheckSendCodeRequest()
        {
        }
        
        public CheckSendCodeRequest(SmsSender.CheckSendCodeRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CheckSendCodeRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Username;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string Password;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string ReciptionNumber;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string Code;
        
        public CheckSendCodeRequestBody()
        {
        }
        
        public CheckSendCodeRequestBody(string Username, string Password, string ReciptionNumber, string Code)
        {
            this.Username = Username;
            this.Password = Password;
            this.ReciptionNumber = ReciptionNumber;
            this.Code = Code;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CheckSendCodeResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CheckSendCodeResponse", Namespace="http://tempuri.org/", Order=0)]
        public SmsSender.CheckSendCodeResponseBody Body;
        
        public CheckSendCodeResponse()
        {
        }
        
        public CheckSendCodeResponse(SmsSender.CheckSendCodeResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class CheckSendCodeResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool CheckSendCodeResult;
        
        public CheckSendCodeResponseBody()
        {
        }
        
        public CheckSendCodeResponseBody(bool CheckSendCodeResult)
        {
            this.CheckSendCodeResult = CheckSendCodeResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface FastSendSoapChannel : SmsSender.FastSendSoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class FastSendSoapClient : System.ServiceModel.ClientBase<SmsSender.FastSendSoap>, SmsSender.FastSendSoap
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public FastSendSoapClient(EndpointConfiguration endpointConfiguration) : 
                base(FastSendSoapClient.GetBindingForEndpoint(endpointConfiguration), FastSendSoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FastSendSoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(FastSendSoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FastSendSoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(FastSendSoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public FastSendSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SmsSender.AutoSendCodeResponse> SmsSender.FastSendSoap.AutoSendCodeAsync(SmsSender.AutoSendCodeRequest request)
        {
            return base.Channel.AutoSendCodeAsync(request);
        }
        
        public System.Threading.Tasks.Task<SmsSender.AutoSendCodeResponse> AutoSendCodeAsync(string Username, string Password, string ReciptionNumber, string Footer)
        {
            SmsSender.AutoSendCodeRequest inValue = new SmsSender.AutoSendCodeRequest();
            inValue.Body = new SmsSender.AutoSendCodeRequestBody();
            inValue.Body.Username = Username;
            inValue.Body.Password = Password;
            inValue.Body.ReciptionNumber = ReciptionNumber;
            inValue.Body.Footer = Footer;
            return ((SmsSender.FastSendSoap)(this)).AutoSendCodeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SmsSender.SendMessageWithCodeResponse> SmsSender.FastSendSoap.SendMessageWithCodeAsync(SmsSender.SendMessageWithCodeRequest request)
        {
            return base.Channel.SendMessageWithCodeAsync(request);
        }
        
        public System.Threading.Tasks.Task<SmsSender.SendMessageWithCodeResponse> SendMessageWithCodeAsync(string Username, string Password, string ReciptionNumber, string Code)
        {
            SmsSender.SendMessageWithCodeRequest inValue = new SmsSender.SendMessageWithCodeRequest();
            inValue.Body = new SmsSender.SendMessageWithCodeRequestBody();
            inValue.Body.Username = Username;
            inValue.Body.Password = Password;
            inValue.Body.ReciptionNumber = ReciptionNumber;
            inValue.Body.Code = Code;
            return ((SmsSender.FastSendSoap)(this)).SendMessageWithCodeAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<SmsSender.CheckSendCodeResponse> SmsSender.FastSendSoap.CheckSendCodeAsync(SmsSender.CheckSendCodeRequest request)
        {
            return base.Channel.CheckSendCodeAsync(request);
        }
        
        public async System.Threading.Tasks.Task<SmsSender.CheckSendCodeResponse> CheckSendCodeAsync(string Username, string Password, string ReciptionNumber, string Code)
        {
            SmsSender.CheckSendCodeRequest inValue = new SmsSender.CheckSendCodeRequest();
            inValue.Body = new SmsSender.CheckSendCodeRequestBody();
            inValue.Body.Username = Username;
            inValue.Body.Password = Password;
            inValue.Body.ReciptionNumber = ReciptionNumber;
            inValue.Body.Code = Code;
            return await ((SmsSender.FastSendSoap)(this)).CheckSendCodeAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.FastSendSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.FastSendSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.FastSendSoap))
            {
                return new System.ServiceModel.EndpointAddress("https://raygansms.com/FastSend.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.FastSendSoap12))
            {
                return new System.ServiceModel.EndpointAddress("https://raygansms.com/FastSend.asmx");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            FastSendSoap,
            
            FastSendSoap12,
        }
    }
}