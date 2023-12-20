using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;
    }
        
    public event Action onPlayerDeath;
    public void PlayerDeath() 
    {
        if (onPlayerDeath != null) 
        {
            onPlayerDeath();
        }
    }
    public event Action onQuizCorrect;

    public void QuizCorrect()
    {
        if (onQuizCorrect != null)
        {
            onQuizCorrect();
        }
    }
    public event Action onQuizRetries;
    public void QuizRetries()
    {
        if (onQuizRetries != null)
        {
            onQuizRetries();
        }
    }

    public event Action onCoinCollected;
    public void CoinCollected() 
    {
        if (onCoinCollected != null) 
        {
            onCoinCollected();
        }
    }
    public event Action onWeaponsCollected;
    public void WeaponsCollected()
    {
        if (onWeaponsCollected != null)
        {
            onWeaponsCollected();
        }
    }
}
