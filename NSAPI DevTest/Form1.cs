using NSAPI;
using System;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace NSAPI_DEVTEST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            API.ShowTestForm();
        }
    }
}
