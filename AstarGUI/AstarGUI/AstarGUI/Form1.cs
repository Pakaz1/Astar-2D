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
        public static mapArray map = new mapArray(10);

        public Button[,] buttonGrid = new Button[map.Size + 2, map.Size + 2];
        public Form1()
        {
            InitializeComponent();
            populateGrid();
        }

        private void populateGrid()
        {
            int buttonSize = panel1.Width / map.Size;
            panel1.Height = panel1.Width;
            for(int i = 0; i <= map.Size + 1; i++)
            {
                buttonGrid[0, i] = new Button();
                buttonGrid[i, 0] = new Button();
                buttonGrid[map.Size + 1, i] = new Button();
                buttonGrid[i, map.Size + 1] = new Button();
            }
            for(int i = 0; i < map.Size; i++)
            {
                for(int j = 0; j < map.Size; j++)
                {
                    buttonGrid[i + 1, j + 1] = new Button();

                    buttonGrid[i + 1, j + 1].Height = buttonSize;
                    buttonGrid[i + 1, j + 1].Width = buttonSize;

                    buttonGrid[i + 1, j + 1].Click += Grid_Button_Click;

                    panel1.Controls.Add(buttonGrid[i + 1, j + 1]);

                    buttonGrid[i + 1, j + 1].Location = new Point(i * buttonSize, j * buttonSize);

                    buttonGrid[i + 1, j + 1].Text = i + 1 + "|" + j + 1;
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

            for(int i = 1; i <= map.Size; i++)
            {
                for(int j = 1; j <= map.Size; j++)
                {
                    if (buttonGrid[i, j].Text == "A")
                    {
                        oldAx = i;
                        oldAy = j;
                        //foundA = true;
                    }
                    else if (buttonGrid[i, j].Text == "B")
                    {
                        oldBx = i;
                        oldBy = j;
                        //foundB = true;
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

            string[] mapForAstar = new string[map.Size + 2];
            string a = "";
            for (int i = 0; i <= map.Size; i++)
                a += "+";
            mapForAstar[0] = a;
            mapForAstar[map.Size + 1] = a;

            Astar_Algorithm.NodeInformation start = new Astar_Algorithm.NodeInformation();
            Astar_Algorithm.NodeInformation goal = new Astar_Algorithm.NodeInformation();
            for (int i = 1; i <= map.Size; i++)
            {
                a = "+";
                for (int j = 1; j <= map.Size; j++)
                {
                    //a += buttonGrid[i, j].Text;
                    if (buttonGrid[j, i].Text == "A")
                    {
                        a += "A";
                        foundA = true;
                        start.X = j;
                        start.Y = i;
                    }
                    else if (buttonGrid[j, i].Text == "B")
                    {
                        a += "B";
                        foundB = true;
                        goal.X = j;
                        goal.Y = i;
                    }
                    else if (buttonGrid[j, i].Text == "")
                    {
                        a += " ";
                    }
                    else a += "X";
                }
                a += "+";
                mapForAstar[i] = a;
            }
            if (foundA && foundB)
            {
                SimplePriorityQueue<Astar_Algorithm.NodeInformation> path_to_take = Astar_Algorithm.Program.Astar(mapForAstar, start, goal);
                if (path_to_take != null)
                {
                    path_to_take.Remove(path_to_take.Last());
                    path_to_take.Remove(path_to_take.First());
                    //queue.Remove(queue.Last());
                    while (path_to_take.Count > 0)
                    {
                        Astar_Algorithm.NodeInformation node = path_to_take.Dequeue();
                        buttonGrid[node.X, node.Y].Text = "*";
                    }
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
    }
}
