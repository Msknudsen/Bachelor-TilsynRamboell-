using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Ramboell.iOS
{
    /// <summary>
    /// Not implemented yet, to be added
    /// </summary>
    class DocumentPicker: UIDocument
    {
        protected DocumentPicker(NSObjectFlag t) : base(t)
        {
        }

        protected internal DocumentPicker(IntPtr handle) : base(handle)
        {
        }

        public DocumentPicker(NSUrl url) : base(url)
        {
        }
    }
}