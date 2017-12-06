using System;
using System.IO;
using CoreGraphics;
using Firebase.Storage;
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
        public ProjectInfo PDFInfo { get; set; }
        private bool PdfLocal { get; set; }
        private bool MetaLocal { get; set; }
        public NSUrl pdflocalNSUrl { get; set; }
        public NSUrl metalocalNSUrl { get; set; }


        public PdfViewController(IntPtr handle,ProjectInfo pdfInfo) : base(handle)
        {
            //filename to local
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var pdfFilePath = Path.Combine(documents, $"{pdfInfo.Guid}.pdf");
            var metaFilePath = Path.Combine(documents, $"{pdfInfo.MetaId}.json");
           
            //file node from firebase
            var rootNode = Storage.DefaultInstance.GetRootReference();
            var pdfNode = rootNode.GetChild($"{pdfInfo.Guid.ToString()}.pdf");
            var metaNode = rootNode.GetChild($"{pdfInfo.MetaId.ToString()}.json");

            pdflocalNSUrl = NSUrl.FromString(pdfFilePath);
            metalocalNSUrl = NSUrl.FromString(metaFilePath);

            if (!File.Exists(pdfFilePath))
            {
                StorageDownloadTask downloadTask = pdfNode.WriteToFile(pdflocalNSUrl, (url, error) =>
                {
                    if (error != null)
                    {
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        Console.WriteLine($"Loading a pdf: {pdfInfo.Name}");
                        PdfLocal = true;
                    }

                });
            }
            if (!File.Exists(pdfFilePath))
            {
                StorageDownloadTask downloadTask = metaNode.WriteToFile(metalocalNSUrl, (url, error) =>
                {
                    if (error != null)
                    {
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        Console.WriteLine($"Loading a meta info on pdf: {pdfInfo.Name}");

                        MetaLocal = true;
                    }

                });
            }
        }



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
            var documentUrl = NSBundle.MainBundle.GetUrlForResource(PDFInfo.Name, "pdf");
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