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
      {'S', "sfS" },  //arrow
      {'Z', "zfZ" },  //slow
      {'W', "++F----FF++++F--W" }, //wave
      {'Y', "X" },      //sub-branch
      {'E', "(f)FE" }, //explosive
      {'H', "hfH" }, //homing
      {'0', "1" },
      {'1', "2" },
      {'2', "3" },
      {'3', "4" },
      {'4', "5" },

      //{'X', "S[+FY][-FY][++FY][--FY][FY]" }
      //{'X', "S[+++LRY][---RLY]FF[+FY][-FY]" } //snake + fork
      //{'X', "f[+FY]-FY" } // fork
      //{'X', "WX" } //wave
      //{'X', "[-f+fX][+f-fX]WX" } //volley
      //{'X', "f[+++LR[X]][---RL[X]]" }  //snakes
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
    public string XRule { get; protected set; }
    public double MutationRate { get; set; }
    public int Generations { get; protected set; }

    public LSystem(string axiom, string xRule = "", double mutationRate = 0.05)
    {
      Str = axiom;
      XRule = xRule;
      MutationRate = mutationRate;
      Generations = 0;
    }
    public void Evolve(int generations)
    {
      for (int i = 0; i < generations; i++)
      {
        string result = "";
        foreach (char c in Str)
        {
          if (c == 'X')
            result += XRule;
          else if (grammar.ContainsKey(c))
            result += grammar[c];
          else
            result += c;
          if (Game1.rnd.NextDouble() < MutationRate && c != 'K' && c != 'Q')
          {
            IEnumerable<char> l = LProjectile.commands.Keys;
            int j = Game1.rnd.Next(0, l.Count());
            char ch = l.ElementAt(j);
            if (ch == 'K' || ch == 'Q')
              result += ch + '0';
            else if (ch == '(')
              result += "(f)";
            else if (ch != '[')
              result += ch;
          }
        }
        Str = result;
      }
      Generations += generations;
    }
  }
}
