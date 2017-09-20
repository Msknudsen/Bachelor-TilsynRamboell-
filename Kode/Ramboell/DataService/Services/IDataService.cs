using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Providers;

namespace DataService.Services
{
    public interface IDataService
    {
        string PostMessages(DataObject obj);
        IObservableHandle ObserveMessage(Action<ObservationType, DataObject> callbackAction);
        void CancelObservation(IObservableHandle observableHandle); 
    }
}
