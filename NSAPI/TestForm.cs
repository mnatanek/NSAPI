using System;
using System.Collections.Specialized;
using System.IO;
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

            //Przyda się do przechowywania konfiguracji, logów itp...
            Directory.CreateDirectory("C:\\NSAPI");

            LoadParams();
        }
        private void API_LogChanged(object sender, MessageEA e)
        {
            //Wizualizacja danych o logach wygenerowanych przez obiekt API w polu tekstowym tbLog
            tbLog.Text += e.Message + Environment.NewLine;
            tbLog.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveParams();

            //Nowa kolekcja Nazwa/Wartość
            //Ma słuzyć jako parametry typu POST do obsługi danych
            Params Parameters = new Params();

            //Wszystkie elementy z grida, przenosimy do kolekcji Parameters
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.Cells["Name"].Value != null && row.Cells["Value"].Value != null)
                    Parameters.Add(row.Cells["Name"].Value.ToString(), row.Cells["Value"].Value.ToString());
            }

            //Wywołanie funkcji z API zwracającej obiekt dynamiczny
            //Do każdej funkcji może on wyglądać inaczej, zadziała to tylko na plus
            //Format przesyłu danych to JSON
            dynamic d = API.Query(tbMethod.Text, Parameters);


            //Dodatkowa właściwość, która w momencie wykonania funkcji Query zostaje uzupełniona tym co ona zwraca w postaci sformatowanego tekstu
            tbJsonResult.Text = API.RawResponse;
        }

        private void LoadParams()
        {
            dgvParams.Rows.Clear();

            string file = "C:\\NSAPI\\params.dat";

            if (File.Exists(file))
            {
                using (BinaryReader bw = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    int n = bw.ReadInt32();
                    int m = bw.ReadInt32();
                    for (int i = 0; i < m; ++i)
                    {
                        dgvParams.Rows.Add();
                        for (int j = 0; j < n; ++j)
                        {
                            if (bw.ReadBoolean())
                            {
                                dgvParams.Rows[i].Cells[j].Value = bw.ReadString();
                            }
                            else bw.ReadBoolean();
                        }
                    }
                }
            }
        }

        private void SaveParams()
        {
            if (cbSaveParams.Checked == false)
                return;

            string file = "C:\\NSAPI\\params.dat";

            using (BinaryWriter bw = new BinaryWriter(File.Open(file, FileMode.Create)))
            {
                bw.Write(dgvParams.Columns.Count);
                bw.Write(dgvParams.Rows.Count);
                foreach (DataGridViewRow dgvR in dgvParams.Rows)
                {
                    for (int j = 0; j < dgvParams.Columns.Count; ++j)
                    {
                        object val = dgvR.Cells[j].Value;
                        if (val == null)
                        {
                            bw.Write(false);
                            bw.Write(false);
                        }
                        else
                        {
                            bw.Write(true);
                            bw.Write(val.ToString());
                        }
                    }
                }
            }
        }
    }
}
