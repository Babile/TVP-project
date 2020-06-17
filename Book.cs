using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Book
    {
        private int ID;
        private string name;
        private string autor;
        private int price;
        private int discount;
        private int category;

        public Book(int iD, string name, string autor, int price, int discount, int category)
        {
            this.ID = iD;
            this.name = name;
            this.autor = autor;
            this.Price = price;
            this.Discount = discount;
            this.category = category;
        }

        public int Price { get => price; set => price = value; }
        public int Discount { get => discount; set => discount = value; }
        public int propID { get => ID; set => ID = value; }

        public override string ToString()
        {
            return "ID: " + this.ID + ", naziv: " + this.name + ", autor: " + this.autor + ", cena: " + this.price + ", popust: " + this.discount + ", kategorija: " + this.category;
        }
    }
}
