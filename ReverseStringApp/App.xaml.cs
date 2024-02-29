using ReverseStringApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ReverseStringApp
{
    sealed partial class App : Application
    {
        public static BackgroundTaskDeferral AppServiceDeferral = null;
        public static AppServiceConnection Connection = null;
        public static event EventHandler AppServiceDisconnected;
        public static event EventHandler<AppServiceTriggerDetails> AppServiceConnected;
        public static bool IsForeground = false;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.EnteredBackground += App_EnteredBackground;
            this.LeavingBackground += App_LeavingBackground;
        }

        private void App_LeavingBackground(object sender, LeavingBackgroundEventArgs e)
        {
            IsForeground = true;
        }

        private void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            IsForeground = false;
        }

        // Register the connection to the fulltrust process
        protected override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            base.OnBackgroundActivated(args);

            if (args.TaskInstance.TriggerDetails is AppServiceTriggerDetails details)
            {
                // only accept connections from callers in the same package
                if (details.CallerPackageFamilyName == Package.Current.Id.FamilyName)
                {
                    // connection established from the fulltrust process
                    AppServiceDeferral = args.TaskInstance.GetDeferral();
                    args.TaskInstance.Canceled += OnTaskCanceled;

                    // Store a reference of the object so that UWP can send messages to the desktop app
                    Connection = details.AppServiceConnection;
                    AppServiceConnected?.Invoke(this, args.TaskInstance.TriggerDetails as AppServiceTriggerDetails);
                }
            }
        }

        // When the app service is canceled, clean up everything
        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            AppServiceDeferral?.Complete();
            AppServiceDeferral = null;
            Connection = null;
            AppServiceDisconnected?.Invoke(this, null);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
