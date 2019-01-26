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
 *   DependencyAnalysis:
 *   - performs dependency analysis and outputs files dependent on each other
 */
/* 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyAnalysis
{
    using File = String;
    using Type = String;
    public struct TypeItem
    {
        public File file;
        //   public Namespace namesp;
    }
    public class DepAnalysis
    {
        public Dictionary<File, List<TypeItem>> table { get; set; } =
          new Dictionary<File, List<TypeItem>>();

        public void add(Type type, TypeItem ti)
        {
            if (table.ContainsKey(type))
                table[type].Add(ti);
            else
            {
                List<TypeItem> temp = new List<TypeItem>();
                temp.Add(ti);
                table.Add(type, temp);
            }
        }
        public void add(Type type, File file)
        {
            TypeItem temp;
            temp.file = file;
            // temp.namesp = ns;
            add(type, temp);
        }
        public void show()
        {
            foreach (var elem in table)
            {
                Console.Write("\n  {0} depends on", elem.Key);
                foreach (var item in elem.Value)
                {
                    Console.Write("\n    {0}.cs", item.file);
                }
            }
            Console.Write("\n");
        }
    }
    class DepAnalysisDemo
    {
        
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Dependency Analysis");
            Console.Write("\n ====================================");
            // build demo table

            DepAnalysis tt = new DepAnalysis();
            //tt.add("Type_X", "File_A");
            //tt.add("Type_X", "File_B");
            //tt.add("Type_Y", "File_A");
            //tt.add("Type_Z", "File_C");

            tt.show();

            // access elements in table

            Console.Write("\n  TypeTable contains types: ");
            foreach (var elem in tt.table)
            {
                Console.Write("{0} ", elem.Key);
            }
            Console.Write("\n\n");
            Console.ReadLine();
        }
    }
}
