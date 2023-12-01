using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace atestat
{
    public partial class Form2 : Form
    {
        Button[,] b;
        int[,] a;
        int s, m; //minute si secunde timer
        int mines,minesgen,nomine,clear;    //mine explodate,mine generate, spatii fara mine total, spatii fara mine descoperite
        public Form2()
        {
            InitializeComponent();
            mines = 0; minesgen = 0; nomine = 0; clear = 0;
            s = 0; m = 0;
            int co;                                                  //contor care limiteaza nr de mine
            int q;

            label2.Text = "00:00";

            GroupBox groupBox1 = new GroupBox();                     //button table
            groupBox1.FlatStyle = FlatStyle.Flat;
            groupBox1.BackColor = Color.Transparent;

            a = new int[gamevar.n+2, gamevar.n+2];    

            b = new Button[gamevar.n, gamevar.n];

            //RANDOM GENERATION

            if (gamevar.n == 15)  //level 1 generation
            {
                label1.Text = "Level 1";
                q = 4;
                groupBox1.Location = new Point(447, 200);
                groupBox1.Size = new Size(375, 375);
            }
            else if (gamevar.n == 20) //level 2 generation
            {
                label1.Text = "Level 2";
                q = 4;
                groupBox1.Location = new Point(385, 125);
                groupBox1.Size = new Size(500, 500);
            }
            else if (gamevar.n == 25) //level 3 generation
            {
                label1.Text = "Level 3";
                q = 5;
                groupBox1.Location = new Point(322, 45);
                groupBox1.Size = new Size(625, 625);
                exit.Location = new Point(566, 700);
            }
            else q = 0; //error

            for (int i = 0; i < gamevar.n; i++)
                for (int j = 0; j < gamevar.n; j++)
                {
                    b[i, j] = new Button();

                    b[i, j].MouseDown += new MouseEventHandler(button_MouseDown);
                    b[i, j].Tag = i.ToString() + "," + j.ToString();

                    b[i, j].FlatStyle = FlatStyle.Flat;
                    b[i, j].Size = new Size(25, 25);
                    b[i, j].BackColor = Color.Gray;
                    b[i, j].Location = new Point(25 * j, 25 * i);
                    b[i, j].ForeColor = Color.DimGray;
                    groupBox1.Controls.Add(b[i, j]);
                }

            //END GENERATION

            //PREPARING MINE MAP...

            for (int i = 1; i <= gamevar.n; i++)                    //preparing mines
            {
                co = 0;
                for (int j = 1; j <= gamevar.n; j++)
                {
                    if (co <= gamevar.n / 3 - q)
                    {
                        a[i, j] = gamevar.r.Next(0, 2);
                        if (a[i, j] == 1) { co++; minesgen++; a[i, j] = 9; }
                    }
                    else
                    {
                        int co1 = 0;
                        for (int k = gamevar.n; k >= j + 1; k--)
                        {
                            if (co1 <= gamevar.n / 3 - q)
                            {   a[i, k] = gamevar.r.Next(0, 2);
                                if (a[i, k] == 1) { co1++; minesgen++; a[i, k] = 9; }
                            }
                            else
                                break;
                        }
                        break;
                    }
                }
            }

            Console.WriteLine(minesgen);
            nomine = gamevar.n * gamevar.n - minesgen;
            for (int i = 1; i <= gamevar.n; i++)                    //preparing fields around mines 
                for (int j = 1; j <= gamevar.n; j++)
                    if (a[i, j] == 0)
                    {
                        if (a[i + 1, j] == 9) a[i, j]++;
                        if (a[i + 1, j - 1] == 9) a[i, j]++;
                        if (a[i + 1, j + 1] == 9) a[i, j]++;
                        if (a[i - 1, j + 1] == 9) a[i, j]++;
                        if (a[i - 1, j] == 9) a[i, j]++;
                        if (a[i - 1, j - 1] == 9) a[i, j]++;
                        if (a[i, j - 1] == 9) a[i, j]++;
                        if (a[i, j + 1] == 9) a[i, j]++;
                    }
            Console.WriteLine(nomine);
            //END PREPARING

            for (int i = 1; i <= gamevar.n; i++)                  //print the random generated numerical map of the game
            {
                for (int j = 1; j <= gamevar.n; j++)
                    Console.Write(a[i, j] + " ");
                Console.WriteLine();
            }

            groupBox1.Anchor = AnchorStyles.None;
            Controls.Add(groupBox1);                              //adds the button table
            Start();
        }

        private void Start()
        {
            timer1.Start();
            for (int i = 0; i < gamevar.n; i++)
                for (int j = 0; j < gamevar.n; j++)
                    //buton<-clic, atunci daca e bomba(a[i+1,j+1]==9) face bum, altfel curata locatia
                    b[i, j].MouseClick += new MouseEventHandler(clic_buton);
        }

        private void mine_clear(int i,int j)
        {
            if (a[i + 1, j + 1] < 9)
            {
                clear++;      
                b[i, j].BackColor = Color.DarkGray;
                b[i, j].BackgroundImage = null;
                b[i, j].Enabled = false;
                
                switch (a[i + 1, j + 1])
                {
                    case 0:
                        {
                            a[i + 1, j + 1] = 10;
                            if (i + 1 == 1 && j + 1 == 1)
                            {
                                if (i < gamevar.n - 1) mine_clear(i + 1, j);
                                if (j < gamevar.n - 1) mine_clear(i, j + 1);
                                if (i < gamevar.n - 1 && j < gamevar.n - 1) mine_clear(i + 1, j + 1);
                            }
                            else if (i + 1 == 1)
                            {
                                if (j >= 1) mine_clear(i, j - 1);
                                if (i < gamevar.n - 1) mine_clear(i + 1, j);
                                if (j < gamevar.n - 1) mine_clear(i, j + 1);
                                if (i < gamevar.n - 1 && j < gamevar.n - 1) mine_clear(i + 1, j + 1);
                                if (i < gamevar.n - 1) mine_clear(i + 1, j - 1);
                            }
                            else if (j + 1 == 1)
                            {
                                if (i >= 1) mine_clear(i - 1, j);
                                if (i < gamevar.n - 1) mine_clear(i + 1, j);
                                if (j < gamevar.n - 1) mine_clear(i, j + 1);
                                if (i < gamevar.n - 1 && j < gamevar.n - 1) mine_clear(i + 1, j + 1);
                                if (j < gamevar.n - 1) mine_clear(i - 1, j + 1);
                            }
                            else if (i+1 >= 2 && j+1 >= 2)
                            {
                                mine_clear(i - 1, j);
                                mine_clear(i, j - 1);
                                if (i < gamevar.n - 1) mine_clear(i + 1, j);
                                if (j < gamevar.n - 1) mine_clear(i, j + 1);
                                if (i < gamevar.n - 1 && j < gamevar.n - 1) mine_clear(i + 1, j + 1);
                                mine_clear(i - 1, j - 1);
                                if (j < gamevar.n - 1) mine_clear(i - 1, j + 1);
                                if (i < gamevar.n - 1) mine_clear(i + 1, j - 1);
                            }
                            break;
                        }
                    case 1:
                        b[i, j].ForeColor = Color.Black;
                        b[i, j].Text = "1";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 2:
                        b[i, j].ForeColor = Color.Violet;
                        b[i, j].Text = "2";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 3:
                        b[i, j].ForeColor = Color.DarkBlue;
                        b[i, j].Text = "3";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 4:
                        b[i, j].ForeColor = Color.LightBlue;
                        b[i, j].Text = "4";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 5:
                        b[i, j].ForeColor = Color.Green;
                        b[i, j].Text = "5";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 6:
                        b[i, j].ForeColor = Color.Yellow;
                        b[i, j].Text = "6";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 7:
                        b[i, j].ForeColor = Color.DarkOrange;
                        b[i, j].Text = "7";
                        a[i + 1, j + 1] = 10;
                        break;
                    case 8:
                        b[i, j].ForeColor = Color.Red;
                        b[i, j].Text = "8";
                        a[i + 1, j + 1] = 10;
                        break;
                }
                b[i, j].Font = new Font("Ravie", 8, FontStyle.Bold);
                if (clear == nomine && mines<minesgen)
                {
                    timer1.Stop();
                    for (int z = 0; z < gamevar.n; z++)
                        for (int x = 0; x < gamevar.n; x++)
                            if (a[z + 1, x + 1] == 9)
                            {
                                b[z, x].Enabled = false;
                                b[z, x].BackColor = Color.DarkGray;
                                b[z, x].BackgroundImageLayout = ImageLayout.Stretch;
                                b[z, x].BackgroundImage = Properties.Resources.mine;
                            }
                    gamevar.time = label2.Text;
                    decimal mg = minesgen, mi = mines;
                    if (mi == 0) gamevar.score = Convert.ToString(100);
                    else {
                        decimal s = 100 - (mi / mg * 100);
                        NumberFormatInfo setPrecision = new NumberFormatInfo();
                        setPrecision.NumberDecimalDigits = 2;
                        gamevar.score = Convert.ToString(s.ToString("N", setPrecision));//you hit s% of the mines...not good yet
                    }
                    Form3 f = new Form3();
                    f.Show();
                }
            }
        }
        
        public void clic_buton(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < gamevar.n; i++)
                for (int j = 0; j < gamevar.n; j++)
                {
                    if (sender == b[i, j])
                    {
                        b[i, j].Enabled = false;
                        b[i, j].BackgroundImage = Properties.Resources.nothing;
                        b[i, j].BackColor = Color.DarkGray;
                        b[i, j].Font = new Font("Ravie", 8, FontStyle.Bold);
                        if (a[i + 1, j + 1] == 9)
                        {
                            b[i, j].BackgroundImageLayout = ImageLayout.Stretch;
                            b[i, j].BackgroundImage = Properties.Resources.mine;
                            b[i, j].Enabled = false;
                            mines++;
                            label4.Text = "Mines exploded: " + mines;
                            label4.Visible = true;
                            if (mines == minesgen)
                            {
                                for (int v = 0; v < gamevar.n; v++)
                                    for (int w = 0; w < gamevar.n; w++)
                                        b[v, w].Enabled = false;
                                timer1.Stop();
                                MessageBox.Show("You lost! :(");
                            }
                        }
                        else mine_clear(i, j);
                    }
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int v = 0; v < gamevar.n; v++)
                for (int w = 0; w < gamevar.n; w++)
                {
                    if (a[v + 1, w + 1] == 9 && b[v, w].Enabled == false)
                    {
                        a[v + 1, w + 1] += 10;
                        b[v, w].BackColor = Color.DarkGray;
                        b[v, w].BackgroundImageLayout = ImageLayout.Stretch;
                        b[v, w].BackgroundImage = Properties.Resources.boom;
                    }
                }
            if (s == 59) { s = 0; m++; }
            else s++;
            if (m == 0)
                if (s < 10) label2.Text = "00:" + "0" + s;
                else label2.Text = "00:" + s;
            else if (m < 10)
                if (s < 10) label2.Text = "0"+ m + ":" + "0" + s;
                else label2.Text = "0" + m + ":" + s;
            else
                if (s < 10) label2.Text =m + ":" + "0" + s;
                else label2.Text = m + ":" + s;
        }

        private void exit_Click(object sender, EventArgs e){ Close(); }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            // Verifica dacă butonul drept al mouse-ului a fost apăsat
            if (e.Button == MouseButtons.Right)
            {
                // Acceseaza butonul corespunzător din matrice folosind indicii i și j
                Button clickedButton = sender as Button;
                int i = Convert.ToInt32(clickedButton.Tag.ToString().Split(',')[0]);
                int j = Convert.ToInt32(clickedButton.Tag.ToString().Split(',')[1]);
                Button selectedButton = b[i, j];

                // Schimba fundalul butonului
                if (selectedButton.BackgroundImage == null || selectedButton.BackgroundImage == Properties.Resources.nothing)
                {
                    selectedButton.BackgroundImage = Properties.Resources.flag;
                    selectedButton.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    selectedButton.BackgroundImage = Properties.Resources.nothing;
                    selectedButton.BackgroundImage = null;
                }  
            }
        }
    }
}
//////////////////////////////
// form max res: 1940, 1050 //
//////////////////////////////