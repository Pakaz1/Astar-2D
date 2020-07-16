using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Astar_Algorithm
{
    public class NodeInformation
    {
        public int X; //Nodes X location
        public int Y; //Nodes Y location
        public int G = 1; //Value from node to node
        public int H; //This is the value from node to X
        public int F; //Sum value of G and H
        public NodeInformation Parent = null;

        public bool compareXY(NodeInformation compareObject)
        {
            return (X == compareObject.X && Y == compareObject.Y);
        }
    }
    public class Program
    {
        /*
        public static string[] map = new string[]     {"+-----------------+",
                                                       "|A  X             |",
                                                       "| X X X           |",
                                                       "|XX   X     B     |",
                                                       "|   XXX           |",
                                                       "+-----------------+",
                                        };
        */
        static void Main(string[] args)
        {
            /*
            foreach (var a in map)
                Console.WriteLine(a);

            NodeInformation start = new NodeInformation { X = 1, Y = 1 };
            NodeInformation goal = new NodeInformation { X = 12, Y = 3 };
            SimplePriorityQueue<NodeInformation> queue = Astar(map, start, goal);
            if (queue == null)
            {
                Console.WriteLine("No path was found");
            }
            else
            {
                queue.Remove(queue.Last());
                queue.Remove(queue.First());
                //queue.Remove(queue.Last());
                while (queue.Count > 0)
                {
                    NodeInformation node = queue.Dequeue();
                    Console.WriteLine(node.X + " " + node.Y);
                    //Ugly way to insert a "."s into the map by path queue
                    char[] line = map[node.Y].ToCharArray();
                    line[node.X] = '.';
                    map[node.Y] = String.Join("", line);
                    //
                    foreach (var a in map)
                        Console.WriteLine(a);
                }
            }
            foreach (var a in map)
                Console.WriteLine(a);
            Console.ReadKey();
            */
        }
        public static SimplePriorityQueue<NodeInformation> Astar(string[] map, NodeInformation start, NodeInformation goal, int distanceCalculateValue, int neighbourValue)
        {
            NodeInformation currentNode = null;

            SimplePriorityQueue<NodeInformation> openSet = new SimplePriorityQueue<NodeInformation>();

            openSet.Enqueue(start, 0);

            Dictionary<NodeInformation, NodeInformation> cameFrom = new Dictionary<NodeInformation, NodeInformation>();

            Dictionary<NodeInformation, int> gScore = new Dictionary<NodeInformation, int>();
            gScore[start] = 0;

            Dictionary<NodeInformation, int> fScore = new Dictionary<NodeInformation, int>();
            fScore[start] = gScore[start] + calculateHEuclidianValue(start, goal);

            List<NodeInformation> closedList = new List<NodeInformation>();

            while (openSet.Count > 0)
            {
                currentNode = openSet.Dequeue();
                if (currentNode.X == goal.X && currentNode.Y == goal.Y)
                    return reconstructPath(currentNode);

                closedList.Add(currentNode);
                List<NodeInformation> neighbours = calculateNeighbours(currentNode, map, neighbourValue);
                foreach(var neighbour in neighbours)
                {
                    if (closedList.Exists(o => o.X == neighbour.X && o.Y == neighbour.Y))
                        continue;

                    neighbour.H = distanceCalculateValue == 0 ? calculateHManhattanValue(neighbour, goal) : calculateHEuclidianValue(neighbour, goal);
                    neighbour.G = currentNode.G + 1;
                    neighbour.F = neighbour.H + neighbour.G;
                    neighbour.Parent = currentNode;

                    //There should be an if statement to check if coming from this node is better than any other before
                    //Since it's a 2D array I don't think this really matters since you're most likely already going from the best position

                    //Writing an equals in NodeInformation class and using .Contains() might be more clear than this, but this works just fine
                    if (!openSet.Any(o => o.X == neighbour.X && o.Y == neighbour.Y))
                    {
                        openSet.Enqueue(neighbour, neighbour.F);//fScore[neighbour]);
                    }
                }
            }
            return null; //If all failed
        }
        static List<NodeInformation> calculateNeighbours(NodeInformation current, string[] map, int value)
        {
            //Value 0 for 4 neighbours and Value 1 for 8 neighbours
            List<NodeInformation> newList = new List<NodeInformation>();
            //Gather all possible neighbours(depending on selected directions)
            newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y });
            newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y });
            newList.Add(new NodeInformation { X = current.X, Y = current.Y + 1 });
            newList.Add(new NodeInformation { X = current.X, Y = current.Y - 1 });
            //Selected value is for 8 neighbours so add the 4 corner ones
            if (value == 1)
            {
                newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y + 1});
                newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y - 1 });
                newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y + 1 });
                newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y - 1 });
            }
            return newList.Where(o => map[o.Y][o.X] == ' ' || map[o.Y][o.X] == 'B').ToList();
        }
        static int calculateHManhattanValue(NodeInformation current, NodeInformation goal)
        {
            return Math.Abs(current.X - goal.X) + Math.Abs(current.Y - goal.Y);
        }
        static int calculateHEuclidianValue(NodeInformation current, NodeInformation goal)
        {
            return (int) Math.Sqrt(Math.Pow(current.X - goal.X, 2) + Math.Pow(current.Y - goal.Y, 2));
        }
        static SimplePriorityQueue<NodeInformation> reconstructPath(NodeInformation current)
        {
            SimplePriorityQueue<NodeInformation> total_path = new SimplePriorityQueue<NodeInformation>();
            total_path.Enqueue(current, 0);
            while(current.Parent != null)
            {
                current = current.Parent;
                total_path.Enqueue(current, 0);
            }
            return total_path;
        }
    }
}
