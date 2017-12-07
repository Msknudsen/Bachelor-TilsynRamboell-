using System;
using System.IO;
using CoreGraphics;
using Firebase.Storage;
using Foundation;
using PdfKit;
using UIKit;
//https://components.xamarin.com/gettingstarted/firebaseiosstorage#download-files
namespace Ramboell.iOS
{
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class PdfViewController : UIViewController, IPdfDocumentDelegate
    {
        public PdfViewController(IntPtr handle) : base(handle)
        {
        }
        public ProjectInfo PDFInfo { get; set; }
        private bool PdfLocal { get; set; }
        private bool MetaLocal { get; set; }
        public NSUrl PdflocalNsUrl { get; set; }
        public NSUrl MetalocalNsUrl { get; set; }
        public ProjectInfo PdfInfo { get; }
        PdfView PDFView;
        nfloat w;
        nfloat h;

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        private void LoadPdf(NSUrl url)
        {
            PdfDocument document;
            document = new PdfDocument(url);
            if (document != null)
            {
                // Set our document to the view, center it, and set a background color
                PDFView.AutoScales = true;
                PDFView.Frame = new CGRect(10, 10, w - 20, h - 20);
                PDFView.BackgroundColor = UIColor.DarkGray;

                document.Delegate = this;
                PDFView.Document = document;
            }
            var adb1 = UIButton.FromType(UIButtonType.RoundedRect);
            adb1.Frame = new CGRect(60, 10, 50, 44);
            adb1.SetTitle("Submit", UIControlState.Normal);
            adb1.BackgroundColor = UIColor.White;
            adb1.Layer.CornerRadius = 5f;
            adb1.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            adb1.TouchUpInside += delegate
            {
                Console.WriteLine("1 button pressed");
                //var page = document.GetPage(1);
                //var bounds = page.GetBoundsForBox(PdfDisplayBox.Crop);

                PDFView.GoToPreviousPage(this);


            };

            var acdb2 = UIButton.FromType(UIButtonType.RoundedRect);
            acdb2.Frame = new CGRect(100, 10, 50, 44);
            acdb2.SetTitle("Submit", UIControlState.Normal);
            acdb2.BackgroundColor = UIColor.White;
            acdb2.Layer.CornerRadius = 5f;
            acdb2.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            acdb2.TouchUpInside += delegate
            {
                Console.WriteLine("1 button pressed");
                //var page = document.GetPage(1);
                //var bounds = page.GetBoundsForBox(PdfDisplayBox.Crop);

                PDFView.GoToNextPage(this);


            };

            var acdb3 = UIButton.FromType(UIButtonType.RoundedRect);
            acdb3.Frame = new CGRect(150, 10, 50, 44);
            acdb3.SetTitle("Submit", UIControlState.Normal);
            acdb3.BackgroundColor = UIColor.White;
            acdb3.Layer.CornerRadius = 5f;
            acdb3.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            acdb3.TouchUpInside += delegate
            {
                Console.WriteLine("1 button pressed");
                var watermarkPage = PDFView.CurrentPage as MarkedPdfPage;
                var pagePageNumber = watermarkPage.Page.PageNumber;
                watermarkPage?.DrawCircle();
                PDFView.SetNeedsDisplay();
                // And apply the transform.
            };

            View.AddSubviews(PDFView);
            PdfLocal = true;
        }

        StorageDownloadTask downloadTask;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitPdfView();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var pdfFilePath = Path.Combine(documents, $"{PDFInfo.Guid}.pdf");
            var metaFilePath = Path.Combine(documents, $"{PDFInfo.MetaId}.json");

            //file node from firebase
            var rootNode = Storage.DefaultInstance.GetRootReference();
            var pdfNode = rootNode.GetChild($"{PDFInfo.Guid.ToString()}.pdf");
            var metaNode = rootNode.GetChild($"{PDFInfo.MetaId.ToString()}.json");
            //local file url to store file in
            PdflocalNsUrl = NSUrl.FromString(pdfFilePath);
            MetalocalNsUrl = NSUrl.FromString(metaFilePath);
            if (!File.Exists(pdfFilePath))
            {
                downloadTask = pdfNode.WriteToFile(PdflocalNsUrl, (url, error) =>
                {
                    if (error != null)
                    {
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        LoadPdf(PdflocalNsUrl);
                    }

                });
            }
            else
            {
                LoadPdf(PdflocalNsUrl);
            }
            pdfNode.GetData(1 * 1024 * 1024, (data, error) => {
                if (error != null)
                {
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Data for "images/island.jpg" is returned
                    var i = data.Length;
                }
            });

            if (!File.Exists(metaFilePath))
            {
                StorageDownloadTask downloadTask = metaNode.WriteToFile(MetalocalNsUrl, (url, error) =>
                {
                    if (error != null)
                    {
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        Console.WriteLine($"Loading a meta info on pdf: {PDFInfo.Name}");
                        //load layer
                        MetaLocal = true;
                    }

                });
            }

        }

        private void InitPdfView()
        {
            View.BackgroundColor = UIColor.Gray;
            w = View.Bounds.Width;
            h = View.Bounds.Height;
            PDFView = new PdfView()
            {
                AutoScales = true,
                Frame = new CGRect(10, 10, w - 20, h - 20),
                BackgroundColor = UIColor.DarkGray,
                DisplayMode = PdfDisplayMode.SinglePage
            };
        }
        #region PdfDocumentDelegate
        [Export("classForPage")]
        public ObjCRuntime.Class GetClassForPage()
        {
            return new ObjCRuntime.Class(typeof(MarkedPdfPage));
        }
        #endregion
    }
}