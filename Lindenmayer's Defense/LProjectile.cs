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
    public static Dictionary<char, PCommand> commands = new Dictionary<char, PCommand>()
    {
      {'Y', new PCommand(0.5f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'X', new PCommand(0.5f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'f', new PCommand(0.05f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'F', new PCommand(0.1f, (p, gt)=> {
        p.pos += p.Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'-', new PCommand(0.0f, (p, gt)=> {
        p.rotation -= (float)Math.PI/8f;
      })},
      {'+', new PCommand(0.0f, (p, gt)=> {
        p.rotation += (float)Math.PI/8f;
      })},
      {'[', new PCommand(0.0f, (p, gt)=> {
        string lstr = p.BracketSubstring(p.commandIndex);
        LProjectile newP = new LProjectile(p.world, p.Tower, p.tex, p.pos, lstr, p.Forward(), p.Speed, p.Damage/2);
        p.Damage *= 0.5f;
        newP.Lifetime = p.Lifetime*Game1.rnd.NextDouble()*0.5f+0.5f;
        newP.Target = p.Target;
        newP.TurnRate = p.TurnRate;
        newP.Scale = p.Scale * 0.9f;
        newP.color = p.color;
        p.world.AddProjectile(newP);
      })},
      {'(', new PCommand(0.0f, (p, gt)=> {
        string lstr = p.BracketSubstring(p.commandIndex);
        LProjectile newP = new LProjectile(p.world, p.Tower, p.tex, p.pos, lstr, p.Forward(), p.Speed, p.Damage);
        newP.ExplosionRadius = 100.0f;
        newP.TurnRate = p.TurnRate;
        newP.Target = p.Target;
        newP.Scale = p.Scale * 0.9f;
        newP.color = p.color;
        p.world.AddProjectile(newP);
      })},
      {'s', new PCommand(0.0f, (p, gt)=> {
        p.Speed *= 1.1f;
        for (int i = 0; i < 2; i++)
          p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle04"), p.pos, 0.25f, 200.0f, 0.5f, p.color);
      })},
      {'z', new PCommand(0.0f, (p, gt)=> {
        p.Speed *= 0.9f;
        for (int i = 0; i < 2; i++)
          p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle02"), p.pos, 0.35f, 200.0f, 0.5f, p.color);
      })},
      {'h', new PCommand(0.0f, (p, gt)=> {
        float targetAngle = Utility.Vector2ToAngle(p.Target.pos - p.pos);
        p.rotation += Utility.TurnAngle(p.rotation, targetAngle, p.TurnRate);
        p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle04"), p.pos, 0.25f, 50.0f, 1.0f, p.color);
      })}
    };
    static Dictionary<char, char> bracketPairs = new Dictionary<char, char>()
    { {'[', ']' }, {'(', ')'} };

    int commandIndex;

    string LStr;
    PCommand currentCommand;
    float currentCommandElapsedTime;

    public LProjectile(World world, Tower owner, Texture2D tex, Vector2 pos, string LStr, Vector2 direction, float speed, float damage)
      : base(world, owner, tex, pos, direction, speed, damage)
    {
      this.LStr = LStr;
      currentCommandElapsedTime = 0.0f;
      commandIndex = -1;
      GotoNextCommand();
    }

    private void GotoNextCommand()
    {
      for (int i = commandIndex + 1; i < LStr.Length; i++)
      {
        if (commands.ContainsKey(LStr[i]))
        {
          commandIndex = i;
          currentCommand = commands[LStr[i]];
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
      for (int i = bracketIndex; i < LStr.Length; i++)
      {
        if (bracketPairs.ContainsKey(LStr[i]))
          brackets.Push(LStr[i]);
        else if (LStr[i] == bracketPairs[brackets.Peek()])
        {
          brackets.Pop();
          if (brackets.Count == 0)
          {
            commandIndex = i;
            return LStr.Substring(bracketIndex + 1, i - (bracketIndex + 1));
          }
        }
      }
      throw new Exception("Invalid brackets");
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      currentCommand.Invoke(this, gt);
      if(commandIndex == LStr.Length - 1)
        alpha = 1 - currentCommandElapsedTime / currentCommand.Duration;
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