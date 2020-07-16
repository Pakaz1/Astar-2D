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
        public static int mapSize = 10;

        bool Aexists = false;
        int oldAx = -1;
        int oldAy = -1;

        bool Bexists = false;
        int oldBx = -1;
        int oldBy = -1;

        public Button[,] buttonGrid = new Button[mapSize + 2, mapSize + 2];
        public Form1()
        {
            InitializeComponent();
            populateGrid();
            //The properties on numericUpDown always reset so I am doing them here
            numericUpDown1.Value = 10;
            numericUpDown1.Minimum = 1;
            //--------------------------------------------------------------------
            //comboBoxes originally don't have this value or I am blind
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
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

                    //buttonGrid[i + 1, j + 1].Text = (i + 1) + "|" + (j + 1);
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

            //This is a lot to take in but its faster than going through the whole map grid and finding A, B individually
            //Few if statements versus N x N grid with statements
            switch (comboBox1.SelectedIndex)
            {
                //If the user is placing the starting point(A's)
                case 0:
                    //Check if the new A replaces the already standing B
                    if(Bexists && x == oldBx && y == oldBy)
                    {
                        Bexists = false;
                        oldBx = -1;
                        oldBy = -1;
                        resetMap();
                    }
                    if(Aexists)
                    {
                        Aexists = (oldAx == x && oldAy == y ? false : true);
                        //We replace old A with a new one if it existed before
                        if (Aexists)
                        {
                            buttonGrid[x, y].Text = "A";
                            buttonGrid[oldAx, oldAy].Text = "";
                            oldAx = x;
                            oldAy = y;
                        }
                        //If the coordinates matched we remove A
                        else
                        {
                            buttonGrid[x, y].Text = "";
                            resetMap();
                        }
                    }
                    //There are no A's on the grid and we just place one
                    else
                    {
                        buttonGrid[x, y].Text = "A";
                        oldAx = x;
                        oldAy = y;
                        Aexists = true;
                    }

                    break;
                //If the user is placing the goal point(B's)
                case 1:
                    //Check if the new B replaces the aldready standing A
                    if (Aexists && x == oldAx && y == oldAy)
                    {
                        Aexists = false;
                        oldAx = -1;
                        oldAy = -1;
                        resetMap();
                    }
                    if (Bexists)
                    {
                        Bexists = (oldBx == x && oldBy == y ? false : true);
                        //We replace old B with a new one if it existed before
                        if (Bexists)
                        {
                            buttonGrid[x, y].Text = "B";
                            buttonGrid[oldBx, oldBy].Text = "";
                            oldBx = x;
                            oldBy = y;
                        }
                        //If the coordinates matched we remove B
                        else
                        {
                            buttonGrid[x, y].Text = "";
                            resetMap();
                        }
                    }
                    //There are no B's on the grid and we just place one
                    else
                    {
                        buttonGrid[x, y].Text = "B";
                        oldBx = x;
                        oldBy = y;
                        Bexists = true;
                    }
                    break;
                //If the user is placing Walls(X's)
                case 2:
                    //Check if the wall is replacing any of the existing starting/goal positions and reset the map if they are
                    if(Aexists && oldAx == x && oldAy == y)
                    {
                        Aexists = false;
                        oldAx = -1;
                        oldAy = -1;
                        resetMap();
                    }
                    if (Bexists && oldBx == x && oldBy == y)
                    {
                        Bexists = false;
                        oldBx = -1;
                        oldBy = -1;
                        resetMap();
                    }
                    buttonGrid[x, y].Text = buttonGrid[x, y].Text != "X" ? "X" : ""; 
                    break;
            }
            checkAndDrawMap();
        }
        /// <summary>
        /// Removes all values except for A, B and X
        /// </summary>
        public void resetMap()
        {
            string[] values = { "A", "B", "X" };
            for (int i = 1; i <= mapSize; i++)
            {
                for (int j = 1; j <= mapSize; j++)
                {
                    if (values.Contains(buttonGrid[i, j].Text)) continue;
                    else
                        buttonGrid[i, j].Text = "";
                }
            }
        }
        /// <summary>
        /// Checks if A and B exists and we apply A* algorithm if they do
        /// </summary>
        public void checkAndDrawMap()
        {
            if (!Aexists || !Bexists) return; //A and B simply doesnt exist so theres no point in trying to do anything with the algorithm

            resetMap();

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
                        start.X = j;
                        start.Y = i;
                    }
                    else if (buttonGrid[j, i].Text == "B")
                    {
                        goal.X = j;
                        goal.Y = i;
                    }
                }
                mapForAstar[i] = a + "+";
            }
            calculateAndShowAstar(mapForAstar, start, goal);
        }
        public void calculateAndShowAstar(string[] mapForAstar, NodeInformation start, NodeInformation goal)
        {
            SimplePriorityQueue<Astar_Algorithm.NodeInformation> path_to_take = Astar_Algorithm.Program.Astar(mapForAstar, start, goal, comboBox2.SelectedIndex, comboBox3.SelectedIndex);
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown upDown = (NumericUpDown)sender;
            panel1.Controls.Clear();
            mapSize = (int) upDown.Value;
            buttonGrid = new Button[mapSize + 2, mapSize + 2];
            populateGrid();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkAndDrawMap();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkAndDrawMap();
        }
    }
}
