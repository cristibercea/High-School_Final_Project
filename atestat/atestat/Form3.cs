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
    public partial class Form3 : Form
    {
        string s;
        public Form3(){
            InitializeComponent();
            if (gamevar.score == "100")
            {
                message.Location = new Point(110, 235);
                message.Text = "You're a legend! Your accuracy is: 100%";
            }   
            else
                message.Text = "Your accuracy is: " + gamevar.score + "%";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 40)
            {
                MessageBox.Show("Name too long! Please enter a shorter name.");
                textBox1.Text = "";
            }
            else
            {
                if (textBox1.Text == "" || Char.IsWhiteSpace(textBox1.Text, 0) || textBox1.Text.Length == 1)
                {
                    MessageBox.Show("Please enter a valid name!");
                    textBox1.Text = "";
                }
                else
                {
                    message.Visible = false;
                    int nr;
                    s = textBox1.Text;
                    textBox1.Visible = false;
                    button1.Visible = false;
                    label2.Text = "You have been successfully registered! \nRefresh the list in the Main Menu to see your stats!";
                    label2.Location = new Point(45, 175);

                    string[] st = new string[9];
                    StreamReader f = new StreamReader("Clasament.txt");
                    for (nr = 0; nr <= 8 && !f.EndOfStream; nr++) st[nr] = f.ReadLine();
                    f.Close();

                    StreamWriter g = new StreamWriter("Clasament.txt");
                    if (gamevar.n == 15) g.WriteLine("(Lv1) " + s + " - " + gamevar.time + " - " + gamevar.score + "% ");
                    else if (gamevar.n == 20) g.WriteLine("(Lv2) " + s + " - " + gamevar.time + " - " + gamevar.score + "% ");
                    else if (gamevar.n == 25) g.WriteLine("(Lv3) " + s + " - " + gamevar.time + " - " + gamevar.score + "% ");
                    for (nr = 0; nr <= 7; nr++)
                        if (st[nr + 1] != "")
                        {
                            if (st[nr] != "")
                                g.WriteLine(st[nr]);
                        }
                        else {
                            g.Write(st[nr]);
                            break;
                        }
                    if(nr==8 && st[nr] != "") g.Write(st[nr]);
                    g.Close();
                }
            }
        }

        private void exit_Click(object sender, EventArgs e){ Close(); }
    }
}
