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
      {'Y', new PCommand(0.6f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.alpha = 1 - (p.currentCommandElapsedTime/p.currentCommand.Duration);
      })},
      {'X', new PCommand(0.6f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        p.alpha = 1 - (p.currentCommandElapsedTime/p.currentCommand.Duration);
      })},
      {'f', new PCommand(0.05f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'F', new PCommand(0.2f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'-', new PCommand(0.0f, (p, gt)=> {
        p.rotation -= (float)Math.PI/8f;
      })},
      {'+', new PCommand(0.0f, (p, gt)=> {
        p.rotation += (float)Math.PI/8f;
      })},
      {'[', new PCommand(0.0f, (p, gt)=> {
        string axiom = p.BracketSubstring(p.commandIndex);
        LProjectile newP = new LProjectile(p.world, p.owner, p.tex, p.pos, axiom, p.L.XRule, 0, p.Forward(), p.Speed, p.Damage/2, p.Accuracy, p.bIsTargetSeeking);
        newP.color = p.color;
        p.world.AddProjectile(newP);
      })},
      {'(', new PCommand(0.0f, (p, gt)=> {
        string axiom = p.BracketSubstring(p.commandIndex);
        LExpProjectile newP = new LExpProjectile(p.world, p.owner, p.tex, p.pos, axiom, p.L.XRule, 0, p.Forward(), p.Speed, p.Damage, p.Accuracy, p.bIsTargetSeeking);
        newP.color = p.color;
        p.world.AddProjectile(newP);
      })},
      {'S', new PCommand(0.0f, (p, gt)=> {
        p.Speed *= 1.1f;
        p.world.ParticleManager.CreateExplosion(p.pos, 52, Color.White, 0.1f);
      })}
    };
    static Dictionary<char, char> bracketPairs = new Dictionary<char, char>()
    { {'[', ']' }, {'(', ')'} };

    int commandIndex;
    
    LSystem L;
    PCommand currentCommand;
    float currentCommandElapsedTime;
    

    public LProjectile(World world, Tower owner, Texture2D tex, Vector2 pos, string axiom, string xRule, int generations, Vector2 direction, float speed, float damage, float accuracy = 100, bool targetSeeking = false)
      : base(world, owner, tex, pos, direction, speed, damage, accuracy, targetSeeking)
    {
      L = new LSystem(axiom, xRule);
      L.Evolve(generations);
      currentCommandElapsedTime = 0.0f;
      commandIndex = -1;
      CalculateStats();
      GotoNextCommand();
    }

    private void CalculateStats()
    {
      foreach (char c in L.Str)
      {
        if (c == 'S')
        {
          damage += 0.1f;
        }
      }
    }
    private void GotoNextCommand()
    {
      for (int i = commandIndex + 1; i < L.Str.Length; i++)
      {
        if (commands.ContainsKey(L.Str[i]))
        {
          commandIndex = i;
          currentCommand = commands[L.Str[i]];
          currentCommandElapsedTime = 0.0f;
          return;
        }
      }
      currentCommandElapsedTime = 0.0f;
      currentCommand = commands['F'];
      Die();
    }
    private string BracketSubstring(int bracketIndex)
    {
      Stack<char> brackets = new Stack<char>();
      for (int i = bracketIndex; i < L.Str.Length; i++)
      {
        if (bracketPairs.ContainsKey(L.Str[i]))
          brackets.Push(L.Str[i]);
        else if (L.Str[i] == bracketPairs[brackets.Peek()])
        {
          brackets.Pop();
          if (brackets.Count == 0)
          {
            commandIndex = i;
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
      while (currentCommand.Duration < 0.0001f)
      {
        currentCommand.Invoke(this, gt);
        GotoNextCommand();
      }
    }
  }
}