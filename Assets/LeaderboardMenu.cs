using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] GameObject leaderboard;
    public void CloseLeaderboard()
    {
        leaderboard.SetActive(false);
    }
    public void OpenLeaderboard()
    {
        leaderboard.SetActive(true);
    }
}
