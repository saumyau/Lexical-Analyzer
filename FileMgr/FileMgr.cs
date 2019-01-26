// Executive.cs - Demonstrate Prototype Code Analyzer                //
// ver 1.0                                                           //
// Language:    C#, 2017, .Net Framework 4.7.1                       //
// Platform:    Dell Precision T8900, Win10                          //
// Application: Demonstration for CSE681, Project #3, Fall 2018      //
// Author:      Saumya Sharma                                        //
///////////////////////////////////////////////////////////////////////
/*
 *  Module Operations:
 *  ==================
 *  Recursively displays the contents of a directory tree
 *  rooted at a specified path, with specified file pattern.
 *
 *  This version uses delegates to avoid embedding application
 *  details in the Navigator.  Navigate now is reusable.  Clients
 *  simply register event handlers for Navigate events newDir 
 *  and newFile.
 * 
 *  Public Interface:
 *  =================
 *  Navigate nav = new Navigate();
 *  nav.go("c:\temp","*.cs");
 *  nav.newDir += new Navigate.newDirHandler(OnDir);
 *  nav.newFile += new Navigate.newFileHandler(OnFile);
 * 
 */
//
using System;
using System.IO;
using System.Collections.Generic;

namespace FileUtilities
{
  ///////////////////////////////////////////////////////////////////
  // Navigate class
  // - uses public event properties to avoid binding directly
  //   to application processing

  public class Navigate
  { 
    // define public delegate type
    public delegate void newDirHandler(string dir);

    // define public delegate property with private backing store
    event newDirHandler newDir_;  // private backing store
    
    public newDirHandler newDir   // public delegate property
    {
      get { return newDir_; }     // getter's logic can be changed without
                                  // changing user interface
      set { newDir_ = value; }    // same for setter
    }

    public delegate void newFileHandler(string file);

    event newFileHandler newFile_;

    public newFileHandler newFile
    {
      get { return newFile_; }
      set { newFile_ = value; }
    }

    private List<string> patterns_ = new List<string>();

    public Navigate()
    {
      patterns_.Add("*.*");
    }

    public void Add(string pattern)
    {
      if(patterns_.Count == 1 && patterns_[0] == "*.*")
      {
        patterns_.Clear();
      }
      patterns_.Add(pattern);
    }
    ///////////////////////
    // The go function has no application specific code.
    // It just invokes its event delegate to notify clients.
    // The clients take care of all the application specific stuff.
    
    public void go(string path, bool recurse=true)
    {
      path = Path.GetFullPath(path);

      List<string> allFiles = new List<string>();

      foreach(string patt in patterns_)
      {
        string[] files = Directory.GetFiles(path, patt);
        allFiles.AddRange(files);
      }
      if (newDir != null && allFiles.Count > 0)
        newDir.Invoke(path);

      foreach (string file in allFiles)
      {
        if (newFile != null)
          newFile.Invoke(Path.GetFileName(file));
      }

      if (recurse)
      {
        string[] dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
          go(dir);
      }
    }
  }
}
