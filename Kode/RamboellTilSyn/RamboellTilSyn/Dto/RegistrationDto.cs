using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace Ramboell.iOS
{
    public class RegistrationDto
    {
        public string Name { get; set; }
        public string Updated { get; set; }
        public Guid MetaId { get; set; }
        public string Created { get; set; }
        public Guid Guid { get; set; }
    }
}