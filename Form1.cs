using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        public List<Book> books;
        private string[] bookInfo;
        private int billSum = 0;
        private int idBill = 0;
        private int idBook = 0;
        private DataBase dataBase;
        private string query;
        private Form2 form2;

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
            label11.Text = "Status: Disconnected";

            dataBase = new DataBase();
            books = new List<Book>();
            form2 = new Form2();
            disconnectToolStripMenuItem.Enabled = false;

            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;

            zaposlenToolStripMenuItem.Enabled = false;

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
            if (klijentToolStripMenuItem.Checked)
            {
                groupBox1.Enabled = false;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
            }
            else
            {
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;
                groupBox5.Enabled = true;
                groupBox6.Enabled = true;
            }

            query = "SELECT COUNT(knjiga.ID_knjiga) FROM knjiga";
            dataBase.runQuery(query, ref idBook);

            query = "SELECT COUNT(racun.ID_racun) FROM racun";
            dataBase.runQuery(query, ref idBill);

            query = "SELECT * FROM Knjiga";
            dataBase.runQuery(query, ref dataGridView1);

            query = "SELECT * FROM racun";
            dataBase.runQuery(query, ref dataGridView2);

            dataBase.runQuery("SELECT naziv FROM kategorija", ref comboBox1);
            dataBase.runQuery("SELECT naziv FROM kategorija", ref comboBox4);
            dataBase.runQuery("SELECT ID_knjiga FROM knjiga", ref comboBox2);

            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
            label11.Text = "Status: Connected";
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
            groupBox6.Enabled = false;

            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
            label11.Text = "Status: Disconnected";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                query = String.Format("SELECT * FROM knjiga WHERE knjiga.ID_knjiga = {0}", comboBox2.SelectedItem.ToString());
                dataBase.runQuery(query, ref bookInfo);
                
                Book book = new Book(int.Parse(bookInfo[0]), bookInfo[1], bookInfo[2], int.Parse(bookInfo[3]), int.Parse(bookInfo[4]), int.Parse(bookInfo[5]));
                books.Add(book);

                int discount = (book.Price * book.Discount) / 100;
                billSum += (book.Price - discount);
                label3.Text = "Trenutrno stanje: " + billSum;
            }
            else
            {
                MessageBox.Show("Niste odabrali knjigu.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (billSum > 0)
            {
                query = String.Format("INSERT INTO racun (ID_racun, cena, datum, vreme) VALUES({0}, {1}, Date(), '{2}')", ++idBill, billSum, DateTime.Now.ToString("HH:mm"));
                dataGridView2.DataSource = null;
                dataBase.runQuery(query, ref dataGridView2);
                MessageBox.Show("Uspesno uzeto");
                Invalidate();
            }
            else
            {
                MessageBox.Show("Nemate knjigu u korpi.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                query = String.Format("SELECT * FROM (knjiga INNER JOIN kategorija ON knjiga.id_kategorija = kategorija.id_kategorije) WHERE kategorija.naziv = '{0}' ORDER BY knjiga.autor", comboBox1.SelectedItem.ToString());
                dataBase.runQuery(query, ref dataGridView1);
            }
            else
            {
                MessageBox.Show("Niste odabrali kategoriju.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                query = String.Format("SELECT * FROM knjiga ORDER BY knjiga.{0}", comboBox3.SelectedItem.ToString());
                dataBase.runQuery(query, ref dataGridView1);
            }
            else
            {
                MessageBox.Show("Niste odabrali naslov ili autora");
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
            else
            {
                MessageBox.Show("Niste popunili sva polja.");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            form2.getInfo(books, billSum);
            form2.ShowDialog();
            this.books = form2.ListBooks;
            this.billSum = form2.BillSum;
            label3.Text = "Trenutrno stanje: " + billSum;
        }

        private void zaposlenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataBase.Connected == true)
            {
                groupBox1.Enabled = true;
                groupBox5.Enabled = true;
                groupBox6.Enabled = true;
            }
            zaposlenToolStripMenuItem.Enabled = false;
            klijentToolStripMenuItem.Enabled = true;
        }

        private void klijentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataBase.Connected == true)
            {
                groupBox1.Enabled = false;
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
            }
            zaposlenToolStripMenuItem.Enabled = true;
            klijentToolStripMenuItem.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            query = "SELECT * FROM racun";
            dataBase.runQuery(query, ref dataGridView2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Regex regex = new Regex("^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)[0-9]{2}[.]$");
            if (!regex.IsMatch(textBox5.Text) || !regex.IsMatch(textBox6.Text))
            {
                MessageBox.Show("Podaci nisu uneti ili nisu validni");
            }
            else
            {
                query = String.Format("SELECT * FROM racun WHERE (CDATE(racun.datum) BETWEEN #{0}# AND #{1}#)", textBox5.Text, textBox6.Text);
                dataBase.runQuery(query, ref dataGridView2);
            }
        }
    }
}
