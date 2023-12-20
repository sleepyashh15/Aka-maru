using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDB 
{

    static Dictionary<string, PlayerBase> players;

    public static void Init()
    {
        players = new Dictionary<string, PlayerBase>();

        var playerArray = Resources.LoadAll<PlayerBase>("");
        foreach(var player in playerArray)
        {
            if (players.ContainsKey(player.Name))
            {
                Debug.Log($"There are two players with the name {player.Name}");
            }

            players[player.Name] = player;
           
        }
        Debug.Log("Loaded aman");

    }
    public static PlayerBase GetPlayerByName (string name)
    {
        if (!players.ContainsKey(name))
        {
            Debug.Log($"Player with name {name} not found in database.");
            return null;

        }
        return players[name];
    }
}
