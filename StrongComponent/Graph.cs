///////////////////////////////////////////////////////////////////////
// Executive.cs - Demonstrate Prototype Code Analyzer                //
// ver 1.0                                                           //
// Language:    C#, 2017, .Net Framework 4.7.1                       //
// Platform:    Dell Precision T8900, Win10                          //
// Application: Demonstration for CSE681, Project #3, Fall 2018      //
// Author:      Saumya Sharma                                       //
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines the following class:
 *   Graph:
 *   - constructs graph from nodes and outputs the strongly connected components of graph
 */
/* 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrongComponent
{
    public class CsEdge<V, E> // holds child node and instance of edge type E
    {
        public CsNode<V, E> targetNode { get; set; } = null;
        public E edgeValue { get; set; }

        public CsEdge(CsNode<V, E> node, E value)
        {
            targetNode = node;
            edgeValue = value;
        }
    };

    public class CsNode<V, E>
    {
        public V nodeValue { get; set; }
        public string name { get; set; }
        public List<CsEdge<V, E>> children { get; set; }
        public bool visited { get; set; }

        //----< construct a named node >---------------------------------------

        public int LowLink { get; set; }
        public int Index { get; set; }
       
       
        public CsNode(string nodeName)
        {
            name = nodeName;
            children = new List<CsEdge<V, E>>();
            visited = false;
            Index = -1;
            LowLink = 0;
        }
        //----< add child vertex and its associated edge value to vertex >-----

        public void addChild(CsNode<V, E> childNode, E edgeVal)
        {
            children.Add(new CsEdge<V, E>(childNode, edgeVal));
        }
        //----< find the next unvisited child >--------------------------------

        public CsEdge<V, E> getNextUnmarkedChild()
        {
            foreach (CsEdge<V, E> child in children)
            {
                if (!child.targetNode.visited)
                {
                    child.targetNode.visited = true;
                    return child;
                }
            }
            return null;
        }
        //----< has unvisited child? >-----------------------------------

        public bool hasUnmarkedChild()
        {
            foreach (CsEdge<V, E> child in children)
            {
                if (!child.targetNode.visited)
                {
                    return true;
                }
            }
            return false;
        }
        public void unmark()
        {
            visited = false;
        }
        public override string ToString()
        {
            return name;
        }
    }
    /////////////////////////////////////////////////////////////////////////
    // Operation<V,E> class

    public class Operation<V, E>
    {
        //----< graph.walk() calls this on every node >------------------------

        virtual public bool doNodeOp(CsNode<V, E> node)
        {
            Console.Write("\n  {0}", node.ToString());
            return true;
        }
        //----< graph calls this on every child visitation >-------------------

        virtual public bool doEdgeOp(E edgeVal)
        {
            Console.Write(" {0}", edgeVal.ToString());
            return true;
        }
    }
    
    /////////////////////////////////////////////////////////////////////////
    // CsGraph<V,E> class

    public class Graph<V, E>
    {
        public CsNode<V, E> startNode { get; set; }
        public string name { get; set; }
        public bool showBackTrack { get; set; } = false;

        private  List<CsNode<V, E>> adjList { get; set; }  // node adjacency list
        private Operation<V, E> gop = null;
        //----< construct a named graph >--------------------------------------

        public Graph(string graphName)
        {
            name = graphName;
            adjList = new List<CsNode<V, E>>();
            gop = new Operation<V, E>();
            startNode = null;
        }
        public void Tarjan()
        {
            var index = 0; // number of nodes
            var S = new Stack<CsNode<V,E>>();

            Action<CsNode<V,E>> StrongConnect = null;
            StrongConnect = (v) =>
            {
                // Set the depth index for v to the smallest unused index
                
                v.Index = index;
                v.LowLink = index;

                index++;
                S.Push(v);

                // Consider successors of v
                foreach (CsNode<V, E> ww in adjList)
                  if (ww.Index < 0)
                    {
                        // Successor w has not yet been visited; recurse on it
                        StrongConnect(ww);
                        v.LowLink = Math.Min(v.LowLink, ww.LowLink);
                    }
                    else if (S.Contains(ww))
                        // Successor w is in stack S and hence in the current SCC
                        v.LowLink = Math.Min(v.LowLink, ww.Index);

                    // If v is a root node, pop the stack and generate an SCC
                    if (v.LowLink == v.Index)
                    {
                        Console.Write("Strongly Connected Components: \n");
                        CsNode<V, E> w;
                        do
                        {
                            w = S.Pop();
                            Console.Write(" " + w.name + "\n ");
                            //if(w.children.Contains(var item in adjList))
                            foreach (var item in w.children)
                                Console.Write(item.targetNode + "\n ");
                            Console.WriteLine("\n");
                        } while (w != v);
                    }
                    Console.WriteLine();
                
            };

            foreach (CsNode<V, E> v in adjList)
                if (v.Index < 0)
                    StrongConnect(v);
        }
        //----< register an Operation with the graph >-------------------------

        public Operation<V, E> setOperation(Operation<V, E> newOp)
        {
            Operation<V, E> temp = gop;
            gop = newOp;
            return temp;
        }
        //----< add vertex to graph adjacency list >---------------------------

        public void addNode(CsNode<V, E> node)
        {
            adjList.Add(node);
        }
        //----< clear visitation marks to prepare for next walk >--------------

        public void clearMarks()
        {
            foreach (CsNode<V, E> node in adjList)
                node.unmark();
        }
        //----< depth first search from startNode >----------------------------

        public void walk()
        {
            if (adjList.Count == 0)
            {
                Console.Write("\n  no nodes in graph");
                return;
            }
            if (startNode == null)
            {
                Console.Write("\n  no starting node defined");
                return;
            }
            if (gop == null)
            {
                Console.Write("\n  no node or edge operation defined");
                return;
            }
            this.walk(startNode);
            foreach (CsNode<V, E> node in adjList)
                if (!node.visited)
                    walk(node);
            foreach (CsNode<V, E> node in adjList)
                node.unmark();
            return;
        }
        //----< depth first search from specific node >------------------------

        public void walk(CsNode<V, E> node)
        {
            // process this node

            gop.doNodeOp(node);
            node.visited = true;

            // visit children
            do
            {
                CsEdge<V, E> childEdge = node.getNextUnmarkedChild();
                if (childEdge == null)
                {
                    return;
                }
                else
                {
                    gop.doEdgeOp(childEdge.edgeValue);
                    walk(childEdge.targetNode);
                    if (node.hasUnmarkedChild() || showBackTrack)
                    {                         // popped back to predecessor node
                        gop.doNodeOp(node);     // more edges to visit so announce
                    }                         // location and next edge
                }
            } while (true);
        }
    }
    /////////////////////////////////////////////////////////////////////////
    // Test class

    class demoOperation : Operation<string, string>
    {
        override public bool doNodeOp(CsNode<string, string> node)
        {
            Console.Write("\n -- {0}", node.name);
            return true;
        }
    }
    class Test
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Testing CsGraph class");
            Console.Write("\n =======================");

            CsNode<string, string> node1 = new CsNode<string, string>("node1");
            CsNode<string, string> node2 = new CsNode<string, string>("node2");
            CsNode<string, string> node3 = new CsNode<string, string>("node3");
            CsNode<string, string> node4 = new CsNode<string, string>("node4");
            CsNode<string, string> node5 = new CsNode<string, string>("node5");
            var nodes = new List<CsNode<string, string>>();
            //var nodes = new List<Graph<string,string>>();
            for (int i = 0; i < 5; i++)
            {
                // Here you can give each person a custom name based on a number
                nodes.Add(new CsNode<string, string>("node" + i));
            }
           
            node1.addChild(node2, "edge12");
            node1.addChild(node3, "edge13");
            node2.addChild(node3, "edge23");
            node2.addChild(node4, "edge24");
            node3.addChild(node1, "edge31");
            node5.addChild(node1, "edge51");
            node5.addChild(node4, "edge54");

            Graph<string, string> graph = new Graph<string, string>("Fred");
            graph.addNode(node1);
            graph.addNode(node2);
            graph.addNode(node3);
            graph.addNode(node4);
            graph.addNode(node5);

            graph.startNode = node1;
            Console.Write("\n\n  starting walk at {0}", graph.startNode.name);
            Console.Write("\n  not showing backtracks");
            graph.walk();

            graph.startNode = node2;
            Console.Write("\n\n  starting walk at {0}", graph.startNode.name);
            graph.showBackTrack = true;
            Console.Write("\n  show backtracks");
            graph.setOperation(new demoOperation());
            graph.walk();

            Console.Write("\n\n");
            Console.ReadLine();
        }
    }
}
