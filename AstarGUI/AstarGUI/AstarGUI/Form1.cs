using Astar_Algorithm;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstarGUI
{
    public partial class Form1 : Form
    {
        //public static mapArray map = new mapArray(10);
        public static int mapSize = 10;

        public Button[,] buttonGrid = new Button[mapSize + 2, mapSize + 2];
        public Form1()
        {
            InitializeComponent();
            populateGrid();
            //The properties on numericUpDown always reset so I am doing them here
            numericUpDown1.Value = 10;
            numericUpDown1.Minimum = 1;
            //--------------------------------------------------------------------
        }

        private void populateGrid()
        {
            int buttonSize = panel1.Width / mapSize;
            panel1.Height = panel1.Width;
            for(int i = 0; i <= mapSize + 1; i++)
            {
                buttonGrid[0, i] = new Button();
                buttonGrid[i, 0] = new Button();
                buttonGrid[mapSize + 1, i] = new Button();
                buttonGrid[i, mapSize + 1] = new Button();
            }

            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    buttonGrid[i + 1, j + 1] = new Button();

                    buttonGrid[i + 1, j + 1].Height = buttonSize;
                    buttonGrid[i + 1, j + 1].Width = buttonSize;

                    buttonGrid[i + 1, j + 1].Click += Grid_Button_Click;

                    panel1.Controls.Add(buttonGrid[i + 1, j + 1]);

                    buttonGrid[i + 1, j + 1].Location = new Point(i * buttonSize, j * buttonSize);

                    buttonGrid[i + 1, j + 1].Text = (i + 1) + "|" + (j + 1);
                    buttonGrid[i + 1, j + 1].Tag = new Point(i + 1, j + 1);
                }
            }
        }
        private void Grid_Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button) sender;
            Point location = (Point) clickedButton.Tag;


            int x = location.X;
            int y = location.Y;

            int oldAx = 0;
            int oldAy = 0;

            int oldBx = 0;
            int oldBy = 0;

            bool foundA = false;
            bool foundB = false;


            for(int i = 1; i <= mapSize; i++)
            {
                for(int j = 1; j <= mapSize; j++)
                {
                    if (buttonGrid[i, j].Text == "A")
                    {
                        oldAx = i;
                        oldAy = j;
                    }
                    else if (buttonGrid[i, j].Text == "B")
                    {
                        oldBx = i;
                        oldBy = j;
                    }
                    else if (buttonGrid[i, j].Text == "X") continue;
                    else
                        buttonGrid[i, j].Text = "";
                }
            }
            
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    buttonGrid[x, y].Text = "A";
                    buttonGrid[oldAx, oldAy].Text = "";
                    break;
                case 1:
                    buttonGrid[x, y].Text = "B";
                    buttonGrid[oldBx, oldBy].Text = "";
                    break;
                case 2:
                    if (buttonGrid[x, y].Text != "X") buttonGrid[x, y].Text = "X";
                    else buttonGrid[x, y].Text = "";
                    break;
            }

            string[] mapForAstar = new string[mapSize + 2]; //Map for Astar algorithm | +2 because top and bottom are extra boundries

            string a = string.Concat(Enumerable.Repeat("+", mapSize + 2));

            mapForAstar[0] = a;
            mapForAstar[mapSize + 1] = a;

            Astar_Algorithm.NodeInformation start = new Astar_Algorithm.NodeInformation();
            Astar_Algorithm.NodeInformation goal = new Astar_Algorithm.NodeInformation();

            for (int i = 1; i <= mapSize; i++)
            {
                a = "+";
                for (int j = 1; j <= mapSize; j++)
                {
                    a += buttonGrid[j, i].Text == "" ? " " : buttonGrid[j, i].Text;
                    if (buttonGrid[j, i].Text == "A")
                    {
                        foundA = true;
                        start.X = j;
                        start.Y = i;
                    }
                    else if (buttonGrid[j, i].Text == "B")
                    {
                        foundB = true;
                        goal.X = j;
                        goal.Y = i;
                    }
                }
                mapForAstar[i] = a + "+";
            }
            if (foundA && foundB)
            {
                calculateAndShowAstar(mapForAstar, start, goal);
            }
        }
        public void calculateAndShowAstar(string[] mapForAstar, NodeInformation start, NodeInformation goal)
        {
            SimplePriorityQueue<Astar_Algorithm.NodeInformation> path_to_take = Astar_Algorithm.Program.Astar(mapForAstar, start, goal);
            if (path_to_take != null)
            {
                path_to_take.Remove(path_to_take.Last());
                path_to_take.Remove(path_to_take.First());
                while (path_to_take.Count > 0)
                {
                    Astar_Algorithm.NodeInformation node = path_to_take.Dequeue();
                    buttonGrid[node.X, node.Y].Text = "*";
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown)sender;
            panel1.Controls.Clear();
            mapSize = (int) upDown.Value;
            buttonGrid = new Button[mapSize + 2, mapSize + 2];
            populateGrid();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
