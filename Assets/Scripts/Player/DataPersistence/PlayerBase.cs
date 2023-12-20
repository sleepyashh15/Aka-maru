using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player", menuName = "Player/Create new Player")]

public class PlayerBase : ScriptableObject
{
    [SerializeField] string name;
   // [SerializeField] int exp;
    [SerializeField] int expYield;


  

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int ExpYield => expYield;


    public int GetExpForLevel(int level)
    {
        return level * level * level;
    }
}
