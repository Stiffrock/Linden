using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lindenmayers_Defense
{
  class LSystem
  {
    static Dictionary<char, string> grammar = new Dictionary<char, string>()
    {
      {'F',"F" },
      {'X', "F[+X]-X" }
      //{'F',"F" },
      //{'X',"F−[[X]+X]+F[+FX]−X" }
    };

    public string Str { get; protected set; }

    public LSystem(string axiom)
    {
      Str = axiom;
    }
    public void Evolve(int generations)
    {
      for (int i = 0; i < generations; i++)
      {
        string result = "";
        foreach(char c in Str)
        {
          if (grammar.ContainsKey(c))
            result += grammar[c];
          else
            result += c;
        }
        Str = result;
      }
    }
  }
}
