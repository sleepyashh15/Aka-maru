using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour, IPlayerTriggerable, IDataPersistence, ISavable
{
    [SerializeField] private string profileId = "";
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] DestinationIdentifier destinationPortal;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int stageCount;
    // public bool isStageClearedFirst =true;
    public bool isStageClearedFirst = true;
    public bool isStageCleared = true;

    PlayerController player;
    Vector2 playerPosition;
    public int stageIndex = -1;
    // public StageClearCount stageClearCount;
   // public Player Player { get; set; }
    Animator animator;
    Fader fader;



    PlayerController playerController;
    private void Start()
    {
        animator = GetComponent<Animator>();
        fader = FindObjectOfType<Fader>();
      //  playerController = FindObjectOfType<PlayerController>();
     
    }

      //Player _player;
    public void SetData(Player playerx)
    {
        //  stageCount = _player.stageCount;
        //  playerx = GetComponent<Player>();
        Player = playerx;
    }

    public Player Player { get; set; }
    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player;
        SavingSystem.i.Save("saveSlot" + profileId);
        DataPersistenceManager.instance.SaveGame();
        StartCoroutine(SwitchScene(sceneToLoad));
        
    }

    public int StageCleared()
    {
        Player.StageCleared();
        return Player.stageCount;
    }


    public void SwitchSceneOnMap(int scene = -1)
    {
      //  SavingSystem.i.Save("saveSlot" + profileId);
     //   DataPersistenceManager.instance.SaveGame();
       // SceneManager.LoadSceneAsync(scene);
      //  var players = FindObjectOfType<PlayerController>();
        //players.SetPosition
      //  var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
    

      StartCoroutine(SwitchScene(scene));

    }

    public IEnumerator SwitchSceneToMap(int scene = -1)
    {
       

        DontDestroyOnLoad(this.transform.parent.gameObject);
        GameController1.Instance.PauseGame(true);

        yield return SceneManager.LoadSceneAsync(scene);

        yield return new WaitForSeconds(0.36f);



        var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        GameController1.Instance.PauseGame(true);

        yield return new WaitForSeconds(0.36f);

        var players = FindObjectOfType<PlayerController>();
        players.transform.position = destPortal.SpawnPoint.position;

        GameController1.Instance.PauseGame(false);

        Destroy(this.transform.parent.gameObject);
    }

    IEnumerator SwitchScene(int scene = -1)
    {
        if (Player.quizCount >= stageIndex)
        {
            if (isStageClearedFirst)//&& isStageCleared ) 
            {
                StageCleared();
                Debug.Log("Yyeeeeyyyy");
                isStageClearedFirst
                    = false;
            }
            SavingSystem.i.Save("saveSlot" + profileId);
            DataPersistenceManager.instance.SaveGame();
            DontDestroyOnLoad(this.transform.parent.gameObject);

            GameController1.Instance.PauseGame(true);

            // OnPauseBeforeTeleport?.Invoke();
            StartCoroutine(fader.FadeIn(0.5f));

            yield return SceneManager.LoadSceneAsync(scene);

            var players = FindObjectOfType<PlayerController>();
            players.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.001f);
            var fader2 = FindObjectOfType<Fader>();
            fader2.image.color = new Color(fader2.image.color.r, fader2.image.color.g, fader2.image.color.b, 1f);

            DataPersistenceManager.instance.SaveGame();

            // player.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            /*-----------   
                    SavingSystem.i.Delete("saveSlot1");
                    DataPersistenceManager.instance.SaveGame();
                    yield return SceneManager.LoadSceneAsync(sceneToLoad);
                }
                else
                {
                    //   StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));

                }-------*/
            // Debug.Log(stageClearCount.clearCount);
            //Debug.Log("Interacting with NPC");
            //  

            GameController1.Instance.PauseGame(true);
            yield return new WaitForSeconds(0.36f);

            //  var players = FindAnyObjectByType<PlayerController>();

            var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
            GameController1.Instance.PauseGame(true);

            //  var destPortal = FindObjectsOfType<Portal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
            yield return new WaitForSeconds(0.36f);
            //  playerPosition = destPortal.SpawnPoint.position;
            var players2 = FindObjectOfType<PlayerController>();
            //   player.transform.position = destPortal.SpawnPoint.position;      
            players2.gameObject.GetComponent<SpriteRenderer>().enabled = true;


            //  yield return new WaitForSeconds(0.01f);
            players2.transform.position = destPortal.SpawnPoint.position;
            // playerPosition = destPortal.SpawnPoint.position;
            //  player.SetPositionAndSnapToTile(destPortal.spawnPoint.position);



            DataPersistenceManager.instance.SaveGame();
            //   SavingSystem.i.Load("saveSlot" + profileId);
            //     var fader2 = FindObjectOfType<Fader>();
            fader2.image.color = new Color(fader2.image.color.r, fader2.image.color.g, fader2.image.color.b, 1f);
            StartCoroutine(fader2.FadeOut(0.5f));

            GameController1.Instance.PauseGame(true);

            //  OnPauseAfterTeleport?.Invoke();
            GameController1.Instance.PauseGame(false);


            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Bawal pa lods");
            // comingSoon.SetActive(true);
            yield return new WaitForSeconds(5f);
            //comingSoon.SetActive(false);
            //bug.Log("Bawal pa lods");
            //    animator.SetTrigger("Coming");
        }
    }

    public void LoadData(GameData data)
    {
        this.profileId = data.profileId;
    }

    public void SaveData(GameData data)
    {
        data.profileId = this.profileId;
    }

    public object CaptureState()

    {
       // Debug.Log("ETo resulta dapat : " + isStageClearedFirst);
        return isStageClearedFirst;


    }

    public void RestoreState(object state)
    {
        isStageClearedFirst = (bool)state;

       // Debug.Log("Tapos na nga ba? " + isFinishQuiz);
    }

    public Transform SpawnPoint => spawnPoint;

    public enum DestinationIdentifier { A, B, C, D, E }
}
