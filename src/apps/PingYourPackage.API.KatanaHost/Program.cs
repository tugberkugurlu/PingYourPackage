using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PingYourPackage.API.KatanaHost {

    class Program {

        static void Main(string[] args) {

            // TODO: Open both URIs (HTTP and HTTPS).
            // TODO: Use new ASP.NET Web API integration package. 
            //       They have different API.

            using (WebApp.Start<Startup>()) {

                Console.WriteLine("Started on ");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }
    }
}