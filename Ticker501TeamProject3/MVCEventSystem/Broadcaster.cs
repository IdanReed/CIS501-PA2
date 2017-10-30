using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCEventSystem
{
    /// <summary>
    /// An extension of the EventHandler class which provides extra broadcasting abilities.
    /// </summary>
    /// <typeparam name="U">The return type for all the EventListeners</typeparam>
    public class Broadcaster<U>: EventHandler<U> where U: IEventReturn, new()
    {
        /// <summary>
        /// A list of all the current observers. When an event is "broadcast" it sends it to each
        /// of the EventListeners in this registry
        /// </summary>
        List<EventListener<U>> _registry;
        /// <summary>
        /// Routings are a way to pass all events of a specific type directly from incoming to
        /// outgoing. 
        /// </summary>
        List<Type> _routings;
        public Broadcaster()
        {
            _routings = new List<Type>();
            _registry = new List<EventListener<U>>();
        }
        /// <summary>
        /// Registers an observer so when broadcast is called, the specified EventListener is called
        /// </summary>
        /// <param name="e">The event listener to call on broadcast</param>
        public void RegisterObserver(EventListener<U> e)
        {
            _registry.Add(e);
        }
        /// <summary>
        /// Registers an observer with the specified event type as a parameter
        /// </summary>
        /// <typeparam name="T">The type of event to use as a parameter</typeparam>
        /// <param name="e">The event listener to call on broadcast</param>
        /// <seealso cref="Utils.WrapGenericEventListener{T, U}(EventListener{T, U})"/>
        public void RegisterObserver<T>(EventListener<T, U> e) where T :IEvent
        {
            _registry.Add(Utils.WrapGenericEventListener(e));
        }
        /// <summary>
        /// Broadcasts an event to all listeners in the registry.
        /// </summary>
        /// <param name="e">The event to broadcast</param>
        public void Broadcast(IEvent e)
        {
            foreach(EventListener<U> listener in _registry)
            {
                listener(e);
            }
        }

        /// <summary>
        /// Adds a routing of the specific type
        /// </summary>
        /// <param name="t">The type of event to route straight to broadcast</param>
        public void AddRouting(Type t)
        {
            _routings.Add(t);
        }

        /// <summary>
        /// Extends the EventHandler.EventHandle by adding support for routings.
        /// If the type of event is going to be routed then it instantly broadcasts it.
        /// Otherwise it calls EventHandler.EventHandle.
        /// </summary>
        /// <typeparam name="T">The type of event to handle</typeparam>
        /// <param name="e">The Event of type T</param>
        /// <returns>The Event return of type U which is the result of all the handlers.</returns>
        public override U EventHandle<T>(T e)
        {
            if (_routings.Contains(e.GetType()))
            {
                Broadcast(e);
                return (U) new U().Default;
            }
            else
            {
                return base.EventHandle<T>(e);
            }
        }
    }
}
