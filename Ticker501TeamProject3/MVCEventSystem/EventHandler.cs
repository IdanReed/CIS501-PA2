using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCEventSystem
{

    /// <summary>
    /// The EventHandler allows you to register listeners to specific events. 
    /// The event listeners can have a return type that implements IEventReturn and have a parameter
    /// that implements IEvent. Contains various methods for loading event listeners from attributes as well.
    /// </summary>
    /// <typeparam name="U">The return type for all the listeners. Must implement IEventReturn</typeparam>
    public class EventHandler<U> where U:IEventReturn, new()
    {
        /// <summary>
        /// A dictionary containing all the event handlers and the name for each of those handlers
        /// </summary>
        private Dictionary<string, EventListener<U>> _handlers;
        /// <summary>
        /// A dictionary containing all the event handlers that respond to a type of event.
        /// </summary>
        private Dictionary<Type, EventListener<U>> _typeHandlers;

        public EventHandler()
        {
            _handlers = new Dictionary<string, EventListener<U>>();
            _typeHandlers = new Dictionary<Type, EventListener<U>>();
            LoadAttributedListeners();
        }

        #region Attribute Handling

        /// <summary>
        /// Loads all the methods with EventListenerAttr attributes into their repective handler dictionary
        /// </summary>
        private void LoadAttributedListeners()
        {
            var methods = GetType()
                          .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                          .Where(m => m.GetCustomAttributes().OfType<EventListenerAttr>().Any())
                          .ToArray();

            foreach(var method in methods)
            {
                EventListenerAttr attr = method.GetCustomAttribute<EventListenerAttr>();
                var pars = method.GetParameters();
                if (pars[0] != null)
                {
                    Type typeArgument = pars[0].ParameterType;
                    if (attr.Name != null)
                    {
                        Action<MethodInfo, string> genMethod = CreateAttributedListener<IEvent>;
                        MethodInfo callMethod = GetType().GetMethod(
                            genMethod.Method.Name, 
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, 
                            null, 
                            new[] { typeof(MethodInfo), typeof(string) },
                            null
                        );
                        MethodInfo generic = callMethod.MakeGenericMethod(typeArgument);

                        generic.Invoke(this, new object[] { method, attr.Name });
                    }
                    else
                    {
                        Action<MethodInfo, Type> genMethod = CreateAttributedListener<IEvent>;
                        MethodInfo callMethod = GetType().GetMethod(
                            genMethod.Method.Name, 
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                            null, 
                            new[] { typeof(MethodInfo), typeof(Type) },
                            null
                        );
                        MethodInfo generic = callMethod.MakeGenericMethod(typeArgument);

                        generic.Invoke(this, new object[] { method, attr.Type });
                    }
                }
                
            }
            Debug.WriteLine(methods); 
        }
        /// <summary>
        /// Creates an event listener from an attribute of the specified EventType.
        /// </summary>
        /// <typeparam name="T">The type of event to take as a parameter</typeparam>
        /// <param name="method">The MethodInfo for the attributed method</param>
        /// <param name="eventName">The name of the event that will trigger this method</param>
        protected void CreateAttributedListener<T>(MethodInfo method, string eventName) where T: IEvent
        {
            Type genericListener = typeof(EventListener<,>);
            Type constructedListener = genericListener.MakeGenericType(typeof(T), typeof(U));

            var newListener = (EventListener<T, U> )Delegate.CreateDelegate(constructedListener, this, method);
            AddEventListener(eventName, newListener);
        }
        /// <summary>
        /// Creates an event listener from an attribute of the specified EventType that responds to all events of that Type
        /// </summary>
        /// <typeparam name="T">The type of event to take as a paramter</typeparam>
        /// <param name="method">The MethodInfo for the attributed method</param>
        /// <param name="eventType">The type of event that will trigger this method</param>
        protected void CreateAttributedListener<T>(MethodInfo method, Type eventType) where T: IEvent
        {
            Type genericListener = typeof(EventListener<,>);
            Type constructedListener = genericListener.MakeGenericType(typeof(T), typeof(U));

            var newListener = (EventListener<T, U> )Delegate.CreateDelegate(constructedListener, this, method);
            AddEventListener(eventType, newListener);
        }
        #endregion Attribute Handling


        /// <summary>
        /// Adds an event listener with IEvent as the parameter to the _handlers dictionary
        /// </summary>
        /// <param name="name">The name of the event that this new listener will respond to</param>
        /// <param name="e">The EventListener to run</param>
        public void AddEventListener(string name, EventListener<U> e)
        { 
            _handlers.Add(name,  e); 
        }
        /// <summary>
        /// Adds an event listener with the specified Event type as the parameter to the _handlers dictionary
        /// </summary>
        /// <typeparam name="T">The type of event used as the parameter</typeparam>
        /// <param name="name">The name of the event that this new listener will repond to</param>
        /// <param name="listener">The event listener to run</param>
        /// <seealso cref="Utils.WrapGenericEventListener{T, U}(EventListener{T, U})"/>
        public void AddEventListener<T>(string name, EventListener<T, U> listener) where T : IEvent
        {
            _handlers.Add(name, Utils.WrapGenericEventListener(listener));
        } 

        /// <summary>
        /// Adds an event listener that responds to all events of type T to the _typeHandlers dictionary
        /// </summary>
        /// <typeparam name="T">The type of event used as the parameter</typeparam>
        /// <param name="type">The type of event to respond to</param>
        /// <param name="listener">The event listener to run</param>
        /// <seealso cref="Utils.WrapGenericEventListener{T, U}(EventListener{T, U})"/>
        public void AddEventListener<T>(Type type, EventListener<T, U> listener) where T: IEvent
        {
            _typeHandlers.Add(type, Utils.WrapGenericEventListener(listener));
        }
        /// <summary>
        /// The event handle. This is used to give to anyone who wants to pass EventHandler an event.
        /// </summary>
        /// <typeparam name="T">The Event type used as the parameter</typeparam>
        /// <param name="e">The event of type T to pass along to the repective handlers</param>
        /// <returns>The result of type U from the handlers.</returns>
        public virtual U EventHandle<T>(T e) where T: IEvent
        {
            if (_handlers.ContainsKey(e.Type))
            {
                IEventReturn result = _handlers[e.Type](e);
                if (result != result.Default) return (U)result;
            }
            if (_typeHandlers.ContainsKey(e.GetType()))
            {
                return _typeHandlers[e.GetType()](e);
            }
            return (U) new U().Default;
        }

    }
}
