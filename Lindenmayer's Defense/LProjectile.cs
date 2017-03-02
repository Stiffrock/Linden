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
      {'F', new PCommand(0.1f, (p, gt)=> {
        p.pos += p.Forward() * 500 * (float)gt.ElapsedGameTime.TotalSeconds;
      })},
      {'-', new PCommand(0.0f, (p, gt)=> {
        p.rotation -= (float)Math.PI/2;
      })},
      {'+', new PCommand(0.0f, (p, gt)=> {
        p.rotation += (float)Math.PI/2;
      })}
    };

    Queue<PCommand> commandQueue;
    LSystem L;
    PCommand currentCommand;
    float currentCommandElapsedTime;

    public LProjectile(World world, Texture2D tex, Vector2 pos) : base(world, tex, pos, Vector2.Zero, 0.0f, 1.0f, 1.0f)
    {
      commandQueue = new Queue<PCommand>();
      L = new LSystem("F");
      L.Evolve(5);
      currentCommandElapsedTime = 0.0f;
      ParseLSystem();
      currentCommand = commandQueue.Dequeue();
    }
    private void ParseLSystem()
    {
      foreach (char c in L.Representation)
      {
        if (commands.ContainsKey(c))
          commandQueue.Enqueue(commands[c]);
      }
    }
    public override void Update(GameTime gt)
    {
      currentCommandElapsedTime += (float)gt.ElapsedGameTime.TotalSeconds;
      if (currentCommandElapsedTime >= currentCommand.Duration && commandQueue.Count() != 0)
      {
        currentCommand = commandQueue.Dequeue();
        currentCommandElapsedTime = 0;
      }
      currentCommand.Invoke(this, gt);
    }
  }
}
