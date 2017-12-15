using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Newtonsoft.Json;

namespace Ramboell.iOS
{
    public class MetaListJSonSingleton
    {
        private static MetaListJSonSingleton instance;
        public List<PdfObject> PdfObjects { get; private set; }

        private MetaListJSonSingleton(string path)
        {
           var txt =  File.ReadAllText(path);
            PdfObjects = JsonConvert.DeserializeObject<List<PdfObject>>(txt);
        }

        private void NewMetaFile(string path)
        {
            var txt = File.ReadAllText(path);
            PdfObjects = JsonConvert.DeserializeObject<List<PdfObject>>(txt);
        }
        public static MetaListJSonSingleton GetInstance(string path)
        {
            return instance ?? (instance = new MetaListJSonSingleton(path));
        }

        public List<PdfObject> TryGetNewMetaList(string path)
        {
            {
                if (instance == null) return null;

                NewMetaFile(path);
                return instance.PdfObjects;
            }
        }
    }
}
