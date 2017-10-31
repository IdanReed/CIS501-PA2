using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCEventSystem
{
    /// <summary>
    /// A simple error implmentation using IEventReturn
    /// </summary>
    public class Error: IEventReturn
    {
        /// <summary>
        /// The error message
        /// </summary>
        private string _message;
        
        /// <summary>
        /// A flag representing whether or not a flag occured
        /// </summary>
        private bool _errorOccured;
        
        /// <summary>
        /// The default value for the error
        /// </summary>
        public static Error None = new Error();

        /// <summary>
        /// The implementation of IEventReturn that returns Error.None
        /// </summary>
        public IEventReturn Default
        {
            get { return None; }
        }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Constructs an error where no error occured. Used by Error.None
        /// </summary>
        public Error()
        {
            _errorOccured = false;
        }

        /// <summary>
        /// Creates an error with a message and where an error occurred.
        /// </summary>
        /// <param name="m">The error message</param>
        public Error(string m)
        {
            _errorOccured = true;
            _message = m;
        }

        /// <summary>
        /// A simple Catch method which calls the action that was passed in if an error occured. If none occured then nothing happens
        /// </summary>
        /// <param name="e">The action to run if an error occured</param>
        public void Catch(Action<Error> e)
        {
            if (_errorOccured)
            {
                e(this);
            }
        }
    }
}
