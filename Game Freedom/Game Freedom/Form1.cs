using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Freedom
{
    public partial class Form1 : Form
    {

        //X to jest 1
        //Y to jest 2
        //taktyczny komentarz dla aktualizacji

        bool turn = true; //true = X turn, false = O turn
        int turn_count = 0;
        static int ROW = 10;
        static int COL = 10;


        char[,] polegry = new char[ROW, COL];
        //char[][] polegry = new char[ROW][COL];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            char[] TC;
            if (turn)
            {
                b.Text = "X";
                string letters = b.Name;
                TC = letters.ToCharArray();

            }
            else
            {
                b.Text = "O";
                string letters = b.Name;
                TC = letters.ToCharArray();

            }
            turn = !turn;
            b.Enabled = false;
            int i = TC[1] - '0';
            int j = TC[2] - '0';

            if (b.Text == "X")
            {
                if (b.Enabled == false)
                {
                    polegry[i, j] = '1';
                }
            }

            else
            {
                if (b.Enabled == false)
                {
                    polegry[i, j] = '2';
                }
            }

            Console.WriteLine("Punkty bialych:" + policzbiale(polegry));
            Console.WriteLine("Punkty czarnych:" + policzczarne(polegry));
            int XPoint = policzbiale(polegry);
            int YPoint = policzczarne(polegry);

            X_point_count.Text = XPoint.ToString();
            Y_point_count.Text = YPoint.ToString();




            //check_winning(i, j);

            //policzbiale(polegry);


            /*char[][] polegry1 = new char[10][];
            for (int z = 0; z < 10; z++)
            {
                polegry1[z] = new char[10];

            }
            for (int z = 0; z < 10; z++)
            {
                for (int k = 0; k < 10; k++)
                {
                    polegry1[z][k] = '0';

                }

            }
            polegry1[0] = "0011110000".ToCharArray();
            wypisz(polegry1);
            Console.WriteLine("Punkty bialych:" + policzbiale(polegry1));*/



        }

        private void check_winning(int i, int j) //atrapa
        {
            if (j < 8)
            {
                if ((polegry[i, j] & polegry[i, j + 1]) == '1')
                    MessageBox.Show("X wygrał", "zwyciestwo");
                if ((polegry[i, j] & polegry[i, j + 1]) == '2')
                    MessageBox.Show("O wygrał", "zwyciestwo");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Equipment:\n" +
                            "Board 10×10, 50 white and 50 bl ack pieces(stones) of the same value.\n" +

                            "The goal of the game:\n" +
                            "To have more pieces, than your opponent, that live in a rows of 4(in all possible directions), at the end of the game.\n" +

                            "Play:\n" +
                            "1.Players alternately place pieces in the fields.Game begins by placing a white figure in any field on table, a player who is next in turn(black) is obliged to play his move to one of the empty fields that are adjacent field that was played the previous move(fields adjacent orthogonal or diagonal).\n" +

                            "2.In a situation where a player can not play a move by the previous rule(all the adjacent fields are filled), he has the right(“freedom”) to play anywhere on the empty field on table, and the other player continues to play by the rule 1. (in the adjacent field).\n" +

                            "3.The game is over when all fields are filled.Once placed stones in the fields is not moving. The player who play last, has the right to choose to play or not to play(if it reduces his score).\n" +

                            "End of the game:\n" +
                            "At the end of the game should be counted live figures.Live figure is figure who are in a row of 4(orthogonal or diagonal).Arrays of larger or smaller length does not “give life” to figure. Score is expressed in the number of pieces that are live at the end of the game.The winner is the player with more points.If the numbers of points are equal, then it is the draw.\n" +

                            "Remark(example):\n" +
                            "In the event when a figure is in more rows: in one direction is in a row of length 4, and in other directions rows with different lengths, the figure is still alive.", "Instruction");
        }

        private void button_enter(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                if (turn)
                    b.Text = "X";
                else
                    b.Text = "O";
            }
        }

        private void button_leave(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.Enabled)
            {
                b.Text = "";
            }

        }

        private void wyswietlTabliceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Clear();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    Console.Write(polegry[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;


            X_point_count.Text = "0"; 
            Y_point_count.Text = "0";


                foreach (Control c in Controls)
                {
                    try
                    {
                        Button b = (Button)c;
                        b.Enabled = true;
                        b.Text = "";
                    }
                catch { }
                }

            
            
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    polegry[i, j] = '0';
                }

            }


            Console.Clear();
        }

        static int policzbiale(char[,] polegry)
        {

            int wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i,k];
                if (napis.Contains("0011111111") ||
                    napis.Contains("0111101111") ||
                    napis.Contains("1111001111") ||
                    napis.Contains("1111011110") ||
                    napis.Contains("1111111100") ||
                    napis.Contains("0111111110"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis.Contains("1111"))
                {
                    wynik++;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k,i];
                if (napis2.Contains("0011111111") ||
                    napis2.Contains("0111101111") ||
                    napis2.Contains("1111001111") ||
                    napis2.Contains("1111011110") ||
                    napis2.Contains("1111111100") ||
                    napis2.Contains("0111111110"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis2.Contains("1111"))
                {
                    wynik++;
                }

            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1,start_col + j];
                if (napis5.Contains("0011111111") ||
                    napis5.Contains("0111101111") ||
                    napis5.Contains("1111001111") ||
                    napis5.Contains("1111011110") ||
                    napis5.Contains("1111111100") ||
                    napis5.Contains("0111111110"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis5.Contains("1111"))
                {
                    wynik++;
                }
            }
            char[][] invpolegry = new char[10][];
            for (int i = 0; i < 10; i++)
            {
                invpolegry[i] = new char[10];
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    invpolegry[i][j] = polegry[i,9 - j];
                }

            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                if (napis6.Contains("0011111111") ||
                     napis6.Contains("0111101111") ||
                     napis6.Contains("1111001111") ||
                     napis6.Contains("1111011110") ||
                     napis6.Contains("1111111100") ||
                     napis6.Contains("0111111110"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis6.Contains("1111"))
                {
                    wynik++;
                }
            }

            return wynik;
        }

        static int policzczarne(char[,] polegry)
        {
            int wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i,k];
                if (napis.Contains("0022222222") ||
                   napis.Contains("0222202222") ||
                   napis.Contains("2222002222") ||
                   napis.Contains("2222022220") ||
                   napis.Contains("2222222200") ||
                   napis.Contains("0222222220"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis.Contains("2222"))
                {
                    wynik++;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k,i];
                if (napis2.Contains("0022222222") ||
                   napis2.Contains("0222202222") ||
                   napis2.Contains("2222002222") ||
                   napis2.Contains("2222022220") ||
                   napis2.Contains("2222222200") ||
                   napis2.Contains("0222222220"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis2.Contains("2222"))
                {
                    wynik++;
                }

            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1,start_col + j];
                if (napis5.Contains("0022222222") ||
                   napis5.Contains("0222202222") ||
                   napis5.Contains("2222002222") ||
                   napis5.Contains("2222022220") ||
                   napis5.Contains("2222222200") ||
                   napis5.Contains("0222222220"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis5.Contains("2222"))
                {
                    wynik++;
                }
            }
            char[][] invpolegry = new char[10][];
            for (int i = 0; i < 10; i++)
            {
                invpolegry[i] = new char[10];
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    invpolegry[i][j] = polegry[i,9 - j];
                }

            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                if (napis6.Contains("0022222222") ||
                   napis6.Contains("0222202222") ||
                   napis6.Contains("2222002222") ||
                   napis6.Contains("2222022220") ||
                   napis6.Contains("2222222200") ||
                   napis6.Contains("0222222220"))
                {
                    wynik++;
                    wynik++;
                }
                else if (napis6.Contains("2222"))
                {
                    wynik++;
                }
            }

            return wynik;
        }

        static void wypisz(char[][] polegry)
        {
            for (int i = 0; i < ROW; i++)
            {
                Console.WriteLine(polegry[i]);
            }
        }
    }
}
