using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tarjan
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader reader = File.OpenText("C:\\Users\\Saumya\\Downloads\\Project3-SS\\Tarjan\\GraphInput.txt");
            int vertexCount = int.Parse(reader.ReadLine());
            DepGraph graph = DepGraph.CreateGraph(vertexCount);
            for (int i = 0; i < vertexCount; i++)
            {
                String[] edges = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < edges.Length - 1; j++)
                {
                    int d = int.Parse(edges[j]);
                    graph.Vertices[i].Dependencies.Add(graph.Vertices[d - 1]);
                }
            }

            TarjanCycleDetect tarjan = new TarjanCycleDetect();
            List<List<Vertex>> nodes = tarjan.DetectCycle(graph);
            nodes[2].ForEach(Console.WriteLine);

            foreach (object elem in nodes)
            {
                Console.WriteLine(elem);
            }
            Console.WriteLine(nodes);
            Console.ReadLine();
        }
    }
        public class Vertex
        {
            public int ID = -1;
            public int index = -1;
            public int lowlink = -1;
            public List<Vertex> Dependencies = new List<Vertex>();
        }


        public class DepGraph
        {
            public readonly List<Vertex> Vertices = new List<Vertex>();

            public static DepGraph CreateGraph(int nodeCount)
            {
                DepGraph graph = new DepGraph();
                for (int i = 1; i <= nodeCount; i++)
                {
                    Vertex v = new Vertex();
                    v.ID = i;
                    graph.Vertices.Add(v);
                }
                return graph;
            }
        }


        public class TarjanCycleDetect
        {
            private List<List<Vertex>> output;
            private Stack<Vertex> nodeStatck;
            private int index;

            public List<List<Vertex>> DetectCycle(DepGraph g)
            {
                output = new List<List<Vertex>>();
                index = 0;
                nodeStatck = new Stack<Vertex>();
                foreach (Vertex v in g.Vertices)
                {
                    if (v.index < 0)
                    {
                        StrongConnect(v);
                    }
                }
                return output;
            }

            private void StrongConnect(Vertex v)
            {
                v.index = index;
                v.lowlink = index;
                index++;
                nodeStatck.Push(v);

                foreach (Vertex w in v.Dependencies)
                {
                    if (w.index < 0)
                    {
                        StrongConnect(w);
                        v.lowlink = Math.Min(v.lowlink, w.lowlink);
                    }
                    else if (nodeStatck.Contains(w))
                    {
                        v.lowlink = Math.Min(v.lowlink, w.index);
                    }
                }

                if (v.lowlink == v.index)
                {
                    List<Vertex> scc = new List<Vertex>();
                    Vertex w;
                    do
                    {
                        w = nodeStatck.Pop();
                        scc.Add(w);
                    } while (v != w);
                    output.Add(scc);
                }
            }
        }
    }
