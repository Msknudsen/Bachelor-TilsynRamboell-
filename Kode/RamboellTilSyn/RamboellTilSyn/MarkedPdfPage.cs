using System;
using System.Collections.Generic;
using System.IO;
using CoreGraphics;
using Foundation;
using PdfKit;
using UIKit;

namespace Ramboell.iOS
{
    [Register("MarkedPdfPage")]
    public class MarkedPdfPage : PdfPage
    {
        #region properties
        MetaListJSonSingleton listOfData;
        public string JsonData { get; set; }

        #endregion
        #region Constructors

        protected MarkedPdfPage(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

        }
        #endregion

        public override void Draw(PdfDisplayBox box, CoreGraphics.CGContext context)
        {
            // Draw original content
            base.Draw(box, context);
            using (context)
            {  
                // Draw watermark underlay
                UIGraphics.PushContext(context);
                context.SaveState();
                var pageBounds = this.GetBoundsForBox(box);
                context.TranslateCTM(0.0f, pageBounds.Size.Height);
                context.ScaleCTM(1.0f, -1.0f);
                //context.RotateCTM((float) (Math.PI / 4.0f));


                Console.WriteLine($"{pageBounds}");

                var attributes = new UIStringAttributes()
                {
                    ForegroundColor = UIColor.FromRGBA(255, 0, 0, 125),
                    Font = UIFont.BoldSystemFontOfSize(84)
                };

                var text = new NSAttributedString("TEST", attributes);
                text.DrawString(new CGPoint(250, 40));
                RenderObjects(pageBounds, context);
                context.RestoreState();
                UIGraphics.PopContext();
            }
        }

        private void RenderObjects(CGRect rect, CGContext g)
        {
            if (listOfData == null) return;
            using (g)
            {
                foreach (var item in listOfData.PdfObjects)
                {
                    
                    if (Page.PageNumber == item.PageNo)
                    {
                        var size = new CGRect(item.XCord, item.YCord, 50, 50);
                        CGImage img = null;
                        switch (item.Shape)
                        {
                            case Shape.Circle:
                                img = UIImage.FromBundle("back").CGImage;
                                break;
                            case Shape.CheckMark:

                                img = UIImage.FromBundle("checkmark").CGImage;
                                break;
                            default:
                            #if DEBUG
                                Console.WriteLine("Using an invalid Shape ");
                                break;
                            #else
                                throw new Exception("Using an invalid Shape ");
                            #endif
                        }
                        
                        g.DrawImage(size, img);
                        
                    }
                }
            }
            
        }

        public PdfDisplayBox Mybox { get; set; }

        public CGContext Myctx { get; set; }
        public void DrawPdfInMemory()
        {
            Console.WriteLine("DrawPdfInMemory");
            //data buffer to hold the PDF
            NSMutableData data = new NSMutableData();
            //create a PDF with empty rectangle, which will configure it for 8.5x11 inches
            UIGraphics.BeginPDFContext(data, CGRect.Empty, null);
            //start a PDF page
            UIGraphics.BeginPDFPage();
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                g.ScaleCTM(1, -1);
                g.TranslateCTM(0, -25);
                g.SelectFont("Helvetica", 25, CGTextEncoding.MacRoman);
                g.ShowText("Hello Core Graphics");
            }
            //complete a PDF page
            UIGraphics.EndPDFContent();
        }
        public void AddStuff(PdfDisplayBox box)
        {

            //using (CGContext g = UIGraphics.GetCurrentContext())
            //{
            //    g.SaveState();
            //    g.DrawPDFPage(this.Page);
            //    CGAffineTransform pdfTransform =
            //        Page.GetDrawingTransform(CGPDFBox.Crop, GetBoundsForBox(Mybox), 0, true);

            //    // And apply the transform.
            //    g.ConcatCTM(pdfTransform);

            //    g.RestoreState();
            //    // Finally, we draw the page and rest
            //}
        }
        public void AddStuff()
        {
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                g.SaveState();
                g.DrawPDFPage(this.Page);
                CGAffineTransform pdfTransform =
                    Page.GetDrawingTransform(CGPDFBox.Crop, GetBoundsForBox(Mybox), 0, true);

                // And apply the transform.
                g.ConcatCTM(pdfTransform);

                g.RestoreState();
                // Finally, we draw the page and rest
            }
        }


        public void DrawObjectFrom(string path)
        {
            listOfData = MetaListJSonSingleton.GetInstance(path);
        }
    }


    public enum Shape
    {
        CheckMark,
        Circle,
        Minus
    }
}