/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Dan.Main;

public class HighscoreTable : MonoBehaviour {

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private string publicLeaderboardKey = "63727d0500f70c208c9b128be840d4bd781ac501d0109c38ce5eed9fe604f7a5";

    public void GetLeaderboard()
    {
    }
    private void Awake()
    {
      // 
    }
    private void Start() {
      
        //  highscore.highscoreEntryList.Clear();
          //PlayerPrefs.DeleteAll();
       

           PlayerPrefs.DeleteKey("highscoreTable" + DataPersistenceManager.instance.GetSelectedProfileId());

           //  PlayerPrefs.DeleteAll();
           //  highscoreEntryList.Clear();
           //  PlayerPrefs.Delete("");
           // PlayerPrefs.GetString("highscoreTable").

           entryContainer = transform.Find("highscoreEntryContainer");
       
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable" + DataPersistenceManager.instance.GetSelectedProfileId());
       // jsonString = "";
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

       
        //highscores.clearHighScore();


        if (highscores == null) {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");

            GetLeaderboard();

            //   AddHighscoreEntry("JOE", 5 );
            //   AddHighscoreEntry("DAV", 3);
            //  AddHighscoreEntry("CAT", 2);
            //   AddHighscoreEntry("MAX", 3);
            //   AddHighscoreEntry("LEN" , 5);


            LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
            {

                //  int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
                for (int i = 0; i < msg.Length; i++)
                {
                    //  names[i].text = msg[i].Username;
                    //   scores[i].text = msg[i].Score.ToString();
                    Debug.Log(i);
                  //  AddHighscoreEntry("CMK", 4);
                    AddHighscoreEntry(msg[i].Username, int.Parse(msg[i].Score.ToString()));

                }

                // Reload
                jsonString = PlayerPrefs.GetString("highscoreTable" + DataPersistenceManager.instance.GetSelectedProfileId());
                highscores = JsonUtility.FromJson<Highscores>(jsonString);

              
                // Sort entry list by Score
                if (highscores.highscoreEntryList.Count > 10)
                {
                    for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
                    {
                        highscores.highscoreEntryList.RemoveAt(10);
                    }
                }

                for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
                    for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                        if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                          // Swap
                           HighscoreEntry tmp = highscores.highscoreEntryList[i];
                           highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                         highscores.highscoreEntryList[j] = tmp;
                        }
                        if (highscores.highscoreEntryList[j].name == highscores.highscoreEntryList[i].name)
                        {
                             highscores.highscoreEntryList.Remove(highscores.highscoreEntryList[j]);
                           // Debug.Log("Remove ka dapat : " + highscores.highscoreEntryList[i].name + highscores.highscoreEntryList[j].score +" or "+highscores.highscoreEntryList[i].score);
                        }
                    }
                }
    
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

            }));



           
        }

      
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
        default:
            rankString = rank + "TH"; break;

        case 1: rankString = "1ST"; break;
        case 2: rankString = "2ND"; break;
        case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();
        
     


       


        // Set background visible odds and evens, easier to read
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        // Highlight First
        if (rank == 1) {
            entryTransform.Find("posText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("scoreText").GetComponent<Text>().color = Color.green;
            entryTransform.Find("nameText").GetComponent<Text>().color = Color.green;
        }

        // Set tropy
        switch (rank) {
        default:
            entryTransform.Find("trophy").gameObject.SetActive(false);
            break;
        case 1:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("FFD200");
            break;
        case 2:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("C6C6C6");
            break;
        case 3:
            entryTransform.Find("trophy").GetComponent<Image>().color = UtilsClass.GetColorFromString("B76F56");
            break;

        }

        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(string name,int score) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { name = name, score = score};
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable" + DataPersistenceManager.instance.GetSelectedProfileId());
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
        if (highscores.highscoreEntryList.Count > 10)
        {
            for (int h = highscores.highscoreEntryList.Count; h > 10; h--)
            {
                highscores.highscoreEntryList.RemoveAt(10);
            }
        }


        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable" + DataPersistenceManager.instance.GetSelectedProfileId(), json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;



        public void clearHighScore()
        {
            highscoreEntryList.Clear();
        }


        

    }

    /*
     * Represents a single High score entry
     * */
    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        //public int retries;
        public string name;
       
    }

}
