using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearCount : MonoBehaviour, IDataPersistence
{
    public int clearCount = 0;


    private void Start()
    {
        GameController1.Instance.onStageCleared += OnStageCleared;
     

    }
  
    private void OnDestroy()
    {
        // unsubscribe from events
        GameController1.Instance.onStageCleared -= OnStageCleared;
    }

    private void OnStageCleared()
    {
        clearCount++;
       
    }

    public void LoadData(GameData data)
    {
        this.clearCount = data.quizCount;
      
    }

    public void SaveData(GameData data)
    {
        data.quizCount = this.clearCount;
       
    }
}
