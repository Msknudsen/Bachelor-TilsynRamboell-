using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Providers
{
    public class Providers
    {
    }

    public enum ObservationType
    {
        ChildAdded,
        ChildChanged,
        ChildRemoved
    }
    public interface IObservableHandle { }

    public interface IObservable : IObservableHandle
    {
        void CancelObservation();
    }

    public interface IDataProvider<T> : IObservable where T : DataObject, new()
    {
        string Create(T obj);
        void Delete(Guid id);
        Task<T> ReadAsync(Guid id);
        Task<IEnumerable<T>> ReadAllAsync();
        void Observe(Action<ObservationType, T> handler);
    }
}
