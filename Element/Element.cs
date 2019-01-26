// Executive.cs - Demonstrate Prototype Code Analyzer                //
// ver 1.0                                                           //
// Language:    C#, 2017, .Net Framework 4.7.1                       //
// Platform:    Dell Precision T8900, Win10                          //
// Application: Demonstration for CSE681, Project #3, Fall 2018      //
// Author:      Saumya Sharma                                        //
///////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ------------------
 * This module defines the Elem class, which holds:
 *   - type: class, struct, enum
 *   - name
 *   - code location: start and end line numbers
 *   - size and complexity metrics: lines of code and scope count
 *  
 */
/* Required Files:
 *   Element.cs
 *   
 *
 * Note:
 * This package does not have a test stub as it contains only a data structure.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
  public class Elem  // holds scope information
  {
    public string type { get; set; }
    public string name { get; set; }
    public int beginLine { get; set; }
    public int endLine { get; set; }
    public int beginScopeCount { get; set; }
    public int endScopeCount { get; set; }

    public override string ToString()
    {
      StringBuilder temp = new StringBuilder();
      temp.Append("{");
      temp.Append(String.Format("{0,-10}", type)).Append(" : ");
      temp.Append(String.Format("{0,-10}", name)).Append(" : ");
      temp.Append(String.Format("{0,-5}", beginLine.ToString()));  // line of scope start
      temp.Append(String.Format("{0,-5}", endLine.ToString()));    // line of scope end
      temp.Append("}");
      return temp.ToString();
    }
  }
}

