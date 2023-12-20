using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;


public class PlayerController : MonoBehaviour, IDataPersistence, ISavable
{


    public int level { get; set; }
    public int Exp { get; set; }
    //



    public int GetExpForLevel(int level)
    {
        return level * level * level;
    }



    public HumanoidLand _input;
    public float moveSpeed;
    public LayerMask solidObjectLayer;
    public LayerMask interactableLayer;
    public LayerMask portalLayer;
    public DialogueManager dialogueManager;
    public GameObject visualCue;

    private bool isMoving;
    private Vector2 movement;

    private Animator animator;
    public static float timeRemaining = 10;
    bool isTimeOut;
    public int stageClearedCount = 0;

    Scene m_Scene;
    public string sceneName;
    public bool isFirstTime = true;
    public GameObject tutorialCanvas;
    public GameObject tutorialCanvas2;
    [SerializeField] private string profileId = "";
    [SerializeField] private int profId;
    bool isMovementFinish = false;
    public bool isInteracting { get; private set; } = false;

    public float offsetY { get; private set; } = 0.3f;

    private void Awake()
    {

        animator = GetComponent<Animator>();
        // _input = new HumanoidLand();
        SetPositionAndSnapToTile(transform.position);


    }
    private void Start()
    {



        m_Scene = SceneManager.GetActiveScene();
        GameController1.Instance.onStageCleared += OnStageCleared;
        sceneName = m_Scene.name;
        if (isFirstTime)
        {


            // StartCoroutine(VisualManager.Instance.ShowVisual(tutorial1));



            //   isFirstTime = false;
            isMovementFinish = true;
           // tutorialCanvas.SetActive(true);
            SavingSystem.i.Delete("saveSlot" + profileId);
        }
        else
        {

           // tutorialCanvas.SetActive(false);
            SavingSystem.i.Load("saveSlot" + profileId);
        }
        SaveManager.ClearSavedGame();

    }

    private void OnApplicationQuit()
    {
        SavingSystem.i.Save("saveSlot"+ profileId);
        SaveManager.ClearSavedGame();

    }
    public void OnStageCleared()
    {
        stageClearedCount++;

    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + offsetY;

        transform.position = pos;
    }

    public void SetPosition(Vector2 pos)
    {
        pos.x = 0f;
        pos.y = 0f;

        transform.position = pos;
    }


    public void HandleUpdate()
    {

        //Exp = GetExpForLevel(level);

        if (!isMoving)
        {
            //   // input.x = Input.GetAxisRaw("Horizontal");
            //  input.y = Input.GetAxisRaw("Vertical");
            // movement.x = UnityEngine.Input.GetAxisRaw("Horizontal");
            // movement.y = UnityEngine.Input.GetAxisRaw("Vertical");
            movement = new Vector3(_input.MoveInput.x, _input.MoveInput.y);
            // if (input.x != 0) input.y = 0;
            if (movement != Vector2.zero)
            {
                animator.SetFloat("moveX", movement.x);
                animator.SetFloat("moveY", movement.y);
                var targetPos = transform.position;
                targetPos.x += movement.x;
                targetPos.y += movement.y;

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));

                }
                onMoveOver();
                VisualCue();                

            }
           

        }
        animator.SetBool("isMoving", isMoving);

        if (_input.InteractIsPressed)
        {
            Interact();
        }


    }
    public void Interact()
    {

        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
            UnityEngine.Debug.Log("Auaw mo bah");

        }
        else
        {

        }


    }


    public void VisualCue()
    {

        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);

       

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.VisualCue();
            visualCue = GameController1.Instance.PlayerInteract();
           
           // visualCue.SetActive(false);
            isInteracting = true;


            if (isMovementFinish)
            {
                isFirstTime = false;

                //  StartCoroutine(VisualManager.Instance.ShowVisual(tutorial2));




                isMovementFinish = false;
                tutorialCanvas2.SetActive(true);
                DataPersistenceManager.instance.SaveGame();
            }
        }
        else
        {
            isInteracting = false;
           
            // collider.GetComponent<Interactable>()?.CloseVisualCue();
            //  StartCoroutine(VisualCueManager.Instance.HideVisualCue());
            //   collider.GetComponent<Interactable>()?.CloseVisualCue();
            //  visualCue.SetActive(false);
        }

        if (isInteracting)
        {

        }
        else
        {
            //  GameController1.Instance.StartInteract(GetComponent<NPCController>());
            //./ visualCue = GameController1.Instance.PlayerInteract(false);
           // visualCue.SetActive(true);
        }

       
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;


    }

    public void onMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, offsetY), 0.3f, GameLayers.i.PortalLayer);

        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if (triggerable != null)
            {
                triggerable.OnPlayerTriggered(this);
                break;
            }
        }

    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidObjectLayer | GameLayers.i.InteractableLayer) != null)
        {
            return false;

        }

        return true;
    }



    public void SaveData(GameData data)
    {

        //  Debug.Log("Loaded aman");
        //   data.SceneLevelSaved = this.sceneName;
        //  data.playerPosition = new Vector3(transform.position.x, transform.position.y);

        //  data.isFirstTime = this.isFirstTime;
        //     data.isTeleported = this.isTeleported;

        // data.position = new float[] { transform.position.x, transform.position.y };

        data.isFirstTime = this.isFirstTime;
       // data.profileId = this.profileId;

        // data.dataPlayers 

        //    var s = GetComponent<PlayerParty>().Players.Select(p => p.GetSaveData()).ToList();

        //position = new float[] { transform.position.x, transform.position.y },
        //  data.dataPlayers = GetComponent<PlayerParty>().Players.Select(p => p.GetSaveData()).ToList();

        //  position = new float[] { transform.position.x, transform.position.y },

        // data.players = GetComponent<PlayerParty>().Players.Select(p => p.GetSaveData()).ToList();




    }
    public void LoadData(GameData data)
    {
        var saveData = new PlayerSaveData();
        //  var saveData = (PlayerSaveData)Object;
        //  this.sceneName = data.SceneLevelSaved;
        //    this.isTeleported = data.isTeleported;
        //  this.isFirstTime = data.isFirstTime;
        // transform.position = new Vector3(data.position[0], data.position[1]);
        //  transform.position = new Vector3(data.playerPosition.x, data.playerPosition.y);
        this.isFirstTime = data.isFirstTime;
        this.profileId = data.profileId;

        //  if(!isFirstTime)
          // GetComponent<PlayerParty>().Players = saveData.dataPlayers.Select(s => new Player(s)).ToList();
    }

    public object CaptureState()
    {
        var saveData = new PlayerSaveData()
        {
            position = new float[] { transform.position.x, transform.position.y },
            // dataPlayers = GetComponent<PlayerParty>().Players.Select(p => p.GetSaveData()).ToList()

        };

        return saveData;

    }

    public void RestoreState(object state)
    {
        var saveData = (PlayerSaveData)state;
        var pos = saveData.position;
        transform.position = new Vector3(pos[0], pos[1]);
        // stageClearedCount = saveData.stageClearedCount;
        //   GetComponent<PlayerParty>().Players = saveData.dataPlayers.Select(s => new Player(s)).ToList();


    }

}

  [System.Serializable]
public class PlayerSaveData
{
        public float[] position;
        public List<GameData> dataPlayers;
       
     
 }

