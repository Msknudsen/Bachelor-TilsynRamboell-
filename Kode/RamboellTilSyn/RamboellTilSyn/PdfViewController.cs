using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CoreGraphics;
using Firebase.Storage;
using Foundation;
using Newtonsoft.Json;
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
        public bool ShapeIsSelected { get; set; }
        public Shape? Shape;
        StorageDownloadTask downloadTask;
        public RegistrationDto PDFInfo { get; set; }
        private bool PdfLocal { get; set; }
        private bool MetaLocal { get; set; }
        public NSUrl PdfLocalNsUrl { get; set; }
        public NSUrl MetalocalNsUrl { get; set; }
        public RegistrationDto PdfInfo { get; }
        MyPdfView PDFView;
        private nfloat h;
        private nfloat w;
        UIButton preSelectedbtn;

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
            View.UserInteractionEnabled = true;
            PDFView.UserInteractionEnabled = true;

            UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(TapOnPdf);
            PDFView.AddGestureRecognizer(tapGesture);
            //file node from firebase

            var rootNode = Storage.DefaultInstance.GetRootReference();
            var pdfNode = rootNode.GetChild($"{PDFInfo.Guid.ToString()}.pdf");
            var jsonNode = rootNode.GetChild($"{PDFInfo.MetaId.ToString()}.json");
            //local file url to store file in

            var pdfName = $"{PDFInfo.Guid}.pdf";
            var pdfFilePath = "file://" + Path.Combine(documents, pdfName);
            PdfLocalNsUrl = NSUrl.FromString(pdfFilePath);

            var metaName = $"{PDFInfo.MetaId}.json";
            var jsonFilePath = "file://" + Path.Combine(documents, metaName);
            MetalocalNsUrl = NSUrl.FromString(jsonFilePath);

            if (!File.Exists(pdfFilePath) && PdfLocalNsUrl.IsFileUrl)
            {
                pdfNode.GetData(10 * 1024 * 1024, (data, error) =>
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
            if (!File.Exists(jsonFilePath) && MetalocalNsUrl.IsFileUrl)
            {
                jsonNode.GetData(10 * 1024 * 1024, (data, error) =>
                {
                    if (error != null)
                    {
                        Console.WriteLine();
                        throw new Exception(error.LocalizedDescription);
                    }
                    if (data.Save(MetalocalNsUrl, NSDataWritingOptions.Atomic, out error))
                    {
                        Console.WriteLine("saved as " + PDFInfo.MetaId);
                    }
                    else
                    {
                        Console.WriteLine("NOT saved as " + PDFInfo.Guid + " because" + error.LocalizedDescription);
                    }

                });
            }
            LoadPdf(PdfLocalNsUrl);
        }
        private void TapOnPdf(UITapGestureRecognizer obj)
        {
            var position = obj.LocationInView(obj.View);
            Console.WriteLine($"{position.X}, {position.Y}");
            //check which shape we are working with
            if (Shape != null)
            {
                
                if (PDFView.CurrentPage is MarkedPdfPage watermarkPage)
                {
                    var pagePageNumber = watermarkPage.Page.PageNumber;
                    //watermarkPage.DrawObject(Shape,pagePageNumber,position);
                    var jsonList = File.ReadAllText(MetalocalNsUrl.Path);
                    var pdfObjects = JsonConvert.DeserializeObject<List<PdfObject>>(jsonList);
                    //Add comment when time is right
                    pdfObjects.Add(new PdfObject
                    {
                        XCord = (int)position.X,
                        YCord = (int)position.Y,
                        PageNo = (int) pagePageNumber,
                        Shape = (Shape) Shape,
                        TimeStamp = new DateTime().ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss")
                    });
                    File.WriteAllText(MetalocalNsUrl.Path,JsonConvert.SerializeObject(pdfObjects));
                }
                PDFView.SetNeedsDisplay();
            }
        }

     
        public class MyPdfView: PdfView
        {
            public override void TouchesBegan(NSSet touches, UIEvent evt)
            {
                base.TouchesBegan(touches, evt);
                Console.WriteLine("Hello, I got touched");
            }
            
        }
        private void InitPdfView(nfloat nwidth, nfloat nfloat)
        {
            PDFView = new MyPdfView()
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

            var prePageBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.PrePage);
            prePageBtn.Frame = new CGRect(pLocal.X, pLocal.Y, 70, pH);
            prePageBtn.TouchUpInside += delegate
            {
                PDFView.GoToPreviousPage(this);
            };

            var nxtPageBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.NxtPage);
            nxtPageBtn.Frame = new CGRect(pLocal.X + 100, pLocal.Y, 70, pH);
            nxtPageBtn.TouchUpInside += delegate
            {
                PDFView.GoToNextPage(this);
            };
            var addCircleBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddCircle);
            addCircleBtn.Frame = new CGRect(pLocal.X + 200, pLocal.Y, 70, pH);
            addCircleBtn.TouchUpInside += delegate
            {
                Console.WriteLine("addCircleBtn pressed");
                SelectShape(iOS.Shape.Circle, addCircleBtn);
                
               
            };

            var addCheckMarkBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddCheckMark);
            addCheckMarkBtn.Frame = new CGRect(pLocal.X + 300, pLocal.Y, 70, pH);
            addCheckMarkBtn.TouchUpInside += delegate
            {
                SelectShape(iOS.Shape.Minus, addCheckMarkBtn);
            };
            var addMinusBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddMinus);
            addMinusBtn.Frame = new CGRect(pLocal.X + 400, pLocal.Y, 70, pH);
            addMinusBtn.TouchUpInside += delegate
            {
                SelectShape(iOS.Shape.Minus, addMinusBtn);
            };
            var showListBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.ShowList);
            showListBtn.Frame = new CGRect(pLocal.X + 500, pLocal.Y, 70, pH);
            showListBtn.TouchUpInside += delegate
            {
                Console.WriteLine("showListBtn pressed");
            };
            panelView.AddSubviews(prePageBtn,nxtPageBtn,addCircleBtn,addCheckMarkBtn,addMinusBtn,showListBtn);

            return panelView;
        }
        private void SelectShape(Shape shape, UIButton btn)
        {

            if (preSelectedbtn != null)
            {
                Shape = null;
                preSelectedbtn.Selected = false;
                if (Equals(preSelectedbtn, btn))
                    return;
            }
            Shape = shape;
            btn.Selected = true;
            preSelectedbtn = btn;
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