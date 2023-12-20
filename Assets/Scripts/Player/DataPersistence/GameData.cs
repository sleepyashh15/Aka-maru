using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static GameData;


[System.Serializable]
public class GameData
{
   // public List<GameData> dataPlayers;
    // public Dictionary<string, PlayerBase> players;
    static Dictionary<string, PlayerBase> players;
   // public SerializableDictionary<string, PlayerBase> players;
    public long lastUpdated;
    public int deathCount;
    public Vector2 playerPosition;
    public string SceneLevelSaved;
    public bool isFirstTime;
   

    //  static Dictionary<string, PlayerBase> players;
    //  [SerializeField] List<> players;
    public int quizCount;
    public int stageCount;
    public int retriesCount;
    public bool isTeleported;

    // [SerializeField] List<Player> players;
    public string name;
    public string username;
    public int level;
    public float exp;
    public int score;
    public string saveCount;
    public string profileId;
    public string selectedID;

    public List<string> list;

    public AttributesData playerAttributesData;
    // public List<GameData> dataPlayers;
    public List<GameData> dataPlayers;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        // 
        saveCount = "";
        this.deathCount = 0;      
       // this.stageCount = 0;
        this.retriesCount = 0;
     //   this.exp = 0;
     //   this.level = 0;
      //  this.name = "";
    //playerPosition[] = Vector3.zero;      
    playerPosition = Vector2.zero;
        SceneLevelSaved = "";
        isFirstTime = true;
        isTeleported = false;
        playerAttributesData = new AttributesData();
     //   players = new SerializableDictionary<string, Object>();
        list = new List<string>();
        // players = new SerializableDictionary<string, PlayerBase>();
        // dataPlayers = new List<GameData>();

        //players = new Dictionary<string, PlayerBase>();

      //  dataPlayers = new List<GameData>();
    }

    public static void Init()
    {
        players = new Dictionary<string, PlayerBase>();

        var playerArray = Resources.LoadAll<PlayerBase>("");
        foreach (var player in playerArray)
        {
            if (players.ContainsKey(player.Name))
            {
                Debug.Log($"There are two players with the name {player.Name}");
            }

            players[player.Name] = player;

        }
      
    }
    public static PlayerBase GetPlayerByName(string name)
    {
        if (!players.ContainsKey(name))
        {
            Debug.Log($"Player with name {name} not found in database.");
            return null;

        }
        return players[name];
    }
}