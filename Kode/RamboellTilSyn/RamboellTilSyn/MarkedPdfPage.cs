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

                //var attributes = new UIStringAttributes()
                //{
                //    ForegroundColor = UIColor.FromRGBA(255, 0, 0, 125),
                //    Font = UIFont.BoldSystemFontOfSize(84)
                //};

                //var text = new NSAttributedString("TEST", attributes);
                //text.DrawString(new CGPoint(250, 40));
                RenderObjects(pageBounds, context);
                context.RestoreState();
                UIGraphics.PopContext();
            }
        }

        private void RenderObjects(CGRect rect, CGContext g)
        {

            var listOfData = MetaListJSonSingleton.TryGetInstance();
            if (listOfData?.PdfObjects != null)
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
}