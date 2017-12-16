using System;

namespace Ramboell.iOS
{
        public class PdfObjectDto
        {
            public int PageNo { get; set; }
            public int XCord { get; set; }
            public int YCord { get; set; }
            public Shape Shape { get; set; }
            public String Comment { get; set; }
            public int Size { get; set; }
            public string TimeStamp { get; set; }
        }
}