using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeoLib.Client.Contracts;
using GeoLib.Contracts;
using GeoLib.Proxies;

namespace GeoLib.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeoClient _Proxy;

        public MainWindow()
        {
            InitializeComponent();

            _Proxy = new GeoClient("tcpEP");

            this.Title = "UI Running on Thread " + Thread.CurrentThread.ManagedThreadId +
                         " | Process " + Process.GetCurrentProcess().Id.ToString();
        }

        private void getInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (zipCodeTxt.Text != "")
            {
                // Metadata Exchange
                ServiceReference1.GeoServiceClient proxy = new ServiceReference1.GeoServiceClient("BasicHttpBinding_IGeoService");

                ZipCodeData data = proxy.GetZipInfo(zipCodeTxt.Text);

                if (data != null)
                {
                    cityLbl.Content = data.City;
                    stateLbl.Content = data.State;
                }

                //GeoClient proxy = new GeoClient("tcpEP");

                //ZipCodeData data = proxy.GetZipInfo(zipCodeTxt.Text);
                //if (data != null)
                //{
                //    cityLbl.Content = data.City;
                //    stateLbl.Content = data.State;
                //}

                //proxy.Close();
            }
        }

        private void getZipCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (stateTxt.Text != null)
            {
                //GeoClient proxy = new GeoClient("tcpEP");

                //proxy.Open();

                IEnumerable<ZipCodeData> data = _Proxy.GetZips(stateTxt.Text);
                if (data != null)
                {
                    zipCodesLst.ItemsSource = data;
                }

                //proxy.Close();

                /*
                // Setting proxy programmatically.
                EndpointAddress address = new EndpointAddress("http://localhost/GeoService");
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.MaxReceivedMessageSize = 2000000;

                GeoClient proxy = new GeoClient(binding, address);

                IEnumerable<ZipCodeData> data = proxy.GetZips(stateTxt.Text);
                if (data != null)
                {
                    zipCodesLst.ItemsSource = data;
                }

                proxy.Close();
                */
            }
        }

        private void makeCallBtn_Click(object sender, RoutedEventArgs e)
        {
            // Using ChannelFactory, endpoint/address added in code.
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:8010/MessageService");
            Binding binding = new NetTcpBinding();

            ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>(binding, address); // You can't just call the default constructor (it's a bug).
            IMessageService proxy = factory.CreateChannel();

            proxy.ShowMsg(textToShowTxt.Text);

            factory.Close();

            /*
            // Using ChannelFactory, note the endpoint is added to the App.config
            ChannelFactory<IMessageService> factory = new ChannelFactory<IMessageService>(""); // You can't just call the default constructor (it's a bug).
            IMessageService proxy = factory.CreateChannel();

            proxy.ShowMsg(textToShowTxt.Text);

            factory.Close();
            */

            /*
            // Using the MessageClient
            if (textToShowTxt.Text != "")
            {
                EndpointAddress address = new EndpointAddress("net.tcp://localhost:8010/MessageService");
                Binding binding = new NetTcpBinding();

                MessageClient proxy = new MessageClient(binding, address);

                proxy.ShowMessage(textToShowTxt.Text);

                proxy.Close();
            }
            */
        }
    }
}
