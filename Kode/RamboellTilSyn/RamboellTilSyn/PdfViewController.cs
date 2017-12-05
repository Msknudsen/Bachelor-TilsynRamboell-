using System;
using CoreGraphics;
using Foundation;
using PdfKit;
using UIKit;

namespace Ramboell.iOS
{
    public partial class PdfViewController : UIViewController, IPdfDocumentDelegate
    {
        public PdfViewController(IntPtr handle) : base(handle)
        {
        }

        public string PDFName { get; set; }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }
        PdfView PDFView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Gray;
            nfloat w = View.Bounds.Width;
            nfloat h = View.Bounds.Height;
            PDFView = new PdfView()
            {
                AutoScales = true,
                Frame = new CGRect(10, 10, w - 20, h - 20),
                BackgroundColor = UIColor.DarkGray,
                DisplayMode = PdfDisplayMode.SinglePage
            };
            var documentUrl = NSBundle.MainBundle.GetUrlForResource(PDFName, "pdf");
            var document = new PdfDocument(documentUrl);
            if (documentUrl != null)
            {
                // Set our document to the view, center it, and set a background color
                PDFView.AutoScales = true;
                PDFView.Frame = new CGRect(10, 10, w - 20, h - 20);
                PDFView.BackgroundColor = UIColor.DarkGray;

                document.Delegate = this;
                PDFView.Document = document;
            }
            View.AddSubviews(PDFView);
        }
    }
}