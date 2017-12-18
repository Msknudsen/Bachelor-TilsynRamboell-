using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CoreAnimation;
using CoreGraphics;
using Firebase.Database;
using Firebase.Storage;
using Foundation;
using Newtonsoft.Json;
using PdfKit;
using UIKit;
//https://components.xamarin.com/gettingstarted/firebaseiosstorage#download-files
namespace Ramboell.iOS
{

    /// <summary>
    /// Showing the PDF and Allows user to mark pdf with objects.
    /// </summary>
    [System.ComponentModel.DesignTimeVisible(false)]
    public partial class PdfViewController : UIViewController, IPdfDocumentDelegate
    {
        public PdfViewController(IntPtr handle) : base(handle)
        {
            
        }
        /// <summary>
        /// This enum saves the state of the Shape selected for pdf 
        /// </summary>
        public Shape Shape;
        /// <summary>
        /// The objects for the pdf
        /// </summary>
        List<PdfObjectDto> pdfObjects;
        public RegistrationDto PDFInfo { get; set; }
        public NSUrl PdfLocalNsUrl { get; set; }
        public NSUrl MetalocalNsUrl { get; set; }
        MyPdfView PDFView;
        private nfloat h;
        private nfloat w;
        private UIButton _preSelectedbtn;
        StorageReference _jsonNode;

        const int panelHeight = 70;

