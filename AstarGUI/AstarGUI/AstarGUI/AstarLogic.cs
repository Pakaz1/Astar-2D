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
    }
    class AstarLogic
    {
        public static string[] map = new string[]     {"+-----------------+",
                                                       "|A  X             |",
                                                       "| X X X           |",
                                                       "|XX   X     B     |",
                                                       "|   XXX           |",
                                                       "+-----------------+",
                                        };
        static void Main(string[] args)
        {
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
        }
        public static SimplePriorityQueue<NodeInformation> Astar(string[] map, NodeInformation start, NodeInformation goal)
        {
            NodeInformation currentNode = null;
            //NodeInformation start = new NodeInformation { X = 1, Y = 1 };
            //NodeInformation goal = new NodeInformation { X = 12, Y = 3 };

            SimplePriorityQueue<NodeInformation> openSet = new SimplePriorityQueue<NodeInformation>();

            openSet.Enqueue(start, 0);

            Dictionary<NodeInformation, NodeInformation> cameFrom = new Dictionary<NodeInformation, NodeInformation>();

            Dictionary<NodeInformation, int> gScore = new Dictionary<NodeInformation, int>();
            gScore[start] = 0;

            Dictionary<NodeInformation, int> fScore = new Dictionary<NodeInformation, int>();
            fScore[start] = gScore[start] + calculateHValue(start, goal);

            List<NodeInformation> closedList = new List<NodeInformation>();

            while (openSet.Count > 0)
            {
                currentNode = openSet.Dequeue();
                if (currentNode.X == goal.X && currentNode.Y == goal.Y)
                    return reconstructPath(currentNode);

                closedList.Add(currentNode);
                List<NodeInformation> neighbours = calculateNeighbours(currentNode);
                foreach (var neighbour in neighbours)
                {
                    if (closedList.Count(o => o.X == neighbour.X && o.Y == neighbour.Y) > 0) //The way this is implemented could be better/maybe will fix later
                        continue;
                    neighbour.H = calculateHValue(neighbour, goal);
                    neighbour.G = currentNode.G + 1;
                    neighbour.F = neighbour.H + neighbour.G;
                    neighbour.Parent = currentNode;
                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Enqueue(neighbour, neighbour.F);//fScore[neighbour]);
                    }
                }
            }
            return null; //If all failed
        }
        static List<NodeInformation> calculateNeighbours(NodeInformation current)
        {
            List<NodeInformation> newList = new List<NodeInformation>();
            //Gather all possible neighbours(4 directions right now)
            if (map[current.Y].ToCharArray()[current.X + 1] == ' ' || map[current.Y].ToCharArray()[current.X + 1] == 'B') newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y });
            if (map[current.Y].ToCharArray()[current.X - 1] == ' ' || map[current.Y].ToCharArray()[current.X - 1] == 'B') newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y });
            if (map[current.Y + 1].ToCharArray()[current.X] == ' ' || map[current.Y + 1].ToCharArray()[current.X] == 'B') newList.Add(new NodeInformation { X = current.X, Y = current.Y + 1 });
            if (map[current.Y - 1].ToCharArray()[current.X] == ' ' || map[current.Y - 1].ToCharArray()[current.X] == 'B') newList.Add(new NodeInformation { X = current.X, Y = current.Y - 1 });
            return newList;
        }
        static int calculateHValue(NodeInformation current, NodeInformation goal)
        {
            return Math.Abs(current.X - goal.X) + Math.Abs(current.Y - goal.Y);
        }
        static SimplePriorityQueue<NodeInformation> reconstructPath(NodeInformation current)
        {
            SimplePriorityQueue<NodeInformation> total_path = new SimplePriorityQueue<NodeInformation>();
            total_path.Enqueue(current, 0);
            while (current.Parent != null)
            {
                current = current.Parent;
                total_path.Enqueue(current, 0);
            }
            return total_path;
        }
    }
}
