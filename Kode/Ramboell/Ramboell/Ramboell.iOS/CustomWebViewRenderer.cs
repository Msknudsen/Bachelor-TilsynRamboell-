using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Foundation;
using Ramboell;
using Ramboell.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPdfWebView), typeof(CustomWebViewRenderer))]
namespace Ramboell.iOS
{
    public class CustomWebViewRenderer : ViewRenderer<CustomPdfWebView, UIWebView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CustomPdfWebView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SetNativeControl(new UIWebView());
            }
            if (e.OldElement != null)
            {
                // Cleanup
            }
            if (e.NewElement != null)
            {
                var customWebView = Element;
                
                var fileName = Path.Combine(NSBundle.MainBundle.BundlePath,
                    $"Content/{WebUtility.UrlEncode(customWebView.Uri)}");
                Control.LoadRequest(new NSUrlRequest(new NSUrl(fileName, false)));

                Control.ScalesPageToFit = true;

            }
        }
    }
}
