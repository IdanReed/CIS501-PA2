using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIUpdateChainTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            

            
        }
        
        
    }

    public static class extendo
    {
        static List< Tuple<Control, Action<object>>> setActions  = new List<Tuple<Control, Action<object>>>();

        static List<Tuple<Control, List<Control>>> updateWeb = new List<Tuple<Control, List<Control>>>();

        public static void TriggerUpdate(this Control ele)
        {
            
        }

        public static void setControlAction(this Control ele, Action<object> updateMethod)
        {
            for(int index = 0; index < setActions.Count; index++)
            {

            }   
            foreach (Tuple<Control, Action<object>> tuple in setActions)
            {
                if (tuple.Item1.Equals(ele))
                {
                    Tuple<Control, Action<object>> newTuple = new Tuple<Control, Action<object>>(ele, updateMethod);
                    tuple = newTuple;
                }
            }
        }

    }
}

