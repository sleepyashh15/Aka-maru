using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState { FreeRoam, Dialog, Paused, Visual, Pause }
public enum VisualState { Show, Hide }
public class GameController1 : MonoBehaviour, IDataPersistence
{

    [SerializeField] PlayerController playerController;
    [SerializeField] GameObject touchController;
    [SerializeField] StageClearCount stageClearCount;
    [SerializeField] RoadMap roadmap;
    [SerializeField] GameObject cone;
    [SerializeField] GameObject visualCue;

    GameState state;
    GameState stateBeforePause;
    VisualState visualState;
    public static GameController1 Instance { get; private set; }

    [SerializeField] NPCController npcController;
    [SerializeField] VisualManager visualManager;
    [SerializeField] QuizSystem quizSystem;
    [SerializeField] PortalMenu portalmenu;

    bool isFirstTime;

    public event Action onStageCleared;

    public void StageCleared()
    {
        if (onStageCleared != null)
        {
            onStageCleared();
        }
    }

    public event Action onCorrectCount;

    public void CorrectCount()
    {
        if (onCorrectCount != null)
        {
            onCorrectCount();
        }
    }

    public event Action<bool> OnVisualOver;

    public event Action<bool> OnPlayerInteract;

    public void VisualOver(bool over)
    {
        if (OnVisualOver != null)
        {
            OnVisualOver(over);
        }


    }

    public GameObject PlayerInteract(bool interact = false)
    {
        if (OnPlayerInteract != null)
        {
            OnPlayerInteract(interact);
           
        }
        else
        {
          //  OnPlayerInteract(false);
            //return visualCue;
        }
        return visualCue;


    }

    public event Action<bool> OnPause;

    public void Pause(bool over)
    {
        if (OnPause != null)
        {
            OnPause(over);
        }


    }

    Player player;

    public void SetData(Player playerx)
    {
        //  stageCount = _player.stageCount;
        //  playerx = GetComponent<Player>();
        player = playerx;
    }

    private void Awake()
    {
        Instance = this;
        //  PlayerDB.Init();
       GameData.Init();
    }
    void Start()
    {
        var playerParty = playerController.GetComponent<PlayerParty>();

        if (!isFirstTime)
            quizSystem.StartQuiz(playerParty);

        this.OnVisualOver += EndDialog;
        this.OnPlayerInteract += EndInteract;

     


        DialogueManager.Instance.OnShowDialog += () =>
        {
            state = GameState.Dialog;
        };

        DialogueManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;

        };
        VisualManager.Instance.OnShowVisual += () =>
        {
            state = GameState.Visual;
        };

        VisualManager.Instance.OnCloseVisual += () =>
        {
            if (state == GameState.Visual)
                state = GameState.FreeRoam;

        };


    }

    void EndInteract(bool interact)
    {
        if (npcController != null && interact == true)
        {
             npcController.PlayerInteracted();
          
            //  Debug.Log("end naman ah");
            npcController = null;
           // return true;
            //SaveManager.ClearSavedGame();
        }
        else
        {
           
           // return false;
        }
     
    }

    void EndDialog(bool end)
    {
        if (npcController != null && end == true)
        {
            npcController.xFinishQuiz();
            Debug.Log("end naman ah");
            npcController = null;
            SaveManager.ClearSavedGame();
        }
    }

    public void StartVisual(NPCController npcController)
    {
        this.npcController = npcController;
    }


    public void StartInteract(NPCController npcController, GameObject visualCue)
    {
        this.npcController = npcController;
        this.visualCue = visualCue;
    }

    public void PausedPrompt(bool pause)
    {
        if (pause)
        {
            stateBeforePause = state;
            state = GameState.Pause;
            Debug.Log("eyyy");
        }
        else
        {
            Debug.Log("yekz");
            state = GameState.FreeRoam;
        }
    }

    public void Paused(bool pause)
    {
        if (pause)
        {
            stateBeforePause = state;
            state = GameState.Pause;
            Debug.Log("eyyy");
        }
        else
        {
            Debug.Log("yekz");
            state = stateBeforePause;
        }
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            stateBeforePause = state;
            state = GameState.Paused;
            Debug.Log("eyyy");
        }
        else
        {
            Debug.Log("yekz");
           // state = stateBeforePause;
            state = GameState.FreeRoam;
        }
    }

    private void Update()
    {

        var playerParty = playerController.GetComponent<PlayerParty>();
        roadmap.Roadmaps(playerParty);
        roadmap.StageUnlock();

        portalmenu.Portals(playerParty);
      

        if (playerController.GetComponent<PlayerParty>().GetDefaultPlayer().quizCount >= 4 && SceneManager.GetActiveScene().buildIndex == 3)
        {
            cone.SetActive(false);
        }
        else {};
      
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
            touchController.SetActive(true);
        }
        
        if (state == GameState.Dialog)
        {
           // DialogueManager.Instance.HandleUpdate();
            touchController.SetActive(false);
        }
        if(state == GameState.Visual)
        {
            VisualManager.Instance.HandleUpdate();
            touchController.SetActive(false);
        }

        if (state == GameState.Pause)
        {
           // VisualManager.Instance.HandleUpdate();
            touchController.SetActive(false);
          
        }
        if (state == GameState.Paused)
        {
            // VisualManager.Instance.HandleUpdate();
            touchController.SetActive(false);
        
        }


        /*    if (Input.GetKeyDown(KeyCode.K))
            {
                SavingSystem.i.Save("saveSlot1");

            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SavingSystem.i.Load("saveSlot1");
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SavingSystem.i.Delete("saveSlot1");
            }*/


    }

    public void LoadData(GameData data)
    {
        this.isFirstTime = data.isFirstTime;
    }

    public void SaveData(GameData data)
    {
      
    }
}