        /// <summary>
        /// getting all the needed data before being able to show the view. This will check for the pdf and json file on the next if not exist
        /// </summary>
        public override void ViewDidLoad()
        {
            Shape = Shape.None;
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
            _jsonNode = rootNode.GetChild($"{PDFInfo.MetaId.ToString()}.json");
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
                    Logger.Log("PDFViewController_downloadEvent", PDFInfo.Guid.ToString(), "Succes");

                    if (error != null)
                    {
                        Logger.Log("PDFViewController_downloadError", PDFInfo.Guid.ToString(), error.LocalizedDescription);
                        throw new Exception(error.LocalizedDescription);
                    }

                    if (data.Save(PdfLocalNsUrl, NSDataWritingOptions.Atomic, out error))
                    {
                        Logger.Log("PDFViewController_fileEvent", PDFInfo.Guid.ToString(), "Pdf saved");

                        if (!File.Exists(jsonFilePath) && MetalocalNsUrl.IsFileUrl)
                        {
                            _jsonNode.GetData(10 * 1024 * 1024, (jsonData, JsonReaderExceptionrror) =>
                            {
                                if (error != null)
                                {
                                    Logger.Log("PDFViewController_fileEvent", PDFInfo.MetaId.ToString(), error.LocalizedDescription);
                                    throw new Exception(error.LocalizedDescription);
                                }
                                if (jsonData.Save(MetalocalNsUrl, NSDataWritingOptions.Atomic, out error))
                                {
                                    pdfObjects = MetaListJSonSingleton.GetInstance(MetalocalNsUrl.Path).PdfObjects;
                                    Logger.Log("PDFViewController.FileEvent", PDFInfo.MetaId.ToString(), "json file saved");
                                    add();
                                    Console.WriteLine("saved json as " + PDFInfo.MetaId);
                                    LoadPdf(PdfLocalNsUrl);
                                }
                                else
                                {
                                    Logger.Log("PDFViewController.DownloadError",PDFInfo.MetaId.ToString(),error.LocalizedDescription);
                                }

                            });
                        }
                    }
                    else
                    {
                        Console.WriteLine("NOT saved as " + PDFInfo.Guid + " because" + error.LocalizedDescription);
                        Logger.Log("PDFViewController.DownloadError", PDFInfo.Guid.ToString(), error.LocalizedDescription);
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
            else
            {
                pdfObjects = MetaListJSonSingleton.GetInstance(MetalocalNsUrl.Path).PdfObjects;
                add();

                LoadPdf(PdfLocalNsUrl);
            }

        }

        private void add()
        {
            pdfObjects.Add(new PdfObjectDto
            {
                XCord = 0,
                YCord = 50,
                PageNo = 1,
                Shape = Shape.Circle,
                TimeStamp = ""
            });
            pdfObjects.Add(new PdfObjectDto
            {
                XCord = (int)w - 50,
                YCord = 50,
                PageNo = 1,
                Shape = Shape.Circle,
                TimeStamp = ""
            });

            pdfObjects.Add(new PdfObjectDto
            {
                XCord = (int)w - 50,
                YCord = 50,
                PageNo = 1,
                Shape = Shape.Circle,
                TimeStamp = ""
            });
        }
        /// <summary>
        /// Detecting a Tap gesture when user tapping on screen, then saving it to lokal, upload it and reload the pdfview for new object 
        /// </summary>
        /// <param name="obj"></param>
        private void TapOnPdf(UITapGestureRecognizer obj)
        {
            var position = obj.LocationInView(obj.View);
            //check which shape we are working with
            if (Shape == Shape.None) return;
            if (PDFView.CurrentPage is MarkedPdfPage markPage)
            {
                var pagePageNumber = markPage.Page.PageNumber;

                var timeNow = new DateTime().ToUniversalTime().ToString("dd-MM-yyyy HH:mm:ss");
                //Add comment when time is right
                pdfObjects.Add(new PdfObjectDto
                {
                    XCord = (int)position.X,
                    YCord = (int)position.Y,
                    PageNo = (int)pagePageNumber,
                    Shape = Shape,
                    TimeStamp = timeNow
                });

                //Create a reference to the file you want to upload
                File.WriteAllText(MetalocalNsUrl.Path,JsonConvert.SerializeObject(pdfObjects));

                //Upload the file to the path "images/rivers.jpg"
                StorageUploadTask uploadTask = _jsonNode.PutFile(MetalocalNsUrl, null, (metadata, error) => {
                    if (error != null)
                    {
                        Console.WriteLine("Error");
                    }
                    else
                    {
                        DatabaseReference rootNode = Database.DefaultInstance.GetRootReference();
                        var key = nameof(RegistrationDto.Updated);
                        //update Firebase database 
                        rootNode
                            .GetChild(Global.Pdf)
                            .GetChild(PDFInfo.Guid.ToString())
                            .GetChild(key)
                            .SetValue(new NSString(timeNow));
                    }
                });
                Shape = Shape.None;
                _preSelectedbtn.Selected = false;
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
        /// <summary>
        /// adding PDFView and initi Width and Heigh for later in markedPDF class
        /// </summary>
        /// <param name="nwidth"></param>
        /// <param name="nfloat"></param>
        private void InitPdfView(nfloat nwidth, nfloat nfloat)
        {
            PDFView = new MyPdfView()
            {
                AutoScales = true,
                Frame = new CGRect(0, 0, nwidth, nfloat),
                BackgroundColor = UIColor.DarkGray,
                DisplayMode = PdfDisplayMode.SinglePage
            };
            Global.PdfViewWidth = PDFView.Bounds.Width;
            Global.PdfViewHeight = PDFView.Bounds.Height;
        }

        /// <summary>
        /// LoadPdf when all init is done, this will glue the other functions together and load a pdf with given urlpath together with the pdf panel. 
        /// </summary>
        /// <param name="url"> the url for the pdf</param>
        private void LoadPdf(NSUrl url)
        {
            if (!url.IsFileUrl) return;
            LoadPdfView(url);

            var loadPdfBottomPanel = LoadPdfBottomPanel();
            View.AddSubviews(loadPdfBottomPanel, PDFView);
            View.BringSubviewToFront(loadPdfBottomPanel);
        }

        /// <summary>
        /// Loads all the buttons from factory method into a UIView, and then set the logic and the placement for the buttons 
        /// </summary>
        /// <returns>returns all the buttons in one UIView </returns>
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
            prePageBtn.Frame = new CGRect(pLocal.X, pLocal.Y, pH, pH);
            prePageBtn.TouchUpInside += delegate
            {
                PDFView.GoToPreviousPage(this);
            };

            var nxtPageBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.NxtPage);
            nxtPageBtn.Frame = new CGRect(pLocal.X + 100, pLocal.Y, pH, pH);
            nxtPageBtn.TouchUpInside += delegate
            {
                PDFView.GoToNextPage(this);
            };
            var addCircleBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddCircle);
            addCircleBtn.Frame = new CGRect(pLocal.X + 200, pLocal.Y, pH, pH);
            addCircleBtn.TouchUpInside += delegate
            {
                Console.WriteLine("addCircleBtn pressed");
                SelectShape(iOS.Shape.Circle, addCircleBtn);
                
               
            };

            var addCheckMarkBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddCheckMark);
            addCheckMarkBtn.Frame = new CGRect(pLocal.X + 300, pLocal.Y, pH, pH);
            addCheckMarkBtn.TouchUpInside += delegate
            {
                SelectShape(iOS.Shape.CheckMark, addCheckMarkBtn);
            };
            var addMinusBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.AddMinus);
            addMinusBtn.Frame = new CGRect(pLocal.X + 400, pLocal.Y, pH, pH);
            addMinusBtn.TouchUpInside += delegate
            {
                SelectShape(iOS.Shape.Minus, addMinusBtn);
            };
            var showListBtn = PanelBtnFactory.GetButtonForType(PanelBtnFactory.BtnType.ShowList);
            showListBtn.Frame = new CGRect(pLocal.X + 500, pLocal.Y, pH, pH);
            showListBtn.TouchUpInside += delegate
            {
                Console.WriteLine("showListBtn pressed");
            };
            panelView.AddSubviews(prePageBtn,nxtPageBtn,addCircleBtn,addCheckMarkBtn,addMinusBtn,showListBtn);

