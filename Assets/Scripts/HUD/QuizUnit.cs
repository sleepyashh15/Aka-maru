using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizUnit : MonoBehaviour
{
  // [SerializeField] PlayerBase _base;
   // [SerializeField] int level;
    [SerializeField] QuizHUD hud;

    public QuizHUD Hud
    {
        get { return hud; }
    }
  //  [SerializeField] bool isPlayerUnit;

    public bool IsPlayerUnit { get; set; }
    public Player Player { get; set; }
  /*  public void Setup()
    {
        Player = new Player(_base, level);
        if (IsPlayerUnit)
        {

        }
        Debug.Log("SETYP NAMAN AH");
        hud.SetData(Player);
    }*/

    public void Setup(Player player)
    {
        //  Player = new Player(_base, level);
        Player = player;
        if (IsPlayerUnit)
        {

        }
     //   Debug.Log("SETYP NAMAN AH");
        hud.SetData(Player);
     //   portal.SetData(Player);
       // Player.stageCount;
    }

    public int QuizCleared()
    {
        Player.QuizCleared();
        return Player.quizCount;
    }
   

    public int ScoreCounted()
    {
        Player.ScoreCount();
        return Player.Score;
    }
}
