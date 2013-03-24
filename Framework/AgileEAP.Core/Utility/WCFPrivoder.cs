using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace AgileEAP.Core
{
    public class WCFConfig
    {
        public string Binding
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }

        public bool HasAuth()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Domain);
        }
    }

    public class WCFProvider
    {
        private static Binding CreateBinding(string binding)
        {
            Binding bindinginstance = null;
            if (binding.ToLower() == "basichttpbinding" || string.IsNullOrEmpty(binding))
            {
                BasicHttpBinding ws = new BasicHttpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "netnamedpipebinding")
            {
                NetNamedPipeBinding ws = new NetNamedPipeBinding();
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "netpeertcpbinding")
            {
                NetPeerTcpBinding ws = new NetPeerTcpBinding();
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "nettcpbinding")
            {
                NetTcpBinding ws = new NetTcpBinding();
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Mode = SecurityMode.None;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "wsdualhttpbinding")
            {
                WSDualHttpBinding ws = new WSDualHttpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "wsfederationhttpbinding")
            {
                WSFederationHttpBinding ws = new WSFederationHttpBinding();
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding.ToLower() == "wshttpbinding")
            {
                WSHttpBinding ws = new WSHttpBinding();//SecurityMode.None);
                ws.MaxReceivedMessageSize = 65535000;
                ws.ReaderQuotas.MaxArrayLength = 55535000;
                //ws.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                //ws.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                bindinginstance = ws;
            }

            bindinginstance.CloseTimeout = TimeSpan.FromMinutes(1);
            bindinginstance.OpenTimeout = TimeSpan.FromMinutes(1);
            bindinginstance.SendTimeout = TimeSpan.FromMinutes(10);
            bindinginstance.ReceiveTimeout = TimeSpan.FromMinutes(10);
            return bindinginstance;

        }

        private static void CloseClient<TClient>(ChannelFactory<TClient> factory, TClient client)
        {
            if (client != null && client is ICommunicationObject)
            {
                ICommunicationObject clientChannel = ((ICommunicationObject)client);
                try
                {
                    if (clientChannel.State != System.ServiceModel.CommunicationState.Faulted)
                    {
                        clientChannel.Close();
                    }
                }
                catch (Exception ex)
                {
                    AgileEAP.Core.GlobalLogger.Error<WCFProvider>(ex);
                    clientChannel.Abort();
                }
                clientChannel = null;
            }

            if (factory != null)
            {
                try
                {
                    factory.Close();
                }
                catch (Exception ex)
                {
                    AgileEAP.Core.GlobalLogger.Error<WCFProvider>(ex);
                    factory.Abort();
                }
                factory = null;
            }
        }

        private static ChannelFactory<TClient> CreateChannelFactory<TClient>(WCFConfig config)
        {
            if (string.IsNullOrEmpty(config.URL)) throw new NotSupportedException("this url is Null or Empty!");

            ChannelFactory<TClient> factory = null;
            if (config.HasAuth())
            {
                EndpointIdentity identity = EndpointIdentity.CreateUpnIdentity(string.Format("{0}@{1}", config.UserName, config.Domain));
                EndpointAddress address = new EndpointAddress(new Uri(config.URL), identity);
                Binding binding = CreateBinding(config.Binding);
                ServiceEndpoint endpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(TClient)), binding, address);
                factory = new ChannelFactory<TClient>(endpoint);
            }
            else
            {
                EndpointAddress address = new EndpointAddress(config.URL);
                Binding binding = CreateBinding(config.Binding);
                factory = new ChannelFactory<TClient>(binding, address);
            }

            if (config.Binding == "wshttpbinding" && config.HasAuth())
            {
                factory.Credentials.Windows.ClientCredential = new NetworkCredential(config.UserName, config.Password, config.Domain);
            }

            return factory;
        }

        public static TResult Execute<TClient, TResult>(WCFConfig config, Func<TClient, TResult> func)
        {
            try
            {
                ChannelFactory<TClient> factory = CreateChannelFactory<TClient>(config);
                TClient client = factory.CreateChannel();
                try
                {
                    return func(client);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    CloseClient(factory, client);
                }
            }
            catch (Exception ex)
            {
                AgileEAP.Core.GlobalLogger.Error<WCFProvider>(ex);
                throw;
            }
        }

        public static void Execute<TClient>(WCFConfig config, Action<TClient> func)
        {
            try
            {
                ChannelFactory<TClient> factory = CreateChannelFactory<TClient>(config);
                TClient client = factory.CreateChannel();
                try
                {
                    func(client);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    CloseClient(factory, client);
                }
            }
            catch (Exception ex)
            {
                AgileEAP.Core.GlobalLogger.Error<WCFProvider>(ex);
                throw;
            }
        }
    }
}

