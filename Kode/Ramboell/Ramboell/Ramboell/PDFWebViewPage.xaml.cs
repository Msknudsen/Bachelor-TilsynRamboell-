using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Xamarin.Forms;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ramboell
{
    /// <inheritdoc />
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class PdfWebViewPage : ContentPage
    {
       
        private int ColumnCount => 5;
        public PdfWebViewPage()
        {
            var plainButton = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex ("#eee") },
                    new Setter { Property = Button.TextColorProperty, Value = Color.Black },
                    new Setter { Property = Button.BorderRadiusProperty, Value = 0 },
                    new Setter { Property = Button.FontSizeProperty, Value = 40 }
                }
            };
            Padding = new Thickness(0, 20, 0, 0);
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(7, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            var customPdfWebView = new CustomPdfWebView()
            {
                Uri = "Sample.pdf",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(customPdfWebView, 0, 0);
            Grid.SetColumnSpan(customPdfWebView, ColumnCount);
            /*
            for (var i = 0; i < ColumnCount; i++)
            {
                grid.Children.Add(new Button { Text = ++i + "", Style = plainButton }, i, 1);
            }*/
            grid.Children.Add(new Button { Text = "1", Style = plainButton }, 0, 1);
            grid.Children.Add(new Button { Text ="2", Style = plainButton }, 1, 1);
            grid.Children.Add(new Button { Text = "3", Style = plainButton }, 2, 1);
            grid.Children.Add(new Button { Text = "4", Style = plainButton }, 3, 1);
            grid.Children.Add(new Button { Text = "5", Style = plainButton }, 4, 1);

            Content = grid;
        }
    }
}
