using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Contracts;
using GeoLib.Services;

namespace GeoLib.ConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost hostGeoManager = new ServiceHost(typeof(GeoManager));

            //ServiceDebugBehavior behavior = hostGeoManager.Description.Behaviors.Find<ServiceDebugBehavior>();
            //if (behavior == null)
            //{
            //    Console.WriteLine("Behavior Missing!");
            //    behavior = new ServiceDebugBehavior();
            //    behavior.IncludeExceptionDetailInFaults = true;
            //    hostGeoManager.Description.Behaviors.Add(behavior);
            //}
            //else
            //{
            //    Console.WriteLine("Behavior Found!");
            //    behavior.IncludeExceptionDetailInFaults = true;
            //}

            //string address = "net.tcp://localhost:8009/GeoService";
            //Binding binding = new NetTcpBinding();
            //Type contract = typeof(IGeoService);

            //hostGeoManager.AddServiceEndpoint(contract, binding, address);

            hostGeoManager.Open(); 

            Console.WriteLine("Services started. Press [Enter] to exit.");
            Console.ReadLine();

            hostGeoManager.Close();
        }
    }
}
