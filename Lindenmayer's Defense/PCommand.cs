using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lindenmayers_Defense
{
  class PCommand
  {
    Action<LProjectile, GameTime> command;
    public float Duration { get; private set; }
    public PCommand(float duration, Action<LProjectile, GameTime> command)
    {
      this.Duration = duration;
      this.command = command;
    }
    public void Invoke(LProjectile p, GameTime gt)
    {
      command.Invoke(p, gt);
    }
  }
}
