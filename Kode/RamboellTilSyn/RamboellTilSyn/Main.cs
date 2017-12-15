using System;
using System.ComponentModel;
using UIKit;

namespace Ramboell.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            var instanceId = Guid.NewGuid();
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Logger.Log("Error", instanceId.ToString(),e.Message);
            }
        }
    }
}