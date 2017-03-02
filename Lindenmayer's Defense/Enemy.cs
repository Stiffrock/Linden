using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Lindenmayers_Defense
{
  class Enemy : GameObject, IDamageable
  {
    World world;
    GameObject target;
    float damage;
    float health;
    float speed = 100;

    public Enemy(World world, Texture2D tex, Vector2 pos, float damage, float health, float speed) : base(tex, pos)
    {
      this.world = world;
      this.pos = pos;
      this.tex = tex;
      this.damage = damage;
      this.health = health;
      this.speed = speed;
      target = world.baseTower;
      layerMask = CollisionLayer.TOWER;
      drawHitbox = false;

    }
    protected virtual void Movement(GameTime gt)
    {
      Vector2 direction = target.pos - this.pos;
      direction.Normalize();
      this.pos += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      if (health <= 0)
        Die();
    }
    public override void DoCollision(GameObject other)
    {
      if(other == target && target is IDamageable)
      {
        ((IDamageable)target).TakeDamage(damage);
        Die();
      }
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);

      if (target != null && target != this)
      {
        Movement(gt);
      }
    }
  }
}
