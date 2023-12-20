using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public class DataPlayer : IDataPersistence
{
        [SerializeField] int level;
   // [SerializeField] int exp;
        public int Level
    {
        get { return level; }
    }
  

        public int Exp { get; set; }

        public DataPlayer(int pLevel)
        {
           level = pLevel;
           Init();
        //    Exp = pExp;//

       //     Exp = GetEx//pForLevel(Level);
        }

        public void Init()
        {
            Exp = GetExpForLevel(Level);
        }


        public int GetExpForLevel(int level)
        {
            return level * level * level;
        }

        public bool CheckForLevelUp()
        {
            if (Exp > GetExpForLevel(level + 1))
            {
                ++level;
            return true;
            }
            return false;
        }

    public DataPlayer(SaveDataPlayer saveData)
    {
        level = saveData.level;
        Exp = saveData.exp;

    }
    public SaveDataPlayer GetSaveData()
    {
        var saveData = new SaveDataPlayer()
        {
            level = Level,
            exp = Exp
        };
        return saveData;
       
    }

    public object CaptureState()
    {
        var saveData = new SaveDataPlayer()
        {
          //  position = new float[] { transform.position.x, transform.position.y },
          level = level,
          exp = Exp
            //  dataPlayer = new data
            //  dataPlayer 
            //  dataPlayer = GetSa
            //   dataPlayers = GetComponent<PlayerParts>().dataPlayers.Select(p => p.GetSaveData()).ToList()
            //  dataPlayers = GetComponent<PlayerParts>().DataPlayers.Select(p => GetSave)
            //   dataPlayers  = GetComponent<PlayerParts>().DataPlayers.Select(p => p.GetSaveData()).ToList()


        };

        return saveData;
    }

    public void RestoreState(object state)
    {
  
    }

    public void LoadData(GameData data)
    {
       // throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
     //   data.level = this.level;
       // data.exp = this.Exp;
    }
}


/*  public bool CheckForLevelUp()
  {
      if (exp > GetExpForLevel(level + 1))
      {
          ++level;
          return true;
      }
      else
      {
          return false;
      }
  }*/
[System.Serializable]
public class SaveDataPlayer
{
        public int level;
        public int exp;
}
    

