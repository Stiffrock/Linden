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
        p.rotation -= (float)Math.PI/8f * p.mirrored;
      })},
      {'+', new PCommand(0.0f, (p, gt)=> {
        p.rotation += (float)Math.PI/8f * p.mirrored;
      })},
      {'K', new PCommand(0.0f, (p, gt)=> {
        string forks = "";
        forks += p.LStr[p.commandIndex + 1];
        int n = int.Parse(forks);
        p.Fork(n);
      })},
      {'Q', new PCommand(0.0f, (p, gt)=> {
        string forks = "";
        forks += p.LStr[p.commandIndex + 1];
        int n = int.Parse(forks);
        p.Volley(n);
      })},
      {'[', new PCommand(0.0f, (p, gt)=> {
        string lstr = p.BracketSubstring(p.commandIndex);
        p.Damage *= 0.7f;
        LProjectile newP = new LProjectile(p.world, p.Tower, p.tex, p.pos, lstr, p.Forward(), p.Speed, p.Damage);

        newP.Lifetime = p.Lifetime;// *Game1.rnd.NextDouble()*0.5f+0.5f;
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
          p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle04"), p.pos, 0.25f, 200.0f, 0.25f, p.color*0.7f);
      })},
      {'z', new PCommand(0.0f, (p, gt)=> {
        p.Speed *= 0.9f;
        for (int i = 0; i < 2; i++)
          p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle02"), p.pos, 0.25f, 200.0f, 0.25f, p.color*0.8f);
      })},
      {'h', new PCommand(0.0f, (p, gt)=> {
        if(p.Target == null)
          return;
        float targetAngle = Utility.Vector2ToAngle(p.Target.pos - p.pos);
        p.rotation += Utility.TurnAngle(p.rotation, targetAngle, p.TurnRate);
        p.world.ParticleManager.GenerateParticle(AssetManager.GetTexture("particle04"), p.pos, 0.25f, 50.0f, .7f, p.color*0.6f);
      })}
    };
    static Dictionary<char, char> bracketPairs = new Dictionary<char, char>()
    { {'[', ']' }, {'(', ')'} };

    protected int commandIndex;
    protected int mirrored;
    protected int generation;

    protected string LStr;
    protected PCommand currentCommand;
    protected float currentCommandElapsedTime;

    public LProjectile(World world, Tower owner, Texture2D tex, Vector2 pos, string LStr, Vector2 direction, float speed, float damage)
      : base(world, owner, tex, pos, direction, speed, damage)
    {
      this.LStr = LStr;
      currentCommandElapsedTime = 0.0f;
      commandIndex = -1;
      mirrored = 1;
      generation = 0;
      GotoNextCommand();
    }
    public LProjectile(LProjectile other)
      : base(other)
    {
      LStr = other.LStr;
      commandIndex = other.commandIndex;
      currentCommand = other.currentCommand;
      currentCommandElapsedTime = other.currentCommandElapsedTime;
      TurnRate = other.TurnRate;
      Target = other.Target;
      Scale = other.Scale;
      color = other.color;
      mirrored = other.mirrored;
      generation = other.generation;
    }
    protected void Fork(int n)
    {
      if (n <= 1 || generation >= 5)
        return;
      ++generation;
      float rotDiff = (float)Math.PI / 8f;
      Damage *= 0.7f;
      Scale *= 0.9f;
      rotation -= (n - 1) / 2f * rotDiff;
      for (int i = 0; i < n - 1; i++)
      {
        LProjectile newP = new LProjectile(this);
        newP.GotoNextCommand();
        world.AddProjectile(newP);
        rotation += rotDiff;
        if (i < n / 2)
          newP.mirrored = -1;
      }
    }
    protected void Volley(int n)
    {
      if (n <= 1 || generation >= 5)
        return;
      ++generation;
      float distBetween = 15;
      Damage *= 0.7f;
      Scale *= 0.9f;
      pos -= Right() * ((n - 1) / 2f * distBetween);
      for (int i = 0; i < n - 1; i++)
      {
        LProjectile newP = new LProjectile(this);
        newP.GotoNextCommand();
        world.AddProjectile(newP);
        pos += Right() * distBetween;
        if (i < n / 2)
          newP.mirrored = -mirrored;
      }
    }
    protected void GotoNextCommand()
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
    protected string BracketSubstring(int bracketIndex)
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
      if (commandIndex == LStr.Length - 1)
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