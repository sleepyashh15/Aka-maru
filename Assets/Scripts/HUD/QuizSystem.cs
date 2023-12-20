using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuizSystem : MonoBehaviour
{
    //Quiz System for Player only NPC is in Visual Manager setup
    [SerializeField] QuizUnit playerUnit;


    PlayerParty playerParty;

    public void StartQuiz(PlayerParty playerParty)
    {
        this.playerParty = playerParty;
        StartCoroutine(SetupQuiz());
    }

    private void Start()
    {
       

        
    }

    public IEnumerator SetupQuiz()
    {
        yield return new WaitForSeconds(0.3f);
        playerUnit.Setup(playerParty.GetDefaultPlayer());

        yield return 0;
    }



}
