using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Analytics;
using Foundation;
using UIKit;
using UserNotifications;

namespace Ramboell.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register("AppDelegate")]
	public partial class AppDelegate : 
        global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
	{
	    protected AppDelegate()
	    {
	        // Register your app for remote notifications.
	        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
	        {
	            // iOS 10 or later
	            var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge |
	                              UNAuthorizationOptions.Sound;
	            UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
	            {
	                Console.WriteLine(granted);
	            });

	            // For iOS 10 display notification (sent via APNS)
	            UNUserNotificationCenter.Current.Delegate = this;

	            // For iOS 10 data message (sent via FCM)
	            //Messaging.SharedInstance.RemoteMessageDelegate = this;
	        }

	    }

	    //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			Xamarin.Forms.Forms.Init ();
			LoadApplication (new Ramboell.App ());
		    Firebase.Core.App.Configure();
            return base.FinishedLaunching (app
                , options);
		}
	}
}
