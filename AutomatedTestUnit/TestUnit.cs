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
 *   Executive:
 *   - uses Parser, RulesAndActions, Semi, and Toker to perform basic
 *     code metric analyzes
 */
/* Required Files:
 *   Executive.cs
 *   Parser.cs
 *   IRulesAndActions.cs, RulesAndActions.cs, ScopeStack.cs, Elements.cs
 *   ITokenCollection.cs, Semi.cs, Toker.cs
 *   Display.cs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeTable;
using DependencyAnalysis;
using StrongComponent;
using System.IO;

namespace AutomatedTestUnit
{
    using CodeAnalysis;
    using Lexer;
    using File = String;
    using Type = String;
   
    class TestUnit
    {
        static List<string> ProcessCommandline(string[] args)
        {
            List<string> files = new List<string>();
            if (args.Length < 2)
            {
                Console.Write("\n  Please enter path and file(s) to analyze\n\n");
                return files;
            }
            string path = args[0];
            if (!Directory.Exists(path))
            {
                Console.Write("\n  invalid path \"{0}\"", System.IO.Path.GetFullPath(path));
                return files;
            }
            path = Path.GetFullPath(path);
            for (int i = 1; i < args.Length; ++i)
            {
                string filename = Path.GetFileName(args[i]);
                files.AddRange(Directory.GetFiles(path, filename));
            }
            return files;
        }

        static void ShowCommandLine(string[] args)
        {
            Console.Write("\n  Commandline args are:\n  ");
            foreach (string arg in args)
            {
                Console.Write("  {0}", arg);
            }
            Console.Write("\n  current directory: {0}", System.IO.Directory.GetCurrentDirectory());
            Console.Write("\n");
        }
        static List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return files;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Path containing all the test files : TestFiles\\.."); 
            Console.WriteLine("Enter the directory of test files");
            string file_path = Console.ReadLine();
            List<String> files = new List<String>();
            files = DirSearch(file_path);
            Console.Write("This project contains 13 packages demonstrating requirement 3:\n");
            Console.Write("1) DemoExecutive\n");
            Console.Write("2) DemoReqs\n");
            Console.Write("3) DependencyAnalysis\n");
            Console.Write("4) Display\n");
            Console.Write("5) Element\n");
            Console.Write("6) FileMgr\n");
            Console.Write("7) Parser\n");
            Console.Write("8) SemiExp\n");
            Console.Write("9) StrongComponent\n");
            Console.Write("10) TestHarness\n");
            Console.Write("11) Toker\n");
            Console.Write("12) TypeTable\n");
            Console.Write("13) AutomatedTestUnit\n");
            Console.Write("\n ======================\n");
            Console.Write("\n  Demonstrating Parser");
            Console.Write("\n ======================\n");

            //ShowCommandLine(args);

            //List<string> files = ProcessCommandline(args);
            DepAnalysis da = new DepAnalysis();
            foreach (string file in files)
            {
                Console.Write("\n  Processing file {0}\n", System.IO.Path.GetFileName(file));

                ITokenCollection semi = Factory.create();
                //semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", args[0]);
                    return;
                }

                Console.Write("\n  Type and Function Analysis");
                Console.Write("\n ----------------------------");

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();

                try
                {
                    while (semi.get().Count > 0)
                        parser.parse(semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> table = rep.locations;
                TypeTable.TypeTable tt = new TypeTable.TypeTable();
                ///shows typetable
                foreach (Elem e in table)
                {
                    tt.add(e.type, e.name);
                    if (e.type == "using")
                        da.add(System.IO.Path.GetFileName(file), e.name);
                }
                tt.show();
                //Console.Write("\n ----------------------------");
                Console.WriteLine("\nShowing TypeTables of files demonstrating requirement 5:\n");
                //Console.Write("\n ----------------------------");
                Console.Write("\n  TypeTable contains types: ");
                foreach (var elem in tt.table)
                {
                    Console.Write("{0} ", elem.Key);
                }
                ////Display.showMetricsTable(table);

                Console.Write("\n");
                semi.close();
            }
            var nodes = new List<CsNode<string, string>>();
            foreach (var elem in da.table)
            {
                nodes.Add(new CsNode<string, string>(elem.Key));
                //Console.Write("\n  {0} depends on", elem.Key);
            }
            nodes.ToArray();

            //foreach (var elem in da.table)
            //{
            //    foreach (var item in elem.Value)
            //    {
            //        //nodes[IndexOf(elem.Key)].addChild(nodes[IndexOf(item.file)], "edge" + elem.Key + " " + item.file);
            //        //nodes[0].addChild(item.file, "edge" + elem.Key + " " + item.file);
            //        //nodes[nodes.IndexOf(elem.Value)].addChild(nodes[nodes.IndexOf(item.file)], "edge" + elem.Key + " " + item.file);

            //        Console.Write("\n    {0}.cs", item.file);
            //    }
            //}
            //iterate through list of list elements of a file
            //da.table --gets table, 
            for (int i = 0; i < da.table.Count; i++)
            {
                var item = da.table.ElementAt(i);
                for (int j = 0; j < item.Value.Count; j++)
                {
                    //Console.WriteLine(i+"^"+ da.table.Values);
                    nodes[i].addChild(nodes[j], "edge" + i + j);
                }
            }
            Graph<string, string> graph = new Graph<string, string>("Fred");
            for (int i = 0; i < da.table.Count; i++)
            {
                graph.addNode(nodes[i]);
            }

            graph.startNode = nodes[0];
            //Console.WriteLine("\n Showing graph walk starting at node[0]\n");
            //Console.Write("\n\n  starting walk at {0}", graph.startNode.name);
            //Console.Write("\n  not showing backtracks");
            //graph.walk();
            Console.Write("\n ----------------------------");
            Console.WriteLine("\nShowing dependencies between files demonstrating requrement 4:\n\n");
            Console.Write("\n ----------------------------");
            //shows dependency analysis
            da.show();
            Console.Write("\n\n");
            Console.Write("\n ----------------------------");
            Console.Write("\nShowing all strong components, if any, in the file collection, based on the dependency analysis demonstrating requirement 6:\n\n");
            Console.Write("\n ----------------------------");
            graph.startNode = nodes[0];
            graph.Tarjan();
            Console.Write("\n\n");
            Console.Write("\n ----------------------------");
            Console.WriteLine("\nThis demo executive file demonstrates all the outputs and is well formatted as per requirement 7 and 8\n");
            Console.Write("\n ----------------------------");
            Console.ReadLine();
        }
    }
}
