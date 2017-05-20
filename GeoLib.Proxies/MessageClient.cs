using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using GeoLib.Client.Contracts;

namespace GeoLib.Proxies
{
    public class MessageClient : ClientBase<IMessageService>, IMessageService
    {
        public MessageClient(Binding binding, EndpointAddress address)
            : base(binding, address)
        { }

        public void ShowMsg(string message)
        {
            Channel.ShowMsg(message);
        }
    }
}
