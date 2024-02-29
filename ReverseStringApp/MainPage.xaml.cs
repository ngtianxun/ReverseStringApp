using System;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.AppService;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using Windows.Foundation.Collections;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReverseStringApp
{
    public sealed partial class MainPage : Page
    {
        private AppServiceConnection reverseStringService;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }

        // Randamize input string
        private void randomizeInput_Click(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            string input = "";
            for (int i = 0; i < 5; i++)
            {
                input += (char)rand.Next(65, 90);
            }
      
            InputTextBoxIn.Text = input;
        }

        // Obtain input string and pass it to backend
        private async void enter_Click(object sender, RoutedEventArgs e)
        {
            string input = InputTextBoxIn.Text;

            // Check if input is empty
            if (input == "")
            {
                // Trigger notification to show user that input is empty
                var dialog = new Windows.UI.Popups.MessageDialog("Please enter a string");

                // Show notification
                _ = dialog.ShowAsync();
                return;
            }

            var (output, status) = await passStringToBackend(input);

            if (status.Contains("Fail"))
            {
                // Trigger notification to show user that there was an error
                var dialog = new Windows.UI.Popups.MessageDialog(output);

                // Show notification
                _ = dialog.ShowAsync();
                return;
            }
            OutputTextBoxOut.Text = output;
        }

        // Clear input and output text boxes
        private void clear_Click(object sender, RoutedEventArgs e)
        {
            InputTextBoxIn.Text = "";
            OutputTextBoxOut.Text = "";
        }

        // Use IPC/Appservices to send input string to backend to reverse string
        private async Task<(string, string)> passStringToBackend(string input)
        {
            // TODO: Pipe approach, not working, permission denied issue
            /*System.IO.Pipes.NamedPipeClientStream pipeClient = new System.IO.Pipes.NamedPipeClientStream(".",
      "testpipe",
      PipeDirection.InOut);

            try
            {
                if (pipeClient.IsConnected != true) 
                { 
                    pipeClient.Connect();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Error";
            }

            StreamReader sr = new StreamReader(pipeClient);
            StreamWriter sw = new StreamWriter(pipeClient);

            string temp;
            temp = sr.ReadLine();

            if (temp == "Waiting")
            {
                try
                {
                    sw.WriteLine("Test Message");
                    sw.Flush();
                    pipeClient.Close();
                }
                catch (Exception ex) { throw ex; }
            }

            return "Error establishing connection with IPC backend";*/

            // Build request dataframe 
            var message = new ValueSet();
            message.Add("Request", "ReverseString");
            message.Add("Input", input);

            // Using AppServiceConnection that is defined in the App class to send the message to full trust background process
            AppServiceResponse response = await App.Connection.SendMessageAsync(message);
            string result = "";

            if (response.Status == AppServiceResponseStatus.Success)
            {
                result = response.Message["Result"] as string;
            }

            return (result, response.Message["Status"] as string);
        }
    }
}
