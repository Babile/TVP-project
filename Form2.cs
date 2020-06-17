using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project
{
    public partial class Form2 : Form
    {
        private List<Book> listBooks;
        private int billSum;

        public List<Book> ListBooks { get => listBooks; set => listBooks = value; }
        public int BillSum { get => billSum; set => billSum = value; }

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Text = "Knjige u korpi";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.listBox1.HorizontalScrollbar = true;
           //this.ListBooks = new List<Book>();
        }

        public void getInfo(List<Book> books, int bill)
        {
            this.label1.Text = "Ukupno: " + bill;
            this.ListBooks = books;
            this.BillSum = bill;

            listBox1.Items.Clear();

            foreach (var i in books)
            {
                listBox1.Items.Add(i.ToString());
            }
            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                int idBook;
                string selectedIBook = listBox1.Items[this.listBox1.IndexFromPoint(e.Location)].ToString();
                string id = getBetween(selectedIBook, "ID: ", ",");
                idBook = int.Parse(id);

                listBox1.Items.RemoveAt(this.listBox1.IndexFromPoint(e.Location));

                for (int i = 0; i < this.ListBooks.Count; i++)
                {
                    if (this.ListBooks[i].propID == idBook)
                    {
                        int discount = (this.ListBooks[i].Price * this.ListBooks[i].Discount) / 100;
                        this.BillSum -= (this.ListBooks[i].Price - discount);
                        this.ListBooks.RemoveAt(i);
                        break;
                    }
                }
            }
            
            this.Close();
        }

        private string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            
            return "";
        }
    }
}
