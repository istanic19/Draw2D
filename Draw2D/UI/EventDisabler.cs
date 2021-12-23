using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw2D.UI
{
    public class EventDisabler
    {
        private int _stack;
        public bool Bypass { get; set; }

        public EventDisabler()
        {
            _stack = 0;
        }

        private void Add()
        {
            _stack++;
        }

        private void Remove()
        {
            if (_stack > 0)
                _stack--;
        }

        private bool Disabled()
        {
            return _stack > 0 && !Bypass;
        }

        public static EventDisabler operator ++(EventDisabler de)
        {
            de.Add();
            return de;
        }

        public static EventDisabler operator --(EventDisabler de)
        {
            de.Remove();
            return de;
        }

        public static bool operator true(EventDisabler de)
        {
            return de.Disabled();
        }

        public static bool operator false(EventDisabler de)
        {
            return !de.Disabled();
        }

        public static bool operator !(EventDisabler de)
        {
            return !de.Disabled();
        }

        public static implicit operator bool(EventDisabler de)
        {
            return de.Disabled();
        }
    }
}
