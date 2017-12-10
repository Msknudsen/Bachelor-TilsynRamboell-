using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Ramboell.iOS.Dto
{
    public class ProjectDto
    {
        public Guid Guid { get; set; }
        public string Address { get; set; }
        public string BuildOwner { get; set; }
        public string Customer { get; set; }
        public string Name { get; set; }
        public Guid Pdf { get; set; }
        public int ProjectNumber { get; set; }
    }
}