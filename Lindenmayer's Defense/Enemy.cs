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
    Texture2D pixel;
    World world;
    GameObject target;
    float damage;
    ValueBar healthBar;
    float health;
    public float speed = 100;
    int goldGain;

    public Enemy(World world, Texture2D tex, Vector2 pos, float damage, float health, float speed) : base(tex, pos)
    {
      this.world = world;
      this.pos = pos;
      this.tex = tex;
      this.damage = damage;
      this.health = health;
      this.speed = speed;
      target = world.baseTower;
      healthBar = new ValueBar(new Rectangle((int)pos.X, (int)pos.Y, 30, 5), health, health, Color.Green * 0.7f, Color.Red * 0.5f);
      pixel = AssetManager.GetTexture("pixel");
      Layer = CollisionLayer.ENEMY;
      LayerMask = CollisionLayer.TOWER;
      goldGain = 5;
      Scale = 2.0f;
    }
    public override void Update(GameTime gt)
    {
      base.Update(gt);
      healthBar.SetPos(pos + new Vector2(0, 20));
      if (target != null && target != this)
      {
        Movement(gt);
      }
    }
    protected virtual void Movement(GameTime gt)
    {
      Vector2 direction = target.pos - this.pos;
      direction.Normalize();
      rotation = Utility.Vector2ToAngle(direction);
      this.pos += direction * speed * (float)gt.ElapsedGameTime.TotalSeconds;
    }
    public void TakeDamage(float damage)
    {
      health -= damage;
      healthBar.Value = health;
      if (health <= 0)
      {
        world.ResourceManager.AddGold(goldGain);
        Die();
      }
    }
    public override void DoCollision(GameObject other)
    {
      if (other == target && target is Base)
      {
        ((Base)target).TakeDamage(damage);
        Die();
      }
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);
      healthBar.Draw(sb);
    }
  }
}
