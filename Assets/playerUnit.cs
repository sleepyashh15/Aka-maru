using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerUnit : MonoBehaviour
{
    [SerializeField] int level;
  //  [SerializeField] int exp;
    [SerializeField] bool isPlayerUnit;


    DataPlayer dataPlayer { get; set; }
    public void Setup()
    {
        dataPlayer = new DataPlayer(level);
        if(isPlayerUnit )
        {

        }
    }

}
