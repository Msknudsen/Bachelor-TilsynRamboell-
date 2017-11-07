using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace Ramboell

{

    public class CustomPdfWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create("Uri",
            typeof(string),
            typeof(CustomPdfWebView),
            default(string));

        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }
    }
}
