using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeoLib.Contracts;
using GeoLib.Proxies;

namespace GeoLib.Client2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _Proxy = new GeoClient("");
            _Proxy.Open();
            _SyncContext = SynchronizationContext.Current;
        }

        private GeoClient _Proxy = null;
        private SynchronizationContext _SyncContext = null;

        private async void getZipInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            if (zipCodeTxt.Text != "")
            {
                string zipCode = zipCodeTxt.Text;

                await Task.Run(() =>
                {
                    ZipCodeData data = _Proxy.GetZipInfo(zipCode);
                    if (data != null)
                    {
                        SendOrPostCallback callback = new SendOrPostCallback(arg =>
                        {
                            stateLbl.Content = data.State;
                            cityLbl.Content = data.City;
                        });

                        _SyncContext.Send(callback, null);
                    }
                });
            }
        }
    }
}
