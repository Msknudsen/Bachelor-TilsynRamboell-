using System;
using System.IO;
using System.Linq;
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
        public RegistrationDto PDFInfo { get; set; }
        private bool PdfLocal { get; set; }
        private bool MetaLocal { get; set; }
        public NSUrl PdfLocalNsUrl { get; set; }
        public NSUrl MetalocalNsUrl { get; set; }
        public RegistrationDto PdfInfo { get; }
        PdfView PDFView;
        private nfloat h;
        private nfloat w; 
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        private void LoadPdf(NSUrl url)
        {
            if (!url.IsFileUrl) return;
            var document = new PdfDocument(url);
            var doc = document.GetDataRepresentation();
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
            View.AddSubviews(PDFView, new UIView
            {
                Frame = new CGRect(w - 100, 0, w, 80),
                BackgroundColor = UIColor.LightGray,
            });
            PdfLocal = true;
        }
        private void LoadPdf(NSData url)
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

            View.AddSubviews(PDFView, new UIView
            {
                Frame = new CGRect(w-100,0,w,80),
                BackgroundColor = UIColor.LightGray,
            });

            PdfLocal = true;
        }

        StorageDownloadTask downloadTask;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitPdfView();

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //file node from firebase
            var rootNode = Storage.DefaultInstance.GetRootReference();
            var pdfNode = rootNode.GetChild($"{PDFInfo.Guid.ToString()}.pdf");
            var metaNode = rootNode.GetChild($"{PDFInfo.MetaId.ToString()}.json");
            //local file url to store file in

            var pdfName = $"{PDFInfo.Guid}.pdf";
            var pdfFilePath = "file://" + Path.Combine(documents, pdfName);
            PdfLocalNsUrl = NSUrl.FromString(pdfFilePath);

            var metaName = $"{PDFInfo.MetaId}.json";
            var jsonFilePath = "file://" + Path.Combine(documents, metaName);
            MetalocalNsUrl = NSUrl.FromString(jsonFilePath);

            if (!File.Exists(pdfFilePath) && PdfLocalNsUrl.IsFileUrl)
            {
                pdfNode.GetData(1 * 1024 * 1024, (data, error) =>
                {
                    var test = PdfLocalNsUrl.FilePathUrl;
                    var test2 = PdfLocalNsUrl.AbsoluteString;
                    var test3 = PdfLocalNsUrl.FileReferenceUrl;
                    var test4 = PdfLocalNsUrl.Scheme;
                    if (error != null)
                    {
                        Console.WriteLine();
                        throw new Exception(error.LocalizedDescription);
                    }

                  
                    //doesn't work
                    // saved to pdf, but is not identified as a file by iOS
                    if (data.Save(PdfLocalNsUrl, NSDataWritingOptions.Atomic, out error))
                    {
                        Console.WriteLine("saved as " + PDFInfo.Guid);
                    }
                    else
                    {
                        Console.WriteLine("NOT saved as " + PDFInfo.Guid + " because" + error.LocalizedDescription);
                    }
                    
                    //Environment.GetFolderPath(S

                    //File.WriteAllBytes(filePath, data.ToArray());

                    //using (var fileStream = File.Create(filePath, Convert.ToInt32(data.Length)))
                    //{
                    //    var dataBytes = new byte[data.Count()];
                    //    System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
                    //    fileStream.Write(dataBytes, 0, dataBytes.Length);
                    //}
                    //var rs = File.ReadAllBytes(filePath);
                    
                    //var test = Path.Combine(documents, "test.pdf");
                    ////File.WriteAllBytes(test, data.ToArray());
                    //var sds = data.Save(PdflocalNsUrl, true);

                    //Console.WriteLine("Saved {0}",sds);
                    ////NSUrl.CreateWithDataRepresentation(data, PdflocalNsUrl);


                    //LoadPdf(data);

                    LoadPdf(PdfLocalNsUrl);

                });
                    
                #region Xamarin bug

                //downloadTask = pdfNode.WriteToFile(PdflocalNsUrl, (url, error) =>
                //{
                //    if (error != null)
                //    {
                //        // Uh-oh, an error occurred!
                //    }
                //    else
                //    {
                //        LoadPdf(PdflocalNsUrl);
                //    }

                //});
                #endregion

            }
            else
            {
                Console.WriteLine("Loading local pdf");
                LoadPdf(PdfLocalNsUrl);
            }


            
            //TODO added for later
            //if (!File.Exists(MetalocalNsUrl.Path))
            //{
            //    StorageDownloadTask downloadTask = metaNode.WriteToFile(MetalocalNsUrl, (url, error) =>
            //    {
            //        if (error != null)
            //        {
            //            // Uh-oh, an error occurred!
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Loading a meta info on pdf: {PDFInfo.Name}");
            //            //load layer
            //            MetaLocal = true;
            //        }

            //    });
            //}

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