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
        private string _message;
        private bool _errorOccured;
        public static Error None = new Error();

        public IEventReturn Default
        {
            get { return None; }
        }
        public string Message
        {
            get { return _message; }
        }

        public Error()
        {
            _errorOccured = false;
        }
        public Error(string m)
        {
            _errorOccured = true;
            _message = m;
        }
        public void Catch(Action<Error> e)
        {
            if (_errorOccured)
            {
                e(this);
            }
        }
    }
}
