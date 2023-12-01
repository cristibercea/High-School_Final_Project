using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace atestat
{
    public partial class Form1 : Form
    {
        string s;
        public Form1()
        {
            InitializeComponent();
            //lista cu ultimii 10 jucatori care au completat jocul
            StreamReader f = new StreamReader("Clasament.txt");
            s = f.ReadLine();
            if (s != null) label6.Text = s + "accurate";
            for (int nr = 1; nr <= 9 && !f.EndOfStream; nr++)
            {
                s = f.ReadLine();
                if (s != "") label6.Text += "\n" + s + "accurate";
                else break;
            }
            f.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gamevar.n = 15;
            Form2 f = new Form2();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gamevar.n = 20;
            Form2 f = new Form2();
            f.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gamevar.n = 25;
            Form2 f = new Form2();
            f.Show();
        }

        private void refresh_Click(object sender, EventArgs e) //butonul de testare al clasamentului
        {
            StreamReader f = new StreamReader("Clasament.txt");
            s = f.ReadLine();
            if (s != null) label6.Text = s + "accurate";
            for (int nr = 1; nr <= 9 && !f.EndOfStream; nr++)
            {
                s = f.ReadLine();
                if (s != "") label6.Text += "\n" + s + "accurate";
                else break;
            }
            f.Close();
        }

        private void exit_Click(object sender, EventArgs e) { Application.Exit(); }
    }
}
