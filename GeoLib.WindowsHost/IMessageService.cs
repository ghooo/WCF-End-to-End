using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GeoLib.WindowsHost.Contracts
{
    // We added the name space into the properties/assemblyinfo.cs so we don't have to mention the namespace here.
    //[ServiceContract(Namespace = "https://github.com/ghooo/WcfEndToEnd")]
    [ServiceContract]
    public interface IMessageService
    {
        [OperationContract]
        void ShowMessage(string message);
    }
}
