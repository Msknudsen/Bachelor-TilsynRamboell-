using System;
using System.Collections.Generic;
using System.IO;
using CoreGraphics;
using Foundation;
using PdfKit;
using UIKit;

namespace Ramboell.iOS
{
    /// <summary>
    /// The override of PdfPage, this class would be called each time a pdf is shown
    /// </summary>
    [Register("MarkedPdfPage")]
    public class MarkedPdfPage : PdfPage
    {
        #region properties

        public string JsonData { get; set; }

        #endregion

        #region Constructors

        protected MarkedPdfPage(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #endregion
        /// <summary>
        /// Draw, rescale pdfobject position value, then load the page
        /// </summary>
        /// <param name="box"></param>
        /// <param name="context"></param>
        public override void Draw(PdfDisplayBox box, CoreGraphics.CGContext context)
        {
            // Draw original content
            base.Draw(box, context);
            using (context)
            {
                UIGraphics.PushContext(context);
                context.SaveState();
                var pageBounds = this.GetBoundsForBox(box);
                ScaledWidth = pageBounds.Width / Global.PdfViewWidth;
                ScaledHeight = pageBounds.Height / Global.PdfViewHeight;
                context.TranslateCTM(0.0f, pageBounds.Size.Height);
                context.ScaleCTM(1.0f, -1.0f);
                var s = Page.GetBoxRect(CGPDFBox.Media);

               
                //draw extra data
                RenderObjects(pageBounds, context);
                context.RestoreState();
                UIGraphics.PopContext();
            }
        }

        public nfloat ScaledHeight { get; set; }
        public nfloat ScaledWidth { get; set; }

        /// <summary>
        /// Rendering additional pdf objects on top of the pdf file
        /// </summary>
        private void RenderObjects(CGRect rect, CGContext g)
        {
            var listOfData = MetaListJSonSingleton.TryGetInstance();
            if (listOfData?.PdfObjects != null)
                using (g)
                {
                    for (var i = 0; i < listOfData.PdfObjects.Count; i++)
                    {
                        var item = listOfData.PdfObjects[i];
                        if (Page.PageNumber == item.PageNo)
                        {
                            var w = item.XCord * ScaledWidth;
                            var h = item.YCord * ScaledHeight;
                            var size = new CGRect( w - 25,  h - 25, 50, 50);
                            CGImage img = null;
                            switch (item.Shape)
                            {
                                case Shape.Circle:
                                    img = UIImage.FromBundle("circle").CGImage;
                                    break;
                                case Shape.CheckMark:

                                    img = UIImage.FromBundle("checkmarkpdf").CGImage;
                                    break;
                                case Shape.Minus:
                                    img = UIImage.FromBundle("minuspdf.png").CGImage;
                                    break;
                                default:
#if DEBUG
                                    Console.WriteLine("Using an invalid Shape ");
                                    break;
#else
                                throw new Exception("Using an invalid Shape ");
                            #endif
                            }
                            var attributes = new UIStringAttributes()
                            {
                                ForegroundColor = UIColor.FromRGBA(255, 0, 0, 125),
                                Font = UIFont.BoldSystemFontOfSize(12)
                                
                            };
                            var text1 = new NSAttributedString($"{i+1}", attributes);
                            text1.DrawString(new CGPoint(w+25, h-25));
                            //g.RotateCTM((float) (Math.PI / 12.0f));
                            g.DrawImage(size, img);
                        }
                    }
                }
        }
    }
}