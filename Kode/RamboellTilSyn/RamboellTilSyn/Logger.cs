using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firebase.Analytics;
using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    /// <summary>
    /// Logging events from all devices onto firebase and watch them on console 
    /// </summary>
    public class Logger
    {
        public static void Log(string key,string description)
        {
            NSString[] keys = { ParameterNamesConstants.ContentType, ParameterNamesConstants.ItemId };
            NSObject[] values = { new NSString(key), new NSString(description) };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);
            Analytics.LogEvent(EventNamesConstants.SelectContent, parameters);
        }
        public static void Log(string eventName,string key, string description)
        {
            NSString[] keys = { new NSString(key) };
            NSObject[] values = { new NSString(description) };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);
            Analytics.LogEvent(eventName, parameters);
        }
    }
}