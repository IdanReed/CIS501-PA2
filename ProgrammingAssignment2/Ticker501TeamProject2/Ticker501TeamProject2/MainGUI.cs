using Class_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ticker501TeamProject2
{
    public partial class MainGUI : Form
    {
        private InputHandler _inputHandle;
        private Account _acct;
        private List<Ticker> _tickers;

        public MainGUI(Account a, List<Ticker> tickers, InputHandler inputHander)
        {
            _inputHandle = inputHander;
            _acct = a;
            _tickers = tickers;

            InitializeComponent();
        }

        public void Update(Event e)
        {

        }
    }
}
