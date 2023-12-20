using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerParty : MonoBehaviour, IDataPersistence
{
    [SerializeField] List<Player> players;

    public List<Player> Players
    {
        get
        {
            return players;
        }
        set { players = value; }
    }

    private void Awake()
    {
        foreach (var player in players)
        {
            player.Init();
        }
    }

    public Player GetDefaultPlayer()
    {
       // portal.SetData(Player);
        return players.Where(x => x != null).FirstOrDefault();

    }

    public Player GetNPCPlayer(int index)
    {//Where(x => x.Base.name.Equals(name));
        return players[index];
    }

    public void LoadData(GameData data)
    {

        var saveData = new PlayersSaveData();


        //  position = new float[] { transform.position.x, transform.position.y },
        if (!data.isFirstTime)
        {
            Players = data.dataPlayers.Select(s => new Player(s)).ToList();
          
        }


    }

    public void SaveData(GameData data)
    {
        var saveData = new PlayersSaveData();
        // position = new float[] { transform.position.x, transform.position.y },
        data.dataPlayers = Players.Select(p => p.GetSaveData()).ToList();

        
    }

}
[System.Serializable]
public class PlayersSaveData
{
    public float[] position;
    public List<GameData> dataPlayers;

}






