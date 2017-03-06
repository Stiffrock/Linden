using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayers_Defense
{
  class LProjectile : Projectile
  {
    static Dictionary<char, PCommand> commands = new Dictionary<char, PCommand>()
    {
      {'F', new PCommand(0.4f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'-', new PCommand(0.0f, (p, gt)=> {
        p.rotation -= (float)Math.PI/7.2f;
      })},
      {'+', new PCommand(0.0f, (p, gt)=> {
        p.rotation += (float)Math.PI/7.2f;
      })},
      {'[', new PCommand(0.0f, (p, gt)=> {
        string axiom = p.BracketSubstring(p.cmdIndex);
        LProjectile newP = new LProjectile(p.world, p.tex, p.pos, axiom, 0);
        newP.rotation = p.rotation;
        p.world.AddGameObject(newP);
      })}
    };

    int cmdIndex;
    LSystem L;
    PCommand currentCommand;
    float currentCommandElapsedTime;

    public LProjectile(World world, Texture2D tex, Vector2 pos, string axiom, int generations) : base(world, tex, pos, Vector2.Zero, 50.0f, 1.0f, 1.0f)
    {
      L = new LSystem(axiom);
      L.Evolve(generations);
      currentCommandElapsedTime = 0.0f;
      cmdIndex = -1;
      GotoNextCommand();
    }
    void GotoNextCommand()
    {
      for (int i = cmdIndex + 1; i < L.Str.Length; i++)
      {
        if (commands.ContainsKey(L.Str[i]))
        {
          cmdIndex = i;
          currentCommand = commands[L.Str[i]];
          currentCommandElapsedTime = 0.0f;
          return;
        }
      }
      currentCommand = commands['F'];
      Die();
    }
    private string BracketSubstring(int bracketIndex)
    {
      Stack<object> brackets = new Stack<object>();
      for (int i = bracketIndex; i < L.Str.Length; i++)
      {
        if (L.Str[i] == '[')
          brackets.Push(new object());
        else if (L.Str[i] == ']')
        {
          brackets.Pop();
          if (brackets.Count == 0)
          {
            cmdIndex = i;
            return L.Str.Substring(bracketIndex + 1, i - (bracketIndex + 1));
          }
        }
      }
      throw new Exception("Invalid brackets");
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      currentCommand.Invoke(this, gt);
      currentCommandElapsedTime += (float)gt.ElapsedGameTime.TotalSeconds;
      if (currentCommandElapsedTime >= currentCommand.Duration)
      {
        GotoNextCommand();
      }
    }
  }
}

//class LProjectile : Projectile
//{
//  static Dictionary<char, PCommand> commands = new Dictionary<char, PCommand>()
//    {
//      {'F', new PCommand(0.1f, (p, gt)=> {
//        p.pos += p.Forward() * 500 * (float)gt.ElapsedGameTime.TotalSeconds;
//      })},
//      {'-', new PCommand(0.0f, (p, gt)=> {
//        p.rotation -= (float)Math.PI/2;
//      })},
//      {'+', new PCommand(0.0f, (p, gt)=> {
//        p.rotation += (float)Math.PI/2;
//      })},
//      {'[', new PCommand(0.0f, (p, gt)=> {


//      })}
//    };

//  Queue<PCommand> commandQueue;
//  LSystem L;
//  PCommand currentCommand;
//  float currentCommandElapsedTime;
//  public LProjectile(World world, Texture2D tex, Vector2 pos, string axiom, int generations) : base(world, tex, pos, Vector2.Zero, Vector2.Zero, 1.0f, 1.0f)
//  {
//    commandQueue = new Queue<PCommand>();
//    L = new LSystem(axiom);
//    L.Evolve(generations);
//    currentCommandElapsedTime = 0.0f;
//    ParseLSystem();
//    currentCommand = commandQueue.Dequeue();
//  }
//  private void ParseLSystem()
//  {
//    foreach (char c in L.Representation)
//    {
//      if (commands.ContainsKey(c))
//        commandQueue.Enqueue(commands[c]);
//    }
//  }
//  public override void Update(GameTime gt)
//  {
//    currentCommandElapsedTime += (float)gt.ElapsedGameTime.TotalSeconds;
//    if (currentCommandElapsedTime >= currentCommand.Duration && commandQueue.Count() != 0)
//    {
//      currentCommand = commandQueue.Dequeue();
//      currentCommandElapsedTime = 0;
//    }
//    currentCommand.Invoke(this, gt);
//  }
//}
