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
      {'F', "Ff" },
      {'L', "L--F" }, //turn left
      {'R', "R++F" }, //turn right
      {'P', "SfP" },  //arrow
      {'W', "++F----FF++++F--W" }, //wave
      {'V', "--F++++FF----F++V" }, //wave
      {'Y', "X" },

      //{'X', "[+++LRY][---RLY][LRY][RLY][FY]" }
      //{'X', "[+++LRWY][---RLVY]" }

      {'X', "S[+++LRY][---RLY]FF[+FY][-FY]" }
      //{'X', "f[+FY]-FY" } // fork
      //{'X', "WX" } //wave
      //{'X', "[-f+fX][+f-fX]WX" } //volley
      //{'X', "f[+++LR[X]][---RL[X]]" }  //snakes
      //{'X', "F[+X]-X" },
      //{'[',"[F[XF]" },
      //{'-',"F−[[X]+X]+F[+FX]−X" }
    };

    /* removes X
     * 1snakes: [+++LRY][---RLY]
     * 2snakes: [+++LRY][---RLY][Y]
     * 3snakes: [+++LRY][---RLY][++++LRY][----RLY]
     * 4snakes: [+++LRY][---RLY][++++LRY][----RLY][Y]
    * 1 fork : [+FY][-FY]
    * 2 forks: [+FY][-FY][FY]
    * 3 forks: [+FY][-FY][++FY][--FY]
    * 4 forks: [+FY][-FY][++FY][--FY][FY]
    */

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
        foreach (char c in Str)
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
