// Executive.cs - Demonstrate Prototype Code Analyzer                //
// ver 1.0                                                           //
// Language:    C#, 2017, .Net Framework 4.7.1                       //
// Platform:    Dell Precision T8900, Win10                          //
// Application: Demonstration for CSE681, Project #3, Fall 2018      //
// Author:      Saumya Sharma                                        //
///////////////////////////////////////////////////////////////////////
/*
 *   Operations:
 *  =============
 *   This is a test driver for Navigate.  It provides event handlers
 *   to react to Navigates file and directory events.
 * 
 */

using System;
using System.IO;
using FileUtilities;

namespace Application
{
  class Test
  {
    Test()
    {
    }
    void OnDir(string dir)
    {
      Console.Write("\n  ++ {0}",dir);
    }
    void OnFile(string file)
    {
      string name = Path.GetFileName(file);
      FileInfo fi = new FileInfo(file);
      DateTime dt = File.GetLastWriteTime(file);
      Console.Write("\n  --   {0,-35} {1,8} bytes  {2}",name,fi.Length,dt);
    }
    void Register(Navigate nav)
    {
      nav.newDir += new Navigate.newDirHandler(OnDir);
      nav.newFile += new Navigate.newFileHandler(OnFile);
    }
    [STAThread]
    static void Main(string[] args)
    {
      Console.Write("\n  Demonstrate Directory Navigation with Delegates ");
      Console.Write("\n =================================================");
      Console.Write("\n  Command Line: ");
      foreach(string arg in args)
      {
        Console.Write("{0} ", arg);
      }
      string path;
      if(args.Length > 0)
        path = args[0];
      else
        path = Directory.GetCurrentDirectory();

      Navigate nav = new Navigate();

      int i = 0;
      for(i = 1; i < args.Length; ++i)
      {
        nav.Add(args[i]);
      }

      Test test = new Test();
      test.Register(nav);
      nav.go(path);

      Console.Write("\n\n");
    }
  }
}
