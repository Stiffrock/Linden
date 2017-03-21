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
  /// TO DO
  /// Switch to arrays instead of lists
  /// 
  class UserInterface
  {
    private Container[,] inventoryArray;
    private Container[] componentArray;
    private Container towerStatContainer;
    private Container towerBox;
    private int[] stats;
    private List<int> statList;
    private List<Container> containerList, statContainerList;
    private List<LComponent> componentList;
    private List<string> statName;
    public List<LComponent> result;
    private TowerManager tm;
    private int statStringOffsetX, statStringOffsetY, statOffsetX, statOffsetY;
    private Dictionary<string, LComponent> grammarComponents;
    private World world;
    private Button statButton;
    private List<Button> statButtonList;


    public UserInterface(TowerManager tm, World world)
    {
      containerList = new List<Container>();
      statContainerList = new List<Container>();
      statButtonList = new List<Button>();
      statName = new List<string>();
      statStringOffsetX = 20;
      statOffsetX = 150;
      statStringOffsetY = statOffsetY = 30;
      inventoryArray = new Container[3, 6];
      componentArray = new Container[5];
      statList = new List<int>();
      stats = new int[6];
      this.tm = tm;
      grammarComponents = tm.GetGrammarComponents();
      //this.grammarComponenets = grammarComponenets;
      componentList = new List<LComponent>();
      result = new List<LComponent>();
      this.world = world;
      InitInventoryArray();
      InitComponentArray();
      InitTowerBox();
      InitComponent();
      SetStatNames();
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
    public Container GetTowerCreator()
    {
      return towerBox;
    }
    private void InitComponent()
    {
      int i = 0, j = 0;
      foreach (KeyValuePair<string, LComponent> entry in grammarComponents)
      {
        inventoryArray[j, i].component = entry.Value;
        inventoryArray[j, i].name = entry.Key;
        inventoryArray[j, i].component.pos = inventoryArray[j, i].pos;
        inventoryArray[j, i].component.rec = new Rectangle(100, 100, 20, 20);//inventoryArray[j, i].rec;
        componentList.Add(inventoryArray[j, i].component);

        ++i;
        if (i >= inventoryArray.GetLength(1))
        {
          i = 0;
          ++j;
        }
      }
    }

    private void InitTowerBox()
    {
      towerBox = new Container(AssetManager.GetTexture("pixel"), new Vector2(950, 655));
      towerBox.rec.Size = new Point(100, 100);
      containerList.Add(towerBox);
    }

    private void InitInventoryArray()
    {
      for (int i = 0; i < inventoryArray.GetLength(0); i++)
      {
        for (int j = 0; j < inventoryArray.GetLength(1); j++)
        {
          inventoryArray[i, j] = new Container(AssetManager.GetTexture("pixel"), new Vector2(100 + 55 * j, 600 + 55 * i));
          containerList.Add(inventoryArray[i, j]);
        }
      }
    }

    private void InitComponentArray()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        componentArray[i] = new Container(AssetManager.GetTexture("pixel"), new Vector2(550 + 55 * i, 705));
        componentArray[i].ComponentArray = true;
        containerList.Add(componentArray[i]);
      }
    }

    private void CheckforTowerClick()
    {
      List<GameObject> golist = world.GetGameObjects();
      foreach (GameObject t in golist)
      {
        if (t is Tower && MouseIntersect(t.hitbox))
        {
          Tower temp = (Tower)t;
          if (temp.displayStats)
          {
            RemoveStatWindow(t);
            temp.displayStats = false;
          }
          else
          {
            BuildStatWindow(t);
            temp.displayStats = true;
          }
        }
      }
    }

    private void StatButtonClick()
    {
      if (Input.LeftMouseButtonClicked())
      {
        foreach (Container c in statContainerList)
        {
          foreach (Button b in c.statButtonList)
          {
            if (MouseIntersect(b.GetHitbox()))
            {
              switch (b.statID)
              {
                case "damage":
                  {
                    c.tower.IncreaseLevel_Damage(1);
                    c.statList[0] = c.tower.damageLvl;
                    break;
                  }
                case "firerate":
                  {
                    c.tower.IncreaseLevel_Firerate(1);
                    c.statList[1] = c.tower.firerateLvl;
                    break;
                  }
                case "turnspeed":
                  {
                    c.tower.IncreaseLevel_TurnSpeed(1);
                    c.statList[2] = c.tower.turnspeedLvl;
                    break;
                  }
                case "speed":
                  {
                    c.tower.IncreaseLevel_Speed(1);
                    c.statList[3] = c.tower.speedLvl;
                    break;
                  }
                case "size":
                  {
                    c.tower.IncreaseLevel_Size(1);
                    c.statList[4] = c.tower.sizeLvl;
                    break;
                  }
                case "towerhealth":
                  {
                    c.tower.IncreaseLevel_Health(1);
                    c.statList[5] = c.tower.healthLvl;
                    break;
                  }
                case "generation":
                  {
                    c.tower.IncreaseLevel_Generations(1);
                    c.statList[6] = c.tower.generationLvl;
                    break;
                  }
                default:
                  break;
              }
            }
          }
        }     
      }
    }

    private void RemoveStatWindow(GameObject t)
    {
      Tower temp = (Tower)t;

      for (int i = 0; i < statContainerList.Count; i++)
      {
        if (statContainerList[i].tower == temp)
        {       
          statContainerList.Remove(statContainerList[i]);         
          break;
        }
      }
    }

    private void BuildStatWindow(GameObject t)
    {
      towerStatContainer = new Container(AssetManager.GetTexture("pixel"), t.pos);     
      towerStatContainer.rec.Width = 300;
      towerStatContainer.rec.Height = 200;
      Tower temp = (Tower)t;
      towerStatContainer.tower = temp;

      towerStatContainer.statList.Add(temp.damageLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y), "damage"));

      towerStatContainer.statList.Add(temp.firerateLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 30), "firerate"));

      towerStatContainer.statList.Add(temp.turnspeedLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 60), "turnspeed"));

      towerStatContainer.statList.Add(temp.speedLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 90), "speed"));

      towerStatContainer.statList.Add(temp.sizeLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 120), "size"));

      towerStatContainer.statList.Add(temp.healthLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 150), "health"));

      towerStatContainer.statList.Add(temp.generationLvl);
      towerStatContainer.statButtonList.Add(new Button(AssetManager.GetTexture("dot"), new Vector2(temp.pos.X + 250, temp.pos.Y + 180), "generation"));

      towerStatContainer.mouseOverEffect = false;
      statContainerList.Add(towerStatContainer);
    }
    public bool MouseIntersect(Rectangle uiObject)
    {
      if (uiObject.Contains(Input.GetMousePoint()))
      {
        return true;
      }
      return false;
    }

    private void HandleComponent(int i)
    {
      if (containerList[i].name == null)
        return;
      if (MouseIntersect(containerList[i].rec))
      {
        for (int j = 0; j < componentArray.GetLength(0); j++)
        {
          if (componentArray[j].name == null && !containerList[i].ComponentArray)
          {
            componentArray[j].component = componentList[i];
            componentArray[j].name = containerList[i].name;

            LComponent temp = new LComponent(componentList[i].tex, componentList[i].grammar);
            temp.pos = componentArray[j].pos;
            temp.rec = new Rectangle(100, 100, 20, 20);
            temp.chosen = true;
            result.Add(temp);
            componentList.Add(temp);
            break;
          }
        }
      }
    }

    private void CheckforRemoveComponent()
    {
      for (int i = 0; i < componentArray.GetLength(0); i++)
      {
        if (componentArray[i].name != null && MouseIntersect(componentArray[i].rec))
        {
          for (int j = 0; j < componentList.Count; j++)
          {
            if (componentList[j].pos == componentArray[i].pos)
            {
              componentArray[i].component = null;
              componentArray[i].name = null;
              componentList.Remove(componentList[j]);
            }
          }
        }
      }
    }

    public virtual void Update(GameTime gt)
    {
      for (int i = 0; i < statContainerList.Count; i++)
      {
        statContainerList[i].Update(gt);
      }
      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Update(gt);
        if (Input.LeftMouseButtonClicked())
          HandleComponent(i);
      }
      if (Input.LeftMouseButtonClicked())
      {
        CheckforRemoveComponent();
        CheckforTowerClick();
      }
      StatButtonClick();


    }

    public string GetResult()
    {
      string result = "";
      for (int i = 0; i < componentList.Count; i++)
      {
        if (componentList[i].chosen)
        {
          result += componentList[i].grammar;
        }
      }
      result += "X";
      return result;
    }

    public virtual void Draw(SpriteBatch sb)
    {
     
      for (int i = 0; i < statContainerList.Count; i++)
      {
        statContainerList[i].Draw(sb);
        for (int j = 0; j < statContainerList[i].statList.Count; j++)
        {
          sb.DrawString(AssetManager.GetFont("font1"), statName[j], new Vector2(statContainerList[i].pos.X + statStringOffsetX/*offset*/, statContainerList[i].pos.Y + statStringOffsetY * j), Color.Black);
          sb.DrawString(AssetManager.GetFont("font1"), statContainerList[i].statList[j].ToString(), new Vector2(statContainerList[i].pos.X + statOffsetX/*offset*/, statContainerList[i].pos.Y + statOffsetY * j), Color.Black);
        }
        for (int j = 0; j < statContainerList[i].statButtonList.Count; j++)
        {
          //sb.Draw(statContainerList[i].statButtonList[j].tex, new Rectangle((int)statContainerList[i].statButtonList[j].pos.X, (int)statContainerList[i].statButtonList[j].pos.Y, 20, 20), Color.Red);
          sb.Draw(statContainerList[i].statButtonList[j].tex, statContainerList[i].statButtonList[j].GetHitbox(), Color.Red);
        }
      }    

      for (int i = 0; i < containerList.Count; i++)
      {
        containerList[i].Draw(sb);
      }
  
      for (int i = 0; i < componentList.Count; i++)
      {
        componentList[i].Draw(sb);
      }
     
    }

  }
}
