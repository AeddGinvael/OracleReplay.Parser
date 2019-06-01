using System;
using System.Collections.Generic;
using Oracle.Events;

namespace Oracle.Packets.Model
{
    public class ReplayContext
    {
        public DTClasses DtClasses { get; set; }
        public IObserver Observer { get; set; }
        public Build GameBuild { get; set; }

        public ReplayContext()
        {
            DtClasses = new DTClasses();
            Observer = new Observer(new Dictionary<Type, Action<Args<object>>>());
            GameBuild = new Build();
        }
    }
}