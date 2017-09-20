using DataService.Providers;

namespace DataService.Dto
{
    public class BaseProject : DataObject
    {
        public Location Location { get; set; }
        public string Name { get; set; }
        public Contractor Contractor { get; set; }
    }

    public class Contractor
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
    }
}