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

        bool turn = true; //true = X turn, false = O turn
        int turn_count = 0;
        static int width = 10;
        static int height = 10;

        int[,] array = new int[width, height];

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
                    array[i, j] = 1;
                }
            }

            else
            {
                if (b.Enabled == false)
                {
                    array[i, j] = 2;
                }
            }

            check_winning(i, j);

        }

        private void check_winning(int i, int j) //atrapa
        {
            if (j < 8)
            {
                if ((array[i, j] & array[i, j + 1]) == 1)
                    MessageBox.Show("X wygrał", "zwyciestwo");
                if ((array[i, j] & array[i, j + 1]) == 2)
                    MessageBox.Show("O wygrał", "zwyciestwo");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Equipment:\n" +
                            "Board 10×10, 50 white and 50 black pieces(stones) of the same value.\n" +

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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;

            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = true;
                    b.Text = "";
                }

            }
            catch { }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    array[i, j] = 0;
                }

            }
            Console.Clear();
        }
    }
}
