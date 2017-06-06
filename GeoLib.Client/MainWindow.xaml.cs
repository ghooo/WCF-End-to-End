using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
            //_Proxy = new StatefulGeoClient();

            this.Title = "UI Running on Thread " + Thread.CurrentThread.ManagedThreadId +
                         " | Process " + Process.GetCurrentProcess().Id.ToString();
        }

        private void getInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (zipCodeTxt.Text != "")
            {
                try
                {
                    ZipCodeData data = _Proxy.GetZipInfo(zipCodeTxt.Text);

                    if (data != null)
                    {
                        cityLbl.Content = data.City;
                        stateLbl.Content = data.State;
                    }
                }
                catch (FaultException<ExceptionDetail> ex)
                {
                    MessageBox.Show("FaultException<ExceptionDetail> thrown by service.\n\rException type: " +
                                    ex.GetType().Name + "\n\r" +
                                    "Message: " + ex.Message + "\n\r" +
                                    "Proxy state: " + _Proxy.State.ToString());
                }
                catch (FaultException<ApplicationException> ex)
                {
                    MessageBox.Show("FaultException<ApplicationException> thrown by service.\n\rException type: " +
                                    ex.GetType().Name + "\n\r" +
                                    "Reason: " + ex.Message + "\n\r" +
                                    "Message: " + ex.Detail.Message + "\n\r" +
                                    "Proxy state: " + _Proxy.State.ToString());
                }
                catch (FaultException<NotFoundData> ex)
                {
                    MessageBox.Show("FaultException<NotFoundData> thrown by service.\n\rException type: " +
                                    ex.GetType().Name + "\n\r" +
                                    "Reason: " + ex.Message + "\n\r" +
                                    "Message: " + ex.Detail.Message + "\n\r" +
                                    "Time: " + ex.Detail.When + "\n\r" +
                                    "User: " + ex.Detail.User + "\n\r" +
                                    "Proxy state: " + _Proxy.State.ToString());
                }
                catch (FaultException ex)
                {
                    MessageBox.Show("FaultException thrown by service.\n\rException type: " +
                                    ex.GetType().Name + "\n\r" +
                                    "Message: " + ex.Message + "\n\r" +
                                    "Proxy state: " + _Proxy.State.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception thrown by service.\n\rException type: " +
                                    ex.GetType().Name + "\n\r" +
                                    "Message: " + ex.Message + "\n\r" +
                                    "Proxy state: " + _Proxy.State.ToString());
                }

                //// Instancing and concurrency
                //ZipCodeData data = _Proxy.GetZipInfo();
                //if (data != null)
                //{
                //    cityLbl.Content = data.City;
                //    stateLbl.Content = data.State;
                //}

                //// Metadata Exchange
                //ServiceReference1.GeoServiceClient proxy = new ServiceReference1.GeoServiceClient("BasicHttpBinding_IGeoService");

                //ZipCodeData data = proxy.GetZipInfo(zipCodeTxt.Text);

                //if (data != null)
                //{
                //    cityLbl.Content = data.City;
                //    stateLbl.Content = data.State;
                //}

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
            throw new NotImplementedException();
            //if (stateTxt.Text != null)
            //{
            //    //GeoClient proxy = new GeoClient("tcpEP");

            //    //proxy.Open();

            //    IEnumerable<ZipCodeData> data = _Proxy.GetZips(stateTxt.Text);
            //    if (data != null)
            //    {
            //        zipCodesLst.ItemsSource = data;
            //    }

            //    //proxy.Close();

            //    /*
            //    // Setting proxy programmatically.
            //    EndpointAddress address = new EndpointAddress("http://localhost/GeoService");
            //    BasicHttpBinding binding = new BasicHttpBinding();
            //    binding.MaxReceivedMessageSize = 2000000;

            //    GeoClient proxy = new GeoClient(binding, address);

            //    IEnumerable<ZipCodeData> data = proxy.GetZips(stateTxt.Text);
            //    if (data != null)
            //    {
            //        zipCodesLst.ItemsSource = data;
            //    }

            //    proxy.Close();
            //    */
            //}
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

        private void pushBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (zipCodeTxt.Text != "")
            //{
            //    _Proxy.PushZip(zipCodeTxt.Text);
            //}
        }

        private void getInRangeBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (zipCodeTxt != null && rangeTxt.Text != "")
            //{
            //    IEnumerable<ZipCodeData> data = _Proxy.GetZips(int.Parse(rangeTxt.Text));
            //    if (data != null)
            //    {
            //        zipCodesLst.ItemsSource = data;
            //    }
            //}
        }

        private void updateBatchBtn_Click(object sender, RoutedEventArgs e)
        {
            List<ZipCityData> cityZipList = new List<ZipCityData>()
            {
                new ZipCityData() { ZipCode = "07035", City = "Bedrock" },
                new ZipCityData() { ZipCode = "33030", City = "End of the World" }
            };

            try
            {
                GeoClient proxy = new GeoClient("tcpEP");

                using (TransactionScope scope = new TransactionScope())
                {
                    proxy.UpdateZipCity(cityZipList);
                    proxy.Close();

                    throw new ApplicationException("uh oh");
                    scope.Complete();
                }
                proxy.Close();

                MessageBox.Show("Updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
      }

        private void putBackBtn_Click(object sender, RoutedEventArgs e)
        {
            List<ZipCityData> cityZipList = new List<ZipCityData>()
            {
                new ZipCityData() { ZipCode = "07035", City = "Lincoln Park" },
                new ZipCityData() { ZipCode = "33030", City = "Homestead" }
            };

            try
            {
                _Proxy.UpdateZipCity(cityZipList);

                MessageBox.Show("Updated.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
    }
}
