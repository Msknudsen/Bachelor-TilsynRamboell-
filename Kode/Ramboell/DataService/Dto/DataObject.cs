using System;

namespace DataService.Providers
{
    public class DataObject
    {
        public DataObject()
        {
            Value = new Guid();
        }

        public Guid Value { get; }
    }
}