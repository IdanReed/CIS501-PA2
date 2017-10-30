using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCEventSystem
{
    /// <summary>
    /// An event listener delegate with generic return type and parameter type
    /// </summary>
    /// <typeparam name="T">The parameter type implementing IEvent</typeparam>
    /// <typeparam name="U">The return type implementing IEventReturn</typeparam>
    /// <param name="e">The event of type T</param>
    /// <returns>The return type of type U</returns>
    public delegate U EventListener<T, U>(T e) where T : IEvent;
    /// <summary>
    /// An event listener delegate with generic return type but parameter type of IEvent. 
    /// </summary>
    /// <typeparam name="U">The return type implmenting IEventREturn</typeparam>
    /// <param name="e">The event of type IEvent</param>
    /// <returns>The return type of type U</returns>
    public delegate U EventListener<U>(IEvent e) where U :IEventReturn;

    /// <summary>
    /// The type used for all events
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// The type of event. ie: "updateEvent" or "changedDate"
        /// </summary>
        string Type
        {
            get;
        }
    }

    /// <summary>
    /// The return type used for all events. 
    /// </summary>
    public interface IEventReturn
    {
        /// <summary>
        /// The default for this return type. ie: Error.None or State.NoChange
        /// </summary>
        IEventReturn Default
        {
            get;
        }
    }
}
