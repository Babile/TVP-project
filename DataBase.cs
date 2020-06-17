using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Project
{
    public class DataBase
    {
        private OleDbConnection conn;
        private string DataBasePath;
        private bool connected = false;

        public DataBase()
        {
            DataBasePath = Application.StartupPath + "\\Knjizara.accdb";
        }

        public bool Connected { get => connected; set => connected = value; }

        public void ConnectToDataBase()
        {
            try
            {
                conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + DataBasePath);
                conn.Open();
                connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DisconnectFromDataBase()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn = null;
                    connected = false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void runQuery(string query, ref DataGridView dataGrid)
        {
            try
            {
                DataTable dataTable = new DataTable();

                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(query, conn);
                oleDbDataAdapter.Fill(dataTable);

                dataGrid.DataSource = dataTable.DefaultView;
                dataGrid.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void runQuery(string query, ref ComboBox comboBox)
        {
            try
            {
                DataTable dataTable = new DataTable();

                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(query, conn);
                oleDbDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        comboBox.Items.Add(dataTable.Rows[i][j].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void runQuery(string query, ref int number)
        {
            try
            {
                DataTable dataTable = new DataTable();

                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(query, conn);
                oleDbDataAdapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        int temp = 0;
                        int.TryParse(dataTable.Rows[i][j].ToString(), out temp);
                        number += temp;    
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void runQuery(string query, ref string[] stringArray)
        {
            try
            {
                
                DataTable dataTable = new DataTable();

                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(query, conn);
                oleDbDataAdapter.Fill(dataTable);
                stringArray = new string[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        stringArray[j] = dataTable.Rows[i][j].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
