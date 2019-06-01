using System;
using System.Collections.Generic;

namespace Oracle.Events
{
    
    public interface IObserver
    {
        void Raise(Type t, Args<object> args);
    }
    public class Observer : IObserver
    {
        private Dictionary<Type, Action<Args<object>>> _observer;
        public Observer(Dictionary<Type, Action<Args<object>>> map)
        {
            _observer = map;
        }

        public void Raise(Type t, Args<object> args)
        {
            _observer.TryGetValue(t, out var action);

            if(action != null)
            {
                action.Invoke(args);
            }
        }
    }

}