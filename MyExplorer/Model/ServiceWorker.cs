using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyExplorer.Model
{
    public static class ServiceWorker
    {
        
        public static List<ServiceController> GetAllServices()
        {
            return new List<ServiceController>(ServiceController.GetServices());
        }
    }
}
