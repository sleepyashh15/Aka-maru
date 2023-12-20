using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizCorrectCount : MonoBehaviour
{
    public int correctCount = 0;

    public int CorrectCount
    {
        get { return correctCount; }
    }

    private void Start()
    {
        GameController1.Instance.onCorrectCount += OnCorrectCount;

     

    }
  
    private void OnDestroy()
    {
        // unsubscribe from events
        GameController1.Instance.onCorrectCount -= OnCorrectCount;
    }

    private void OnCorrectCount()
    {
        correctCount++;
       
    }

    public void ResetCorrectCount()
    {
        correctCount = 0;

    }


    /*  public void LoadData(GameData data)
      {
          this.correctCount = data.correctCount;

      }

      public void SaveData(GameData data)
      {
          data.correctCount = this.correctCount;

      }*/
}
