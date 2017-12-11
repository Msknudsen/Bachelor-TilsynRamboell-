﻿using System;
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
        StorageDownloadTask downloadTask;
        public RegistrationDto PDFInfo { get; set; }
        private bool PdfLocal { get; set; }
        private bool MetaLocal { get; set; }
        public NSUrl PdfLocalNsUrl { get; set; }
        public NSUrl MetalocalNsUrl { get; set; }
        public RegistrationDto PdfInfo { get; }
        PdfView PDFView;
        private nfloat h;
        private nfloat w;
        const int panelHeight = 70;
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.Gray;
            w = View.Bounds.Width;
            h = View.Bounds.Height;

            InitPdfView(w, h- panelHeight);

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
                    if (error != null)
                    {
                        Console.WriteLine();
                        throw new Exception(error.LocalizedDescription);
                    }

                    if (data.Save(PdfLocalNsUrl, NSDataWritingOptions.Atomic, out error))
                    {
                        Console.WriteLine("saved as " + PDFInfo.Guid);
                    }
                    else
                    {
                        Console.WriteLine("NOT saved as " + PDFInfo.Guid + " because" + error.LocalizedDescription);
                    }
                    
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

        private void InitPdfView(nfloat nwidth, nfloat nfloat)
        {
            PDFView = new PdfView()
            {
                AutoScales = true,
                Frame = new CGRect(0, 0, nwidth, nfloat),
                BackgroundColor = UIColor.DarkGray,
                DisplayMode = PdfDisplayMode.SinglePage
            };
        }
        private void LoadPdf(NSUrl url)
        {
            if (!url.IsFileUrl) return;
            LoadPdfView(url);

            PdfLocal = true;

            var loadPdfBottomPanel = LoadPdfBottomPanel();

            View.AddSubviews(loadPdfBottomPanel, PDFView);
            View.BringSubviewToFront(loadPdfBottomPanel);
        }

        private UIView LoadPdfBottomPanel()
        {
            var panelView = new UIView
            {
                Frame = new CGRect
                {
                    Location = new CGPoint(0, h - panelHeight),
                    Width = w,
                    Height = panelHeight,
                },
                BackgroundColor = UIColor.LightTextColor
            };
          
            var pW = panelView.Bounds.Width;
            var pH = panelView.Bounds.Height;
            var pLocal = panelView.Bounds.Location;

            var circleButton = UIButton.FromType(UIButtonType.RoundedRect);
            circleButton.Frame = new CGRect(pLocal.X,pLocal.Y, 70, pH);
            circleButton.SetTitle("Submit", UIControlState.Normal);
            circleButton.BackgroundColor = panelView.BackgroundColor;
            circleButton.Layer.CornerRadius = 5f;
            circleButton.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;

            circleButton.TouchUpInside += delegate
            {
                Console.WriteLine("1 button pressed");
                //var page = document.GetPage(1);
                //var bounds = page.GetBoundsForBox(PdfDisplayBox.Crop);

                PDFView.GoToPreviousPage(this);


            };
            var PageBtn = UIButton.FromType(UIButtonType.Plain);
            PageBtn.TouchUpInside += delegate
            {
                PDFView.GoToPreviousPage(this);
            };

            var prePageBtn = UIButton.FromType(UIButtonType.RoundedRect);
            prePageBtn.Frame = new CGRect(pLocal.X, pLocal.Y, 70, pH);
            prePageBtn.BackgroundColor = panelView.BackgroundColor;
            //prePageBtn.SetImage(UIImage.FromBundle("preBtn"), UIControlState.Normal);
            prePageBtn.TouchUpInside += delegate
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
            acdb3.Frame = new CGRect(w, h, 50, 44);
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
       
            panelView.AddSubview(prePageBtn,circleBtn, checkMarkBtn, minusBtn,nxtPageBtn,listPageBtn);

           
            return panelView;
        }

        private void LoadPdfView(NSUrl url)
        {
            var document = new PdfDocument(url);

            if (document != null)
            {
                // Set our document to the view, center it, and set a background color
                document.Delegate = this;
                PDFView.Document = document;
            }
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