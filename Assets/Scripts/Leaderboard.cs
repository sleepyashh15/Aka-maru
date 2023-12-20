using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;


    private string publicLeaderboardKey = "8a658e33dd885f5e9d4ebb97d163fc2ccbd07f8e82faf43166c8b969bdfdc95d";

    private void Start()
    {
        GetLeaderboard();
    }
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
                Debug.Log(i);
            }

        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score,
            ((msg) => {

                GetLeaderboard();
        } ) );
    }

}