            return panelView;
        }



        /// <summary>
        /// Containing the logic for animation of highlighting the UIButtons created, to show user if any shape is selected and remembers it
        /// </summary>
        /// <param name="shape">telling which shape is selected</param>
        /// <param name="btn">a reference of the btn</param>
        private void SelectShape(Shape shape, UIButton btn)
        {
            if (_preSelectedbtn != null)
            {
                //button pressed before
                Console.WriteLine("SelectShape: in if");
                if (Equals(_preSelectedbtn, btn))
                {
                    Console.WriteLine("preselected btn is equal");

                    _preSelectedbtn.Selected = !_preSelectedbtn.Selected;
                    Console.WriteLine($" new preselected btn is { _preSelectedbtn.Selected}");

                    if (_preSelectedbtn.Selected)
                        Shape = shape;
                    else
                        Shape = Shape.None;
                    Shape = _preSelectedbtn.Selected ? shape : Shape.None;
                    //same btn as before
                    return;
                }
                else
                {
                    //not same btn as before
                    Console.WriteLine($" SelectShape:end --> Shape: {Shape}, Selected:{_preSelectedbtn.Selected}");

                    _preSelectedbtn.Selected = false;
                    btn.Selected = true;
                    _preSelectedbtn = btn;
                    Shape = shape;

                }
            }
            else
            {

                Shape = shape;
                btn.Selected = true;
                _preSelectedbtn = btn;
                Console.WriteLine($" SelectShape:last end --> Shape: {Shape}, Selected:{_preSelectedbtn.Selected}");

            }


        }

        /// <summary>
        /// Resetting the MetaListJSonSingleton when view unloads. 
        /// </summary>
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            MetaListJSonSingleton.TryGetInstance().Reset();
        }

        /// <summary>
        /// laods the pdf document into the View
        /// </summary>
        /// <param name="url"></param>
        private void LoadPdfView(NSUrl url)
        {
            var document = new PdfDocument(url);

            if (document != null)
            {
                // Set our document to the view, center it, and set a background color
                document.Delegate = this;
                PDFView.Document = document;
                //var markedPdfPage = document.GetPage(1) as MarkedPdfPage;
                //markedPdfPage.InitPdfObjectFrom();
            }
        }
        /// <summary>
        /// The override for Objective-C to Return an instance of MarkedPdfPage instead of PdfPage
        /// </summary>
        /// <returns></returns>
        #region PdfDocumentDelegate
        [Export("classForPage")]
        public ObjCRuntime.Class GetClassForPage()
        {
            return new ObjCRuntime.Class(typeof(MarkedPdfPage));
        }
        #endregion
    }
}