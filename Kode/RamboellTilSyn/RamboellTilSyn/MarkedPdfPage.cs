using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using PdfKit;
using UIKit;

namespace Ramboell.iOS
{
    [Register("MarkedPdfPage")]
    public class MarkedPdfPage : PdfPage
    {
        public string JsonData { get; set; }
        #region Constructors

        protected MarkedPdfPage(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            objects = new List<Pdfobject>
            {
                new Pdfobject()
                {
                    XCord = 250,
                    YCord = 200,
                    Shape = Shape.Circle,
                    PageNo = 2
                }
            };
        }
        #endregion
        List<Pdfobject> objects;

        public class Pdfobject
        {
            public int PageNo { get; set; }
            public int XCord { get; set; }
            public int YCord { get; set; }
            public Shape Shape { get; set; }
            public String Comment { get; set; }
            public Byte[] BlobBytes { get; set; }
            public int Size { get; set; }
            public DateTime TimeStamp { get; set; }
        }
        public override void Draw(PdfDisplayBox box, CoreGraphics.CGContext context)
        {
            // Draw original content
            base.Draw(box, context);
            var pdfDocument = this.Document;
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
                RenderObjects(context);
                context.RestoreState();
                UIGraphics.PopContext();
            }
        }

        public void DrawCircle()
        {
            objects.Add(new Pdfobject
            {
                PageNo = 1,
                Shape = Shape.Circle,
                Size = 50,
                XCord = 10,
                YCord = 10
            });
        }
        private void RenderObjects(CGContext g)
        {
            foreach (var item in objects)
            {

                if (Page.PageNumber == item.PageNo)
                {
                    var attributes = new UIStringAttributes()
                    {
                        ForegroundColor = UIColor.FromRGBA(255, 0, 0, 125),
                        Font = UIFont.BoldSystemFontOfSize(84)
                    };
                    var text1 = new NSAttributedString("✓", attributes);
                    text1.DrawString(new CGPoint(400, 300));

                    g.SetFillColor(UIColor.Cyan.CGColor);

                    //g.AddEllipseInRect(new CGRect(new CGPoint(item.XCord, item.YCord),CGSize.Empty));
                    g.AddEllipseInRect(new CGRect(
                        new CGPoint(
                            x: item.XCord,
                            y: item.YCord),
                        new CGSize(100, 100)
                        ));
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
    }

    public enum Shape
    {
        CheckMark,
        Cross,
        Circle
    }
}