using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Lindenmayers_Defense.GUI
{
  class StatContainer : Container
  {
    public Tower tower;
    public List<int> statList;
    private List<string> statName;
    private int statStringOffsetX, statStringOffsetY, statOffsetX, statOffsetY;
    private int[] stats;
    public List<Button> statButtonList;

    public StatContainer(Texture2D tex, Vector2 pos, Tower tower) : base(tex, pos)
    {
      this.tower = tower;
      statList = new List<int>();
      rec.Width = 300;
      rec.Height = 200;
      mouseOverEffect = false;
      statName = new List<string>();
      statStringOffsetX = 20;
      statOffsetX = 150;
      statStringOffsetY = statOffsetY = 30;
      statButtonList = new List<Button>();
      statList = new List<int>();
      stats = new int[6];
      SetStatNames();
      BuildStatBox();
    }
    private void SetStatNames()
    {
      statName.Add("Damage");
      statName.Add("Fire rate");
      statName.Add("Turn Speed");
      statName.Add("Speed");
      statName.Add("Size");
      statName.Add("Tower Health");
      statName.Add("Generations");
    }

    private void BuildStatBox()
    {
      statList.Add(tower.damageLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y), "damage"));

      statList.Add(tower.firerateLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 30), "firerate"));

      statList.Add(tower.turnspeedLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 60), "turnspeed"));

      statList.Add(tower.speedLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 90), "speed"));

      statList.Add(tower.sizeLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 120), "size"));

      statList.Add(tower.healthLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 150), "health"));

      statList.Add(tower.generationLvl);
      statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(tower.pos.X + 250, tower.pos.Y + 180), "generation"));
    }
    public bool MouseIntersect(Rectangle uiObject)
    {
      if (uiObject.Contains(Input.GetMousePoint()))
      {
        return true;
      }
      return false;
    }

    public void StatButtonClick()
    {
      if (Input.LeftMouseButtonClicked())
      {    
        foreach (Button b in statButtonList)
        {
          if (MouseIntersect(b.GetHitbox()))
          {
            switch (b.statID)
            {
              case "damage":
                {
                  tower.IncreaseLevel_Damage(1);
                  statList[0] = tower.damageLvl;
                  break;
                }
              case "firerate":
                {
                  tower.IncreaseLevel_Firerate(1);
                  statList[1] = tower.firerateLvl;
                  break;
                }
              case "turnspeed":
                {
                  tower.IncreaseLevel_TurnSpeed(1);
                  statList[2] = tower.turnspeedLvl;
                  break;
                }
              case "speed":
                {
                  tower.IncreaseLevel_Speed(1);
                  statList[3] = tower.speedLvl;
                  break;
                }
              case "size":
                {
                  tower.IncreaseLevel_Size(1);
                  statList[4] = tower.sizeLvl;
                  break;
                }
              case "towerhealth":
                {
                  tower.IncreaseLevel_Health(1);
                  statList[5] = tower.healthLvl;
                  break;
                }
              case "generation":
                {
                  tower.IncreaseLevel_Generations(1);
                  statList[6] = tower.generationLvl;
                  break;
                }
              default:
                break;
            }
          }
        }        
      }
    }

    public override void Update(GameTime gt)
    {
      base.Update(gt);
      StatButtonClick();
    }
    public override void Draw(SpriteBatch sb)
    {
      base.Draw(sb);


      for (int j = 0; j < statList.Count; j++)
      {
        sb.DrawString(AssetManager.GetFont("font1"), statName[j], new Vector2(pos.X + statStringOffsetX/*offset*/,  pos.Y + statStringOffsetY * j), Color.Black);
        sb.DrawString(AssetManager.GetFont("font1"), statList[j].ToString(), new Vector2(pos.X + statOffsetX/*offset*/, pos.Y + statOffsetY * j), Color.Black);
      }
      for (int j = 0; j < statButtonList.Count; j++)
      {
        sb.Draw(statButtonList[j].tex, statButtonList[j].GetHitbox(), Color.Red);
      }
    }
    
  }
}
