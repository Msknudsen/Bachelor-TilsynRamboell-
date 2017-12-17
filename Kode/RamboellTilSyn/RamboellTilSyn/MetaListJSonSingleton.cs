using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using Newtonsoft.Json;

namespace Ramboell.iOS
{
    /// <summary>
    /// Singleton for loading json from a file into a list as draw from MarkPdfPage would be called multiple times
    /// </summary>
    public class MetaListJSonSingleton
    {
        private static MetaListJSonSingleton _instance;
        private string _path ="";
        public List<PdfObjectDto> PdfObjects { get; private set; }

        private MetaListJSonSingleton(string path)
        {
            _path = path;
            var txt = File.ReadAllText(path);
            PdfObjects = JsonConvert.DeserializeObject<List<PdfObjectDto>>(txt) ?? new List<PdfObjectDto>();
        }
        public static  MetaListJSonSingleton GetInstance(string path)
        {
            return _instance ?? (_instance = new MetaListJSonSingleton(path));
        }
        public void Reset()
        {
            _instance = null;
        }


        public static MetaListJSonSingleton TryGetInstance()
        {
            return _instance;
        }
    }
}
