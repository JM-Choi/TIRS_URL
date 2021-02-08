using System;

namespace Task.Factory
{
    internal class StarNew
    {
        private Func<object> p;

        public StarNew(Func<object> p)
        {
            this.p = p;
        }
    }
}