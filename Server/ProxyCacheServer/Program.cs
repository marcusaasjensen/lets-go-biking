using System;
using System.ServiceModel.Description;
using System.ServiceModel;
using ProxyCacheServer.Proxy;

namespace RoutingServer
{
    internal static class Program
    {
        public static void Main()
        {
            var httpUrl = new Uri("http://localhost:8733/Design_Time_Addresses/ProxyCacheServer/JCDecauxService/");

            //Create service host
            var host = new ServiceHost(typeof(JCDecauxService), httpUrl);

            try
            {
                //Enable metadata exchange
                var smb = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };

                host.Description.Behaviors.Add(smb);

                //Start the Service
                host.Open();

                Console.WriteLine("Service is host at " + DateTime.Now);
                Console.WriteLine("Host is running... Press <Enter> key to stop");
                Console.ReadLine();

                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                host.Abort();
            }
        }
    }
}
