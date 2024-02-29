using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private AppServiceConnection connection = null;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAppServiceConnection();
            this.Loaded += MainWindow_Loaded;
        }

        // When the window is loaded, we hide it. The window is not used for anything, but we need it to keep the process running.
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        // Initialize the connection to the UWP app service, and register the event handlers.
        private async void InitializeAppServiceConnection()
        {
            connection = new AppServiceConnection();
            connection.AppServiceName = "interopService";
            connection.PackageFamilyName = Package.Current.Id.FamilyName;
            connection.RequestReceived += Connection_RequestReceived;
            connection.ServiceClosed += Connection_ServiceClosed;

            AppServiceConnectionStatus status = await connection.OpenAsync();
            if (status != AppServiceConnectionStatus.Success)
            {
                // something went wrong ...
                // MessageBox.Show(status.ToString());
                this.IsEnabled = false;
            }
        }

        // When connection to UWP app service is closed, we shut down the desktop process as well.
        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // connection to the UWP lost, so we shut down the desktop process
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Application.Current.Shutdown();
            }));
        }

        // When UWP app service sends a request, we handle it here.
        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // MessageBox.Show("Got called");
            AppServiceDeferral messageDeferral = args.GetDeferral();
            ValueSet messageReceived = args.Request.Message;
            ValueSet messageReturned = new ValueSet();

            if (messageReceived.ContainsKey("Request"))
            {
                if (messageReceived["Request"] as string == "ReverseString")
                {
                    string input = messageReceived["Input"] as string;
                    messageReturned.Add("Result", reverseString(input));
                    messageReturned.Add("Status", "OK");
                }
                else
                {
                    messageReturned.Add("Status", "Fail: request not supported");
                }
            }
            else
            {
                messageReturned.Add("Status", "Fail: unknown command");
            }

            // Send the response asynchronously
            try
            {
                await args.Request.SendResponseAsync(messageReturned);
            }
            catch (Exception e)
            {
                // Your exception handling code here.
            }
            finally
            {
                // Complete the message deferral so that the platform knows that we're done responding.
                messageDeferral.Complete();
            }
        }

        // Reverse the string
        private string reverseString(string input)
        {
            char[] inputArray = input.ToCharArray();
            Array.Reverse(inputArray);
            return new string(inputArray);
        }
    }
}
