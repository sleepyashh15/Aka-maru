
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class Player
{

    //  public PlayerBase Base { get; set; }
    [SerializeField] PlayerBase _base;
    [SerializeField] int level;
    [SerializeField] float exp;
    [SerializeField] int score;
    [SerializeField] string saveCount;
    [SerializeField] string username;
    public string Username
    {
        get { return username; }
        set { username = value; }
    }

    public int quizCount;
    public int stageCount;
    public PlayerBase Base
    {
        get
        {
            return _base;
        }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public int Score
    {
        get { return score; }
        set { score = value; }
    }
    public string SaveCount
    {
        get { return saveCount; }
        set { saveCount = value; }
    }

    // public int Exp { get; set; }
    public float Exp
    {
        get { return exp; }
        set { exp = value; }
    }


    /*
        public Player(PlayerBase pBase, int pLevel)
        {
            _base = pBase;
            level = pLevel;
            Init();

        }*/

    public void Init()
    {
        Exp = Base.GetExpForLevel(Level);
    }

    public bool CheckForLevelUp()
    {
        if (Exp > Base.GetExpForLevel(level + 1))
        {
            ++level;
            return true;
        }
        return false;
    }
    public int QuizCleared()
    {
        ++quizCount;

        return quizCount;
       // return true;
    }

    public int StageCleared()
    {
        ++stageCount;

        return stageCount;
        // return true;
    }
    public int ScoreCount()
    {
        ++score;

        return score;
        // return true;
    }


    public Player(GameData saveData)
    {
        _base =  GameData.GetPlayerByName(saveData.name);
        Level = saveData.level;
        Exp = saveData.exp;
        quizCount = saveData.quizCount;
        score = saveData.score;
       // saveCount = saveData.saveCount;
        Username = saveData.username;
        stageCount = saveData.stageCount;
    }

    public GameData GetSaveData()
    {
        var saveData = new GameData()
        {
            name = Base.Name,
            level = Level,
            exp = exp,
            quizCount = quizCount,
            score = score,
          //  saveCount = saveCount,
            username = Username,
            stageCount = stageCount
        };
        return saveData;
    }

}
/*
[System.Serializable]
public class PlayersSaveData
{
    public string name;
    public int level;
    public float exp;


}*/

  

