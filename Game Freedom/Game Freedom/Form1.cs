using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
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
    class Node
    {
        public char[][] polegry;
        public List<Node> children;
        public float positionValue;
        public Node(char[][] value)
        {
            polegry = value;
            children = new List<Node>();
            positionValue = 0;
        }
        public bool isLeaf(ref Node node)
        {
            return (children == null);
        }
        public void insertData(ref Node node, char[][] data)
        {
            if (node == null)
            {
                node = new Node(data);
            }
            else children.Add(new Node(data));
        }
        public bool search(Node node, char[][] s)
        {
            if (node == null) return false;
            if (node.polegry == s)
            {
                return true;
            }
            else
            {
                foreach (Node element in children)
                {
                    if (node.search(element, s)) return true;
                }
                return false;
            }
        }
        public Node FindChild(Node node, float s)
        {       
            
        foreach (Node element in node.children)
            {
                if (element.positionValue == s) return element;
            }
            return node;
        }
        public void display(Node n)
        {
            if (n == null) return;
            foreach (Node element in children)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        Console.Write(polegry[i][k]);
                    }
                    Console.Write("\n");
                }
            }
        }
    }
    class BinaryTree
    {
        public Node root;
        public int count;
        public BinaryTree()
        {
            root = null;
            count = 0;
        }
        public bool isEmpty()
        {
            return root == null;
        }
        public void insert(char[][] d)
        {
            if (isEmpty())
            {
                root = new Node(d);
            }
            else
            {
                root.insertData(ref root, d);
            }
            count++;
        }
        public bool search(char[][] s)
        {
            return root.search(root, s);
        }
        public bool isLeaf()
        {
            if (!isEmpty()) return root.isLeaf(ref root);
            return true;
        }
        public void display()
        {
            if (!isEmpty()) root.display(root);
        }
        public int Count()
        {
            return count;
        }
    }
    public partial class Form1 : Form
    {

        //X to jest 1
        //Y to jest 2
        //taktyczny komentarz dla aktualizacji

        bool turn = true; //true = X turn, false = O turn
        int turn_count = 0;
        static int ROW = 10;
        static int COL = 10;
        static int glebokosc = 1;

        //char[,] polegry = new char[ROW, COL];
        char[][] polegry = new char[ROW][];


        static List<char[][]> mozliweruchy(char[][] polegry, bool isMaximizingPlayer)
        {
            List<char[][]> ruchy = new List<char[][]>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (polegry[i][j] == '0')
                    {
                        char[][] nowyRuch = (char[][])GetCopy(polegry);
                        if (isMaximizingPlayer) nowyRuch[i][j] = '1';
                        else nowyRuch[i][j] = '2';
                        ruchy.Add(nowyRuch);
                    }
                }
            }
            return ruchy;
        }
        static BinaryTree drzewoGry(char[][] polegry)
        {
            BinaryTree drzewko = new BinaryTree();
            Node korzen = new Node(polegry);
            drzewko.root = korzen;
            splodzDzieci(korzen, glebokosc, true);

            return drzewko;
        }
        static float wnioskuj(Node node, bool isMaximizingPlayer)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            node.positionValue = alphabeta(node, glebokosc, float.MinValue, float.MaxValue, isMaximizingPlayer);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Czas alphabeta: " + elapsedMs);

            Console.WriteLine("Wartosc obecnej pozycji to: " + node.positionValue);

            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            node.positionValue = minimax(node, glebokosc, isMaximizingPlayer);
            watch2.Stop();
            var elapsedMs2 = watch2.ElapsedMilliseconds;
            Console.WriteLine("Czas minimax: " + elapsedMs2);

            Console.WriteLine("Wartosc obecnej pozycji to: " + node.positionValue);
            return node.positionValue;
        }
        static void splodzDzieci(Node node, int depth, bool isMaximizingPlayer)
        {
            if (depth == 0)
            {
                List<char[][]> ruchy = mozliweruchy(node.polegry, isMaximizingPlayer);
                foreach (char[][] ruch in ruchy)
                {
                    node.children.Add(new Node(ruch));
                }
            }
            else
            {
                List<char[][]> ruchy = mozliweruchy(node.polegry, isMaximizingPlayer);
                foreach (char[][] ruch in ruchy)
                {
                    node.children.Add(new Node(ruch));
                }
                foreach (Node i in node.children)
                {
                    splodzDzieci(i, depth - 1, !isMaximizingPlayer);
                }
            }
        }
        static float ocenWartoscBialych(char[][] polegry)
        {
            float wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i][k];
                while (napis.Contains("1111111111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111111111"), 10);
                }
                while (napis.Contains("111111111"))
                {
                    napis = napis.Remove(napis.IndexOf("111111111"), 9);
                }
                while (napis.Contains("11111111"))
                {
                    napis = napis.Remove(napis.IndexOf("11111111"), 8);
                }
                while (napis.Contains("1111111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111111"), 7);
                }
                while (napis.Contains("111111"))
                {
                    napis = napis.Remove(napis.IndexOf("111111"), 6);
                }
                while (napis.Contains("11111"))
                {
                    napis = napis.Remove(napis.IndexOf("11111"), 5);
                }
                while (napis.Contains("1111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111"), 4);
                    wynik++;
                }
                while (napis.Contains("01110"))
                {
                    napis = napis.Remove(napis.IndexOf("01110"), 5);
                    wynik += (float)0.75;
                }
                while (napis.Contains("0110"))
                {
                    napis = napis.Remove(napis.IndexOf("0110"), 4);
                    wynik += (float)0.25;
                }
                while (napis.Contains("01112"))
                {
                    napis = napis.Remove(napis.IndexOf("01112"), 5);
                    wynik += (float)0.5;
                }
                while (napis.Contains("21110"))
                {
                    napis = napis.Remove(napis.IndexOf("21110"), 5);
                    wynik += (float)0.5;
                }
                while (napis.Contains("111"))
                {
                    napis = napis.Remove(napis.IndexOf("111"), 3);
                    wynik += (float)0.5;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k][i];
                while (napis2.Contains("1111111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111111111"), 10);
                }
                while (napis2.Contains("111111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("111111111"), 9);
                }
                while (napis2.Contains("11111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("11111111"), 8);
                }
                while (napis2.Contains("1111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111111"), 7);
                }
                while (napis2.Contains("111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("111111"), 6);
                }
                while (napis2.Contains("11111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("11111"), 5);
                }
                while (napis2.Contains("1111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111"), 4);
                    wynik++;
                }
                while (napis2.Contains("01110"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("01110"), 5);
                    wynik += (float)0.75;
                }
                while (napis2.Contains("0110"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("0110"), 4);
                    wynik += (float)0.25;
                }
                while (napis2.Contains("01112"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("01112"), 5);
                    wynik += (float)0.5;
                }
                while (napis2.Contains("21110"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("21110"), 5);
                    wynik += (float)0.5;
                }
                while (napis2.Contains("111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("111"), 3);
                    wynik += (float)0.5;
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis5.Contains("1111111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111111111"), 10);
                }
                while (napis5.Contains("111111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("111111111"), 9);
                }
                while (napis5.Contains("11111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("11111111"), 8);
                }
                while (napis5.Contains("1111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111111"), 7);
                }
                while (napis5.Contains("111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("111111"), 6);
                }
                while (napis5.Contains("11111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("11111"), 5);
                }
                while (napis5.Contains("1111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111"), 4);
                    wynik++;
                }
                while (napis5.Contains("01110"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("01110"), 5);
                    wynik += (float)0.75;
                }
                while (napis5.Contains("0110"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("0110"), 4);
                    wynik += (float)0.25;
                }
                while (napis5.Contains("01112"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("01112"), 5);
                    wynik += (float)0.5;
                }
                while (napis5.Contains("21110"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("21110"), 5);
                    wynik += (float)0.5;
                }
                while (napis5.Contains("111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("111"), 3);
                    wynik += (float)0.5;
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
                    invpolegry[i][j] = polegry[i][9 - j];
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis6.Contains("1111111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111111111"), 10);
                }
                while (napis6.Contains("111111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("111111111"), 9);
                }
                while (napis6.Contains("11111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("11111111"), 8);
                }
                while (napis6.Contains("1111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111111"), 7);
                }
                while (napis6.Contains("111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("111111"), 6);
                }
                while (napis6.Contains("11111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("11111"), 5);
                }
                while (napis6.Contains("1111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111"), 4);
                    wynik++;
                }
                while (napis6.Contains("01110"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("01110"), 5);
                    wynik += (float)0.75;
                }
                while (napis6.Contains("0110"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("0110"), 4);
                    wynik += (float)0.25;
                }
                while (napis6.Contains("01112"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("01112"), 5);
                    wynik += (float)0.5;
                }
                while (napis6.Contains("21110"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("21110"), 5);
                    wynik += (float)0.5;
                }
                while (napis6.Contains("111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("111"), 3);
                    wynik += (float)0.5;
                }
            }
            return wynik;
        }
        static float ocenWartoscCzarnych(char[][] polegry)
        {
            float wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i][k];
                while (napis.Contains("2222222222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222222222"), 10);
                }
                while (napis.Contains("222222222"))
                {
                    napis = napis.Remove(napis.IndexOf("222222222"), 9);
                }
                while (napis.Contains("22222222"))
                {
                    napis = napis.Remove(napis.IndexOf("22222222"), 8);
                }
                while (napis.Contains("2222222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222222"), 7);
                }
                while (napis.Contains("222222"))
                {
                    napis = napis.Remove(napis.IndexOf("222222"), 6);
                }
                while (napis.Contains("22222"))
                {
                    napis = napis.Remove(napis.IndexOf("22222"), 5);
                }
                while (napis.Contains("2222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222"), 4);
                    wynik++;
                }
                while (napis.Contains("02220"))
                {
                    napis = napis.Remove(napis.IndexOf("02220"), 5);
                    wynik += (float)0.75;
                }
                while (napis.Contains("0220"))
                {
                    napis = napis.Remove(napis.IndexOf("0220"), 4);
                    wynik += (float)0.25;
                }
                while (napis.Contains("02221"))
                {
                    napis = napis.Remove(napis.IndexOf("02221"), 5);
                    wynik += (float)0.5;
                }
                while (napis.Contains("12220"))
                {
                    napis = napis.Remove(napis.IndexOf("12220"), 5);
                    wynik += (float)0.5;
                }
                while (napis.Contains("222"))
                {
                    napis = napis.Remove(napis.IndexOf("222"), 3);
                    wynik += (float)0.5;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k][i];
                while (napis2.Contains("2222222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222222222"), 10);
                }
                while (napis2.Contains("222222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("222222222"), 9);
                }
                while (napis2.Contains("22222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("22222222"), 8);
                }
                while (napis2.Contains("2222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222222"), 7);
                }
                while (napis2.Contains("222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("222222"), 6);
                }
                while (napis2.Contains("22222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("22222"), 5);
                }
                while (napis2.Contains("2222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222"), 4);
                    wynik++;
                }
                while (napis2.Contains("02220"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("02220"), 5);
                    wynik += (float)0.75;
                }
                while (napis2.Contains("0220"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("0220"), 4);
                    wynik += (float)0.25;
                }
                while (napis2.Contains("02221"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("02221"), 5);
                    wynik += (float)0.5;
                }
                while (napis2.Contains("12220"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("12220"), 5);
                    wynik += (float)0.5;
                }
                while (napis2.Contains("222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("222"), 3);
                    wynik += (float)0.5;
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis5.Contains("2222222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222222222"), 10);
                }
                while (napis5.Contains("222222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("222222222"), 9);
                }
                while (napis5.Contains("22222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("22222222"), 8);
                }
                while (napis5.Contains("2222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222222"), 7);
                }
                while (napis5.Contains("222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("222222"), 6);
                }
                while (napis5.Contains("22222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("22222"), 5);
                }
                while (napis5.Contains("2222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222"), 4);
                    wynik++;
                }
                while (napis5.Contains("02220"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("02220"), 5);
                    wynik += (float)0.75;
                }
                while (napis5.Contains("0220"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("0220"), 4);
                    wynik += (float)0.25;
                }
                while (napis5.Contains("02221"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("02221"), 5);
                    wynik += (float)0.5;
                }
                while (napis5.Contains("12220"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("12220"), 5);
                    wynik += (float)0.5;
                }
                while (napis5.Contains("222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("222"), 3);
                    wynik += (float)0.5;
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
                    invpolegry[i][j] = polegry[i][9 - j];
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis6.Contains("2222222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222222222"), 10);
                }
                while (napis6.Contains("222222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("222222222"), 9);
                }
                while (napis6.Contains("22222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("22222222"), 8);
                }
                while (napis6.Contains("2222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222222"), 7);
                }
                while (napis6.Contains("222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("222222"), 6);
                }
                while (napis6.Contains("22222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("22222"), 5);
                }
                while (napis6.Contains("2222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222"), 4);
                    wynik++;
                }
                while (napis6.Contains("02220"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("02220"), 5);
                    wynik += (float)0.75;
                }
                while (napis6.Contains("0220"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("0220"), 4);
                    wynik += (float)0.25;
                }
                while (napis6.Contains("02221"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("02221"), 5);
                    wynik += (float)0.5;
                }
                while (napis6.Contains("12220"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("12220"), 5);
                    wynik += (float)0.5;
                }
                while (napis6.Contains("222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("222"), 3);
                    wynik += (float)0.5;
                }
            }
            return wynik;
        }
        static int policzBiale(char[][] polegry)
        {
            int wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i][k];
                while (napis.Contains("1111111111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111111111"), 10);
                }
                while (napis.Contains("111111111"))
                {
                    napis = napis.Remove(napis.IndexOf("111111111"), 9);
                }
                while (napis.Contains("11111111"))
                {
                    napis = napis.Remove(napis.IndexOf("11111111"), 8);
                }
                while (napis.Contains("1111111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111111"), 7);
                }
                while (napis.Contains("111111"))
                {
                    napis = napis.Remove(napis.IndexOf("111111"), 6);
                }
                while (napis.Contains("11111"))
                {
                    napis = napis.Remove(napis.IndexOf("11111"), 5);
                }
                while (napis.Contains("1111"))
                {
                    napis = napis.Remove(napis.IndexOf("1111"), 4);
                    wynik++;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k][i];
                while (napis2.Contains("1111111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111111111"), 10);
                }
                while (napis2.Contains("111111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("111111111"), 9);
                }
                while (napis2.Contains("11111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("11111111"), 8);
                }
                while (napis2.Contains("1111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111111"), 7);
                }
                while (napis2.Contains("111111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("111111"), 6);
                }
                while (napis2.Contains("11111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("11111"), 5);
                }
                while (napis2.Contains("1111"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("1111"), 4);
                    wynik++;
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis5.Contains("1111111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111111111"), 10);
                }
                while (napis5.Contains("111111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("111111111"), 9);
                }
                while (napis5.Contains("11111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("11111111"), 8);
                }
                while (napis5.Contains("1111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111111"), 7);
                }
                while (napis5.Contains("111111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("111111"), 6);
                }
                while (napis5.Contains("11111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("11111"), 5);
                }
                while (napis5.Contains("1111"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("1111"), 4);
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
                    invpolegry[i][j] = polegry[i][9 - j];
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis6.Contains("1111111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111111111"), 10);
                }
                while (napis6.Contains("111111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("111111111"), 9);
                }
                while (napis6.Contains("11111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("11111111"), 8);
                }
                while (napis6.Contains("1111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111111"), 7);
                }
                while (napis6.Contains("111111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("111111"), 6);
                }
                while (napis6.Contains("11111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("11111"), 5);
                }
                while (napis6.Contains("1111"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("1111"), 4);
                    wynik++;
                }
            }
            return wynik;
        }
        static int policzCzarne(char[][] polegry)
        {
            int wynik = 0;
            for (int i = 0; i < 10; i++)
            {
                String napis = String.Empty;
                for (int k = 0; k < 10; k++) napis += polegry[i][k];
                while (napis.Contains("2222222222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222222222"), 10);
                }
                while (napis.Contains("222222222"))
                {
                    napis = napis.Remove(napis.IndexOf("222222222"), 9);
                }
                while (napis.Contains("22222222"))
                {
                    napis = napis.Remove(napis.IndexOf("22222222"), 8);
                }
                while (napis.Contains("2222222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222222"), 7);
                }
                while (napis.Contains("222222"))
                {
                    napis = napis.Remove(napis.IndexOf("222222"), 6);
                }
                while (napis.Contains("22222"))
                {
                    napis = napis.Remove(napis.IndexOf("22222"), 5);
                }
                while (napis.Contains("2222"))
                {
                    napis = napis.Remove(napis.IndexOf("2222"), 4);
                    wynik++;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                String napis2 = String.Empty;
                for (int k = 0; k < 10; k++) napis2 += polegry[k][i];
                while (napis2.Contains("2222222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222222222"), 10);
                }
                while (napis2.Contains("222222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("222222222"), 9);
                }
                while (napis2.Contains("22222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("22222222"), 8);
                }
                while (napis2.Contains("2222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222222"), 7);
                }
                while (napis2.Contains("222222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("222222"), 6);
                }
                while (napis2.Contains("22222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("22222"), 5);
                }
                while (napis2.Contains("2222"))
                {
                    napis2 = napis2.Remove(napis2.IndexOf("2222"), 4);
                    wynik++;
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis5 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis5 += polegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis5.Contains("2222222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222222222"), 10);
                }
                while (napis5.Contains("222222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("222222222"), 9);
                }
                while (napis5.Contains("22222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("22222222"), 8);
                }
                while (napis5.Contains("2222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222222"), 7);
                }
                while (napis5.Contains("222222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("222222"), 6);
                }
                while (napis5.Contains("22222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("22222"), 5);
                }
                while (napis5.Contains("2222"))
                {
                    napis5 = napis5.Remove(napis5.IndexOf("2222"), 4);
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
                    invpolegry[i][j] = polegry[i][9 - j];
                }
            }
            for (int line = 1; line <= (ROW + COL - 1); line++)
            {
                String napis6 = String.Empty;
                int start_col = Math.Max(0, line - ROW);
                int count = Math.Min(line, Math.Min((COL - start_col), ROW));
                for (int j = 0; j < count; j++) napis6 += invpolegry[Math.Min(ROW, line) - j - 1][start_col + j];
                while (napis6.Contains("2222222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222222222"), 10);
                }
                while (napis6.Contains("222222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("222222222"), 9);
                }
                while (napis6.Contains("22222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("22222222"), 8);
                }
                while (napis6.Contains("2222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222222"), 7);
                }
                while (napis6.Contains("222222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("222222"), 6);
                }
                while (napis6.Contains("22222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("22222"), 5);
                }
                while (napis6.Contains("2222"))
                {
                    napis6 = napis6.Remove(napis6.IndexOf("2222"), 4);
                    wynik++;
                }
            }
            return wynik;
        }
        private static object GetCopy(object input)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                stream.Position = 0;
                return formatter.Deserialize(stream);
            }
        }
        static float minimax(Node node, int depth, bool maximizingPlayer)
        {
            if (depth == 0 || node.children.Count == 0)
            {
                float[] tab = new float[2];
                tab = evaluateBoard(node.polegry);
                node.positionValue = tab[0] + (-1) * tab[1];
                return node.positionValue;
            }
            if (maximizingPlayer)
            {
                float value = float.MinValue;
                foreach (Node element in node.children)
                {
                    value = max(value, minimax(element, depth - 1, false));
                }
                return value;
            }
            else
            {
                float value = float.MaxValue;
                foreach (Node element in node.children)
                {
                    value = min(value, minimax(element, depth - 1, true));
                }
                return value;
            }
        }
        static float alphabeta(Node node, int depth, float alpha, float beta, bool maximizingPlayer)
        {
            if (depth == 0 || node.children.Count == 0)
            {
                float[] tab = new float[2];
                tab = evaluateBoard(node.polegry);
                node.positionValue = tab[0] + (-1) * tab[1];
                return node.positionValue;
            }
            if (maximizingPlayer)
            {
                float value = float.MinValue;
                foreach (Node element in node.children)
                {
                    value = max(value, alphabeta(element, depth - 1, alpha, beta, false));
                    alpha = max(alpha, value);
                    if (alpha >= beta) break;
                }
                return value;
            }
            else
            {
                float value = float.MaxValue;
                foreach (Node element in node.children)
                {
                    value = min(value, alphabeta(element, depth - 1, alpha, beta, true));
                    beta = min(beta, value);
                    if (alpha >= beta) break;
                }
                return value;
            }
        }
        static float min(float a, float b)
        {
            if (a < b) return a;
            else return b;
        }
        static float max(float a, float b)
        {
            if (a > b) return a;
            else return b;
        }
        static float[] evaluateBoard(char[][] polegry)
        {
            float[] tab = new float[2];
            tab[0] = ocenWartoscBialych(polegry);
            tab[1] = ocenWartoscCzarnych(polegry);
            return tab;
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        for(int i=0;i<COL;i++)polegry[i] = new char[COL];
            polegry[0] = "0000000000".ToCharArray();
            polegry[1] = "0000000000".ToCharArray();
            polegry[2] = "0000000000".ToCharArray();
            polegry[3] = "0000000000".ToCharArray();
            polegry[4] = "0000000000".ToCharArray();
            polegry[5] = "0000000000".ToCharArray();
            polegry[6] = "0000000000".ToCharArray();
            polegry[7] = "0000000000".ToCharArray();
            polegry[8] = "0000000000".ToCharArray();
            polegry[9] = "0000000000".ToCharArray();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            char[] TC;
            b.Text = "X";
            string letters = b.Name;
            TC = letters.ToCharArray();

            b.Enabled = false;
            int i = TC[1] - '0';
            int j = TC[2] - '0';

            polegry[i][j] = '1';
            
           
            
            BinaryTree drzewo = new BinaryTree();
            drzewo = drzewoGry(polegry);
            float wartosc = wnioskuj(drzewo.root , true);
            Node node = drzewo.root.FindChild(drzewo.root, wartosc);
            
            Console.WriteLine();
            for (int k = 0; k < 10; k++)
            {
                for (int l = 0; l < 10; l++)
                {
                    if (polegry[k][l] != node.polegry[k][l])
                        {
                        i = k;
                        Console.WriteLine(i);
                        j = l;
                        Console.WriteLine(j);
                    }
                }
            }
            polegry[i][j] = '2';
            wypisz(polegry);
            if (i == 1 && j == 0) { a10.Text = "O"; a10.Enabled = false; }
            if (i == 1 && j == 1) { a11.Text = "O"; a11.Enabled = false; }
            if (i == 1 && j == 2) { a12.Text = "O"; a12.Enabled = false; }
            if (i == 1 && j == 3) { a13.Text = "O"; a13.Enabled = false; }
            if (i == 1 && j == 4) { a14.Text = "O"; a14.Enabled = false; }
            if (i == 1 && j == 5) { a15.Text = "O"; a15.Enabled = false; }
            if (i == 1 && j == 6) { a16.Text = "O"; a16.Enabled = false; }
            if (i == 1 && j == 7) { a17.Text = "O"; a17.Enabled = false; }
            if (i == 1 && j == 8) { a18.Text = "O"; a18.Enabled = false; }
            if (i == 1 && j == 9) { a19.Text = "O"; a19.Enabled = false; }

            if (i == 0 && j == 0) { a00.Text = "O"; a00.Enabled = false; }
            if (i == 0 && j == 1) { a01.Text = "O"; a01.Enabled = false; }
            if (i == 0 && j == 2) { a02.Text = "O"; a02.Enabled = false; }
            if (i == 0 && j == 3) { a03.Text = "O"; a03.Enabled = false; }
            if (i == 0 && j == 4) { a04.Text = "O"; a04.Enabled = false; }
            if (i == 0 && j == 5) { a05.Text = "O"; a05.Enabled = false; }
            if (i == 0 && j == 6) { a06.Text = "O"; a06.Enabled = false; }
            if (i == 0 && j == 7) { a07.Text = "O"; a07.Enabled = false; }
            if (i == 0 && j == 8) { a08.Text = "O"; a08.Enabled = false; }
            if (i == 0 && j == 9) { a09.Text = "O"; a09.Enabled = false; }

            if (i == 2 && j == 0) { a20.Text = "O"; a20.Enabled = false; }
            if (i == 2 && j == 1) { a21.Text = "O"; a21.Enabled = false; }
            if (i == 2 && j == 2) { a22.Text = "O"; a22.Enabled = false; }
            if (i == 2 && j == 3) { a23.Text = "O"; a23.Enabled = false; }
            if (i == 2 && j == 4) { a24.Text = "O"; a24.Enabled = false; }
            if (i == 2 && j == 5) { a25.Text = "O"; a25.Enabled = false; }
            if (i == 2 && j == 6) { a26.Text = "O"; a26.Enabled = false; }
            if (i == 2 && j == 7) { a27.Text = "O"; a27.Enabled = false; }
            if (i == 2 && j == 8) { a28.Text = "O"; a28.Enabled = false; }
            if (i == 2 && j == 9) { a29.Text = "O"; a29.Enabled = false; }
        
            if (i == 3 && j == 0) { a30.Text = "O"; a30.Enabled = false; }
            if (i == 3 && j == 1) { a31.Text = "O"; a31.Enabled = false; }
            if (i == 3 && j == 2) { a32.Text = "O"; a32.Enabled = false; }
            if (i == 3 && j == 3) { a33.Text = "O"; a33.Enabled = false; }
            if (i == 3 && j == 4) { a34.Text = "O"; a34.Enabled = false; }
            if (i == 3 && j == 5) { a35.Text = "O"; a35.Enabled = false; }
            if (i == 3 && j == 6) { a36.Text = "O"; a36.Enabled = false; }
            if (i == 3 && j == 7) { a37.Text = "O"; a37.Enabled = false; }
            if (i == 3 && j == 8) { a38.Text = "O"; a38.Enabled = false; }
            if (i == 3 && j == 9) { a39.Text = "O"; a39.Enabled = false; }

            if (i == 4 && j == 0) { a40.Text = "O"; a40.Enabled = false; }
            if (i == 4 && j == 1) { a41.Text = "O"; a41.Enabled = false; }
            if (i == 4 && j == 2) { a42.Text = "O"; a42.Enabled = false; }
            if (i == 4 && j == 3) { a43.Text = "O"; a43.Enabled = false; }
            if (i == 4 && j == 4) { a44.Text = "O"; a44.Enabled = false; }
            if (i == 4 && j == 5) { a45.Text = "O"; a45.Enabled = false; }
            if (i == 4 && j == 6) { a46.Text = "O"; a46.Enabled = false; }
            if (i == 4 && j == 7) { a47.Text = "O"; a47.Enabled = false; }
            if (i == 4 && j == 8) { a48.Text = "O"; a48.Enabled = false; }
            if (i == 4 && j == 9) { a49.Text = "O"; a49.Enabled = false; }

            if (i == 5 && j == 0) { a50.Text = "O"; a50.Enabled = false; }
            if (i == 5 && j == 1) { a51.Text = "O"; a51.Enabled = false; }
            if (i == 5 && j == 2) { a52.Text = "O"; a52.Enabled = false; }
            if (i == 5 && j == 3) { a53.Text = "O"; a53.Enabled = false; }
            if (i == 5 && j == 4) { a54.Text = "O"; a54.Enabled = false; }
            if (i == 5 && j == 5) { a55.Text = "O"; a55.Enabled = false; }
            if (i == 5 && j == 6) { a56.Text = "O"; a56.Enabled = false; }
            if (i == 5 && j == 7) { a57.Text = "O"; a57.Enabled = false; }
            if (i == 5 && j == 8) { a58.Text = "O"; a58.Enabled = false; }
            if (i == 5 && j == 9) { a59.Text = "O"; a59.Enabled = false; }
        
            if (i == 6 && j == 0) { a60.Text = "O"; a60.Enabled = false; }
            if (i == 6 && j == 1) { a61.Text = "O"; a61.Enabled = false; }
            if (i == 6 && j == 2) { a62.Text = "O"; a62.Enabled = false; }
            if (i == 6 && j == 3) { a63.Text = "O"; a63.Enabled = false; }
            if (i == 6 && j == 4) { a64.Text = "O"; a64.Enabled = false; }
            if (i == 6 && j == 5) { a65.Text = "O"; a65.Enabled = false; }
            if (i == 6 && j == 6) { a66.Text = "O"; a66.Enabled = false; }
            if (i == 6 && j == 7) { a67.Text = "O"; a67.Enabled = false; }
            if (i == 6 && j == 8) { a68.Text = "O"; a68.Enabled = false; }
            if (i == 6 && j == 9) { a69.Text = "O"; a69.Enabled = false; }

            if (i == 7 && j == 0) { a70.Text = "O"; a70.Enabled = false; }
            if (i == 7 && j == 1) { a71.Text = "O"; a71.Enabled = false; }
            if (i == 7 && j == 2) { a72.Text = "O"; a72.Enabled = false; }
            if (i == 7 && j == 3) { a73.Text = "O"; a73.Enabled = false; }
            if (i == 7 && j == 4) { a74.Text = "O"; a74.Enabled = false; }
            if (i == 7 && j == 5) { a75.Text = "O"; a75.Enabled = false; }
            if (i == 7 && j == 6) { a76.Text = "O"; a76.Enabled = false; }
            if (i == 7 && j == 7) { a77.Text = "O"; a77.Enabled = false; }
            if (i == 7 && j == 8) { a78.Text = "O"; a78.Enabled = false; }
            if (i == 7 && j == 9) { a79.Text = "O"; a79.Enabled = false; }

            if (i == 8 && j == 0) { a80.Text = "O"; a80.Enabled = false; }
            if (i == 8 && j == 1) { a81.Text = "O"; a81.Enabled = false; }
            if (i == 8 && j == 2) { a82.Text = "O"; a82.Enabled = false; }
            if (i == 8 && j == 3) { a83.Text = "O"; a83.Enabled = false; }
            if (i == 8 && j == 4) { a84.Text = "O"; a84.Enabled = false; }
            if (i == 8 && j == 5) { a85.Text = "O"; a85.Enabled = false; }
            if (i == 8 && j == 6) { a86.Text = "O"; a86.Enabled = false; }
            if (i == 8 && j == 7) { a87.Text = "O"; a87.Enabled = false; }
            if (i == 8 && j == 8) { a88.Text = "O"; a88.Enabled = false; }
            if (i == 8 && j == 9) { a89.Text = "O"; a89.Enabled = false; }

            if (i == 9 && j == 0) { a90.Text = "O"; a90.Enabled = false; }
            if (i == 9 && j == 1) { a91.Text = "O"; a91.Enabled = false; }
            if (i == 9 && j == 2) { a92.Text = "O"; a92.Enabled = false; }
            if (i == 9 && j == 3) { a93.Text = "O"; a93.Enabled = false; }
            if (i == 9 && j == 4) { a94.Text = "O"; a94.Enabled = false; }
            if (i == 9 && j == 5) { a95.Text = "O"; a95.Enabled = false; }
            if (i == 9 && j == 6) { a96.Text = "O"; a96.Enabled = false; }
            if (i == 9 && j == 7) { a97.Text = "O"; a97.Enabled = false; }
            if (i == 9 && j == 8) { a98.Text = "O"; a98.Enabled = false; }
            if (i == 9 && j == 9) { a99.Text = "O"; a99.Enabled = false; }




            int XPoint = policzBiale(polegry);
            int YPoint = policzCzarne(polegry);

            X_point_count.Text = XPoint.ToString();
            Y_point_count.Text = YPoint.ToString();       



        }
        static void wypisz(char[][] polegry)
        {
            for (int i = 0; i < ROW; i++)
            {
                Console.WriteLine(polegry[i]);
            }
        }
        private void check_winning(int i, int j) //atrapa
        {
            
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
                    Console.Write(polegry[i][j] + " ");
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
                    polegry[i][j] = '0';
                }

            }


            Console.Clear();
        }      
    }
}
