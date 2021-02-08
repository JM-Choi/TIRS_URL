using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace TechFloor.Object
{
    public class ConstructorNoFoundException : Exception
    {
        #region Fields
        private const string ConstructorNoFoundExceptionMessage = "Singleton<T> derived types require a non-public default constructor.";
        #endregion

        #region Constructors
        public ConstructorNoFoundException() : base(ConstructorNoFoundExceptionMessage) { }
        public ConstructorNoFoundException(string message) : base(string.Format($"{ConstructorNoFoundExceptionMessage}-{message}")) { }
        public ConstructorNoFoundException(string message, Exception inner) : base(string.Format($"{ConstructorNoFoundExceptionMessage}-{message}"), inner) { }
        #endregion
    }

    public abstract class Singleton<T> where T : class
    {
        #region Fields
        private static readonly Lazy<T> instance_ = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            if (!Array.Exists(ctors, (ci) => ci.GetParameters().Length == 0))
                throw new ConstructorNoFoundException("Non-public ctor() note found.");
            var ctor = Array.Find(ctors, (ci) => ci.GetParameters().Length == 0);
            return ctor.Invoke(new object[] { }) as T;
        }, LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Properties
        public static T Instance => instance_.Value;
        #endregion
    }
}
