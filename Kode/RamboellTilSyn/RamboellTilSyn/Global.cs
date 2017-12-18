using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Ramboell.iOS
{

    /// <summary>
    /// A place for variables which doesn't belong one place or belongs to multiple places
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// The width used for showing PDF, this will change from device to device 
        /// </summary>
        public static nfloat PdfViewWidth = 0;
        /// <summary>
        /// The height used for showing PDF, this will change from device to device 
        /// </summary>
        public static nfloat PdfViewHeight = 0;
        /// <summary>
        /// the name for firebase node which is placed in multiple places
        /// </summary>
        public const string Pdf = "pdf";
        /// <summary>
        /// the name for firebase node which is placed in multiple places
        /// </summary>
        public const string User = "user";
        /// <summary>
        /// standard name for list of pdf
        /// </summary>
        public const string ProjectListFileName = "list.json";
        /// <summary>
        /// Naming string
        /// </summary>
        public const string AddUserLabel = "Tilføj bruger";
        /// <summary>
        /// Naming string
        /// </summary>
        public const string AddProjectLabel = "Tilføj projekt";
        /// <summary>
        /// Key used by keychain
        /// </summary>
        public static string CurrentUserKey => "SignedInUser";
        /// <summary>
        /// Key used by keychain
        /// </summary>
        public static string CurrentPassKey => "SignedInUserPassword";
    }

}