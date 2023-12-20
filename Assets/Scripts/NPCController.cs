using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.NetworkInformation;


public class NPCController : MonoBehaviour, Interactable, ISavable
{

    public event Action onEnterVisual;
    public event Action onEnterInteract;

    [Header("Profile")]
    [SerializeField] private int npcIndex = 0;

    [SerializeField] Dialog dialog;
    [SerializeField] List<Vector2> movementPattern;
    [SerializeField] float timeBetweenPattern;
    SpriteAnimator spriteAnimator;
    Character character;
    NPCState state;
    float idleTimer = 0f;
    int currentPattern = 0;

    [SerializeField] GameObject visualNovel;
   // [SerializeField] int expYield;
 //   [SerializeField] int level;
    public int stageIndex = -1;
    //public int stageClearedCount  = 0;
    public StageClearCount stageClearCount;
    public GameObject visualCue;
    public GameObject star;
    public PlayerController player;
    public GameScene currentScene;
    public GameScene beforeQuizScene;
    public GameScene passedScene;
    public GameScene failScene;
    public GameScene endQuiz;
    public List<GameScene> Questions;


    public bool isFinishQuiz = false;
    public bool isPlayerInteract = false;
    public static NPCController Instance { get; private set; }

    public QuizCorrectCount quizCorrectCount;

    public event Action onFinishQuiz;
    public VisualManager visualManager;
    public AudioClip[] audioClips;

    // public QuizUnit quizUnit;

    public void Awake()
    {
        character = GetComponent<Character>();
    }

    public int NpcIndex
    {
        get { return npcIndex; }
    }

    public void FinishQuiz()
    {
        if (onFinishQuiz != null)
        {
            onFinishQuiz();
        }
    }

    private void Start()
    {
      //  player = GetComponent<PlayerController>();
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking;

        var oldPos = transform.position;

        yield return character.Move(movementPattern[currentPattern]);

        if(transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle;
    }

    private void Update()
    {
        
        if(star != null)
            star.SetActive(isFinishQuiz);

        if(visualCue != null)
            visualCue.SetActive(!isFinishQuiz);



        //VisualCue();

        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer > timeBetweenPattern)
            {
                idleTimer = 0f;
                //   StartCoroutine(character.Move(new Vector2(2, 0)));
                if (movementPattern.Count > 0)
                    StartCoroutine(Walk());
            }
          

        }

        character.HandleUpdate();
      //  visualCue.SetActive(false);


    }


    public void PlayerInteracted()
    {

          isPlayerInteract = true;
        visualCue.SetActive(true);




        //  SavingSystem.i.Load("saveSlot1");
        // quizCorrectCount.ResetCorrectCount();
    }

    public void xFinishQuiz()
    {

        isFinishQuiz = true;
     
      //  SavingSystem.i.Load("saveSlot1");
       // quizCorrectCount.ResetCorrectCount();
    }
   // [SerializeField] PlayerBase _base;
   // [SerializeField] int level;
    //[SerializeField] QuizHUD hud;
    [SerializeField] QuizUnit playerUnit;

    //  [SerializeField] bool isPlayerUnit;

    public bool IsPlayerUnit { get; set; }
    public Player Player { get; set; }
    /*  public void Setup()
      {
          Player = new Player(_base, level);
          if (IsPlayerUnit)
          {

          }
          Debug.Log("SETYP NAMAN AH");
          hud.SetData(Player);
      }*/

    public void Interact(Transform initiator)
    {


        onEnterVisual?.Invoke();
        //  if (stageClearCount.clearCount < stageIndex)
        //   if (!isFinishQuiz)
        //   {

        // visualNovel.SetActive(true);
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog;
            character.LookTowards(initiator.position);
            Debug.Log("otid bah");

            StartCoroutine(VisualManager.Instance.ShowVisual(currentScene, passedScene, failScene, beforeQuizScene, endQuiz, playerUnit, NpcIndex, isFinishQuiz, Questions, audioClips, () => {
                idleTimer = 0f;
                state = NPCState.Idle;
                GameController1.Instance.StartVisual(this);
            }));
        }
        //  }
        //  else
        // {
        //   FinishQuiz();
        //   StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));

        //  }
        Debug.Log(stageClearCount.clearCount);
        //Debug.Log("Interacting with NPC");
        //  


    }



    public void VisualCue()
    {
        Debug.Log("Cueee");

        onEnterInteract?.Invoke();

       // visualCue.SetActive(true);
        //visualCue.SetActive(player.isInteracting);
        GameController1.Instance.StartInteract(this, visualCue);
        GameController1.Instance.PlayerInteract();
       
        //  GameController1.Instance.PlayerInteract(false);
        // visualCue.SetActive(true);





    }
    public void CloseVisualCue()
    {
        //GameController1.Instance.StartInteract(this);
        GameController1.Instance.PlayerInteract(false);
        // visualCue.SetActive(true);
      // visualCue.SetActive(false);
    }

    public void Show()
    {
        visualCue.SetActive(true);
    }

    public void Hide()
    {
        visualCue.SetActive(false);
    }

    public object CaptureState()
        
    {
        Debug.Log("ETo resulta dapat : " + isFinishQuiz);
        return isFinishQuiz;
     

    }

    public void RestoreState(object state)
    {
        isFinishQuiz = (bool)state;

        Debug.Log("Tapos na nga ba? " + isFinishQuiz);
    }


  


    public enum NPCState { Idle, Walking, Dialog }



}
