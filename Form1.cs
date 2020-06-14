using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        private int visibleBills = 0;
        private int billSum = 0;
        private int idBill = 0;
        private int idBook = 0;
        private DataBase dataBase;
        private string query;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            dataGridView1.ReadOnly = true;
            dataGridView2.ReadOnly = true;

            dataBase = new DataBase();
            disconnectToolStripMenuItem.Enabled = false;

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox3.Items.Add("naziv");
            comboBox3.Items.Add("autor");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataBase.DisconnectFromDataBase();
            this.Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataBase.ConnectToDataBase();
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;

            query = "SELECT COUNT(knjiga.ID_knjiga) FROM knjiga";
            dataBase.runQuery(query, ref idBook);

            query = "SELECT COUNT(racun.ID_racun) FROM racun";
            dataBase.runQuery(query, ref idBill);

            query = "SELECT * FROM Knjiga";
            dataBase.runQuery(query, ref dataGridView1);

            query = "SELECT * FROM racun";
            dataBase.runQuery(query, ref dataGridView2);

            listCategory();
            listBooks();

            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataBase.DisconnectFromDataBase();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox4.Items.Clear();

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;

            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
        }

        private void listCategory()
        {
            dataBase.runQuery("SELECT naziv FROM kategorija", ref comboBox1);
            dataBase.runQuery("SELECT naziv FROM kategorija", ref comboBox4);
        }

        private void listBooks()
        {
            dataBase.runQuery("SELECT ID_knjiga FROM knjiga", ref comboBox2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                query = String.Format("SELECT knjiga.cena FROM knjiga WHERE knjiga.ID_knjiga = {0}", comboBox2.SelectedItem.ToString());
                dataBase.runQuery(query, ref billSum);
                label3.Text = "Trenutrno stanje: " + billSum;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (billSum > 0)
            {
                query = String.Format("INSERT INTO racun (ID_racun, cena, datum, vreme) VALUES({0}, {1}, Date(), '{2}')", ++idBill, billSum, DateTime.Now.ToString("HH:mm"));
                dataBase.runQuery(query, ref dataGridView2);
                MessageBox.Show("Uspesno uzeto");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                query = String.Format("SELECT * FROM (knjiga INNER JOIN kategorija ON knjiga.id_kategorija = kategorija.id_kategorije) WHERE kategorija.naziv = '{0}' ORDER BY knjiga.autor", comboBox1.SelectedItem.ToString());
                dataBase.runQuery(query, ref dataGridView1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                query = String.Format("SELECT * FROM knjiga ORDER BY knjiga.{0}", comboBox3.SelectedItem.ToString());
                dataBase.runQuery(query, ref dataGridView1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int category = 0;
            int price = 500;
            int discount = 0;

            int.TryParse(textBox3.Text, out price);
            int.TryParse(textBox4.Text, out discount);

            if (comboBox4.SelectedIndex > -1)
            {
                query = String.Format("SELECT Kategorija.ID_kategorije FROM Kategorija WHERE Kategorija.naziv = '{0}'", comboBox4.SelectedItem.ToString());
                dataBase.runQuery(query, ref category);
                query = String.Format("INSERT INTO knjiga (ID_knjiga, naziv, autor, cena, popust, id_kategorija) VALUES({0}, '{1}', '{2}', {3}, {4}, {5})", ++idBook, textBox1.Text, textBox2.Text, price, discount, category);
                dataBase.runQuery(query, ref dataGridView1);
                MessageBox.Show("Uspesno dodata knjiga");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (billSum > 0)
            {
                int temp = 0;
                query = String.Format("SELECT knjiga.cena FROM knjiga WHERE knjiga.ID_knjiga = {0}", comboBox2.SelectedItem.ToString());
                dataBase.runQuery(query, ref temp);
                billSum -= temp;
                label3.Text = "Trenutrno stanje: " + billSum;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dataGridView2.Visible = true;

                if (!String.IsNullOrEmpty(textBox5.Text))
                {
                    timer1.Start();
                }
            }
            else
            {
                timer1.Stop();
                visibleBills = 0;
                dataGridView2.Visible = true;
                label10.Text = "Vreme: " + 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            visibleBills++;
            label10.Text = "Vreme: " + visibleBills;

            int temp = 0;
            int.TryParse(textBox5.Text, out temp);

            if (temp < visibleBills)
            {
                timer1.Stop();
                dataGridView2.Visible = false;
                visibleBills = 0;
                label10.Text = "Vreme: " + 0;
            }
        }
    }
}
