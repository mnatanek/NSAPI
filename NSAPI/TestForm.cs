using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAPI
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            API.LogChanged += API_LogChanged;

            cbMethods.DataSource = Enum.GetValues(typeof(API.Methods));
        }

        private void API_LogChanged(object sender, MessageEventArgs e)
        {
            tbLog.Text += e.Msg.Body;
            tbLog.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NameValueCollection p = new NameValueCollection();

            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.Cells["Name"].Value != null && row.Cells["Value"].Value != null)
                    p.Add(row.Cells["Name"].Value.ToString(), row.Cells["Value"].Value.ToString());
            }

            dynamic d = API.Query((API.Methods)cbMethods.SelectedItem, p);

            tbJsonResult.Text = API.RawResponse;
        }
    }
}
