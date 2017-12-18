using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Contacts;

namespace Ramboell.iOS.Dto
{
    /// <summary>
    /// user dto mapping between firebase and C# obj
    /// </summary>
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
