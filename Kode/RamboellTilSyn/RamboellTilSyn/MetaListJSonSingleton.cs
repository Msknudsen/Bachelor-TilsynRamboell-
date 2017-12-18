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


        /// <summary>
        /// private constructor needs to be called from a public method
        /// </summary>
        /// <param name="path"></param>
        private MetaListJSonSingleton(string path)
        {
            _path = path;
            var txt = File.ReadAllText(path);
            PdfObjects = JsonConvert.DeserializeObject<List<PdfObjectDto>>(txt) ?? new List<PdfObjectDto>();
        }

        /// <summary>
        /// Getting a instancee from a speciffic path
        /// </summary>
        /// <param name="path"> the meta list json file path</param>
        /// <returns>an instance of this class</returns>
        public static  MetaListJSonSingleton GetInstance(string path)
        {
            return _instance ?? (_instance = new MetaListJSonSingleton(path));
        }

        /// <summary>
        /// Used to get a new instance of this class instead of the old one, use this if you need a new list from another file
        /// </summary>
        public void Reset()
        {
            _instance = null;
        }



        /// <summary>
        /// Will try get a instance if there is one, without the to specify which path 
        /// </summary>
        /// <returns> Returns null, when failing</returns>
        public static MetaListJSonSingleton TryGetInstance()
        {
            return _instance;
        }
    }
}
