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
    public interface IEvent
    {
        string Type
        {
            get;
        }
    }
    public interface IEventReturn
    {
        IEventReturn Default
        {
            get;
        }
    }
}
