using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace NSAPI
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            //Dodanie zdarzenia informującego o wykonanej akcji z serwerem
            API.LogChanged += API_LogChanged;

            //Podpięcie źródła danych z enumeratora znajdującego się w bibliotece
            //Być może zmienimy to na zwykły string...
            cbMethods.DataSource = Enum.GetValues(typeof(API.Methods));
        }

        private void API_LogChanged(object sender, MessageEventArgs e)
        {
            //Wizualizacja danych o logach wygenerowanych przez obiekt API w polu tekstowym tbLog
            tbLog.Text += e.Msg.Body;
            tbLog.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Nowa kolekcja Nazwa/Wartość
            //Ma słuzyć jako parametry typu POST do obsługi danych
            NameValueCollection Parameters = new NameValueCollection();

            //Wszystkie elementy z grida, przenosimy do kolekcji Parameters
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.Cells["Name"].Value != null && row.Cells["Value"].Value != null)
                    Parameters.Add(row.Cells["Name"].Value.ToString(), row.Cells["Value"].Value.ToString());
            }

            //Wywołanie funkcji z API zwracającej obiekt dynamiczny
            //Do każdej funkcji może on wyglądać inaczej, zadziała to tylko na plus
            //Format przesyłu danych to JSON
            dynamic d = API.Query((API.Methods)cbMethods.SelectedItem, Parameters);

            //Dodatkowa właściwość, która w momencie wykonania funkcji Query zostaje uzupełniona tym co ona zwraca w postaci sformatowanego tekstu
            tbJsonResult.Text = API.RawResponse;
        }
    }
}
