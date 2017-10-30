using MVCEventSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCEventSystem
{
    class Utils
    {
        /// <summary>
        /// A simple lambda wrapper that allows an event listener with any parameter type, T, to be added to the _handlers list which is only for
        /// Event listeners of type IEvent. This is required because of a variance problem I had with my delegates. The parameter types in a delegate are covariant
        /// meaning they can be cast to less specific therefore a lamda wrapper was required.
        /// </summary>
        /// <typeparam name="T">The type of the parameter for the event listener</typeparam>
        /// <typeparam name="U">The return type for the event listener being added</typeparam>
        /// <param name="e">The event listener to wrap</param>
        /// <returns>An event listener with parameter type of IEvent, which allows it to be added to the _handlers array</returns>
        public static EventListener<U> WrapGenericEventListener<T, U>(EventListener<T, U> e) where T : IEvent where U:IEventReturn, new()
        {
            return (IEvent evt) =>
            {
                if (evt is T)
                {
                    return e((T)evt);
                }
                else
                {
                    Debug.WriteLine("--WARNING--: Event was called with incorrect event type. {0} used but {1} needed", evt.GetType(), typeof(T));
                    return (U) new U().Default;
                }
            };
        }
    }
    /// <summary>
    /// A simple attribute class that can either have a name or a type
    /// </summary>
    public class EventListenerAttr : System.Attribute
    {
        private string _name;
        private Type _type;
        public string Name
        {
            get { return _name; }
        }
        public Type Type
        {
            get { return _type; }
        }

        public EventListenerAttr(string name)
        {
            _name = name;
        }
        public EventListenerAttr(Type type)
        {
            _type = type;
        }
    }
}
