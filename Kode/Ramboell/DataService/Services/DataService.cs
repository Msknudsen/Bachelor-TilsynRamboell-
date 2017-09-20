using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Xamarin.Forms;

namespace DataService
{
    public class DataService
    {
        private FirebaseClient _firebase;

        public DataService(Uri databaseUri)
        {
            _firebase = new FirebaseClient(databaseUri.ToString());
        }

        public void Post<T>(T obj)
        {
            
        }
        public async Task<ICollection<T>> GetAll<T>()
        {
            var items = await _firebase
                .Child(typeof(T).Name)
                //.WithAuth("<Authentication Token>") // <-- Add Auth token if required. Auth instructions further down in readme.
                .OrderByKey()
                //.LimitToFirst()
                .OnceAsync<T>().ConfigureAwait(false);

            //TODO need more info on what the firebase is actually returning before i can tell more
            return (ICollection<T>)items;
        }
        public async Task<ICollection<T>> Get<T>(T obj)
        {
            var items = await _firebase
                .Child(typeof(T).Name)
                //.WithAuth("<Authentication Token>") // <-- Add Auth token if required. Auth instructions further down in readme.
                .OrderByKey()
                //.LimitToFirst()
                .OnceAsync<T>().ConfigureAwait(false);

            //TODO need more info on what the firebase is actually returning before i can tell more
            return (ICollection<T>)items;
        }

    }
}
