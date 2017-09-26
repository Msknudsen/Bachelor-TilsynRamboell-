using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Firebase.Analytics;
using Firebase.Database;
using Foundation;
using UIKit;
using UserNotifications;

namespace Ramboell.iOS
{
    //https://components.xamarin.com/gettingstarted/firebaseiosanalytics last seen 22/9
    public class FireBaseDataService: FirebaseService
    {
        DatabaseReference _rootNode;

        public FireBaseDataService()
        {
            _rootNode = Database.DefaultInstance.GetRootReference();
        }

        public void Post<T>(T obj)
        {
            
        }
        public T Get<T>(string objId)
        {
            throw new InvalidOperationException();
        }

        public ICollection<T> GetAll<T>()
        {
            return new Collection<T>();
        }

        public bool Put<T>(T obj)
        {
            return false;
        }
    }
}