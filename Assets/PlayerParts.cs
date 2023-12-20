using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerParts : MonoBehaviour
{


    [SerializeField] List<DataPlayer> dataPlayers;

    public List<DataPlayer> DataPlayers
    {
        get { return dataPlayers; }
        set { dataPlayers = value; }
    }
    void Start()
    {
        foreach (var dataPlayer in dataPlayers)
        {
            dataPlayer.Init();
        }
    }

  
}
