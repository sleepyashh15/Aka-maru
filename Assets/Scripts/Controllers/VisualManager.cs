    using Dan.Main;
using Dan.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualManager : MonoBehaviour, IDataPersistence, ISavable
{
    public ConfirmationPopupMenu confirmationPopupMenu;
    private string publicLeaderboardKey = "63727d0500f70c208c9b128be840d4bd781ac501d0109c38ce5eed9fe604f7a5";
    //Transmigration
   public GameScene currentScene;
   public GameScene passedScene;
    public GameScene failScene;
    public GameScene AfterQuiz;
    public GameScene tempScene;

    public GameScene beforeQuizScene;
    bool isFinishQuiz;
    public GameObject skipButton;
    public GameObject cancelButton;

    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;
    public ChooseController chooseController;
    public AudioController audioController;
    public NPCController nPCController;
    public DataHolder data;
    public GameObject nextArrow;

    public string menuScene;
    public enum GameState { FreeRoam, Dialog }

    private State state = State.IDLE;

    public int quizClearCount = -1;

    private List<StoryScene> history = new List<StoryScene>();

    public event Action OnShowVisual;
    public event Action OnCloseVisual;
    public static VisualManager Instance { get; private set; }
    public QuizCorrectCount quizCorrectCount;
    public TextMeshProUGUI scoreText;
    PlayerBase _base;

    QuizUnit _npcUnit;
    int _npcIndex;
    public PlayerController playerController;
    //public int stageClearCount;
    public List<GameScene> questions;

    public bool IsShowing { get; private set; }

    VisualManager[] visual;

    public QuizManager quizManager;
    public GameObject audioPlayBack;
    public Button audioPlayBackButton;
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    private int currentClipIndex = -1;

    //[SerializeField] PlayerParty party;
    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }


    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;

    //END
    public PlayerController characterController { get; set; }

    public void SetData(PlayerController player)
    {
        //characterController = new PlayerController(level, exp);
       // levelText.text = "" + player.level;
;       // expText.text = "" + player.exp;

    }


    private void Awake()
    {
        nPCController = GetComponent<NPCController>();
        Instance = this;
        visual = this.GetComponentsInChildren<VisualManager>();
    }

    private void Start()
    {
        // stageClearCount = playerUnit.Player.stageCount;
        audioPlayBackButton.onClick.AddListener(PlayCurrentAudio);
      
    }
    void PlayCurrentAudio()
    {
        // Check if the audio source and clip array are assigned
        if (audioSource != null && audioClips != null && audioClips.Length > 0)
        {
            // Stop the audio playback if it's already playing
            audioSource.Stop();

            // Set the current audio clip and play it
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();
        }
        else
        {
            Debug.Log("AudioSource or AudioClip array not assigned!");
        }
    }

    // Method to manually switch to the next audio clip
    public void SwitchToNextAudio()
    {
        // Increment the index for the next audio clip
        currentClipIndex = (currentClipIndex + 1) % audioClips.Length;

        // Play the newly selected audio clip
        PlayCurrentAudio();
    }

    // GameState state; 


    public void showChild(bool isTrue)
    {
        /* GameObject[] children;
         children = visual.GetComponentsInChildren<VisualManager>();
         foreach (GameObject child in children)
         {
             child.SetActive(false);

         }*/
       
        

        if (SaveManager.IsGameSaved())
        {
            SaveData data = SaveManager.LoadGame();
            data.prevScenes.ForEach(scene =>
            {
                history.Add(this.data.scenes[scene] as StoryScene);
            });
            currentScene = history[history.Count - 1];
            history.RemoveAt(history.Count - 1);
            bottomBar.SetSentenceIndex(data.sentence - 1);
        }

        for (int i = 0; i < transform.childCount-1; i++)
        {
            Debug.Log(i+"+++");
           //  Transform child = transform.GetChild(i);
          //  child.transform.gameObject.SetActive(true);
            transform.GetChild(i).gameObject.SetActive(isTrue);
        }
        audioPlayBack.SetActive(false);
    }


    Action onFinished;
 
    public IEnumerator ShowVisual(GameScene gameScene,GameScene pass,GameScene fail, GameScene beforeQuiz, GameScene AfterQuiz, QuizUnit _npcUnit, int npcIndex, bool isFinishQuiz, List<GameScene> questions, AudioClip[] audioClips, Action onFinished=null)
        {
  
        yield return new WaitForEndOfFrame();
        this.currentScene = gameScene;
        this.passedScene = pass;
        this.failScene = fail;
        this.beforeQuizScene = beforeQuiz;
        this._npcUnit = _npcUnit;
        this._npcIndex = npcIndex;
        showChild(true);
        this.onFinished = onFinished;
        this.isFinishQuiz = isFinishQuiz;
        OnShowVisual?.Invoke();
        IsShowing = true;
        this.questions = questions;
        this.AfterQuiz = AfterQuiz;
        this.audioClips = audioClips;
        var playerParty = playerController.GetComponent<PlayerParty>();
        StartQuiz(playerParty);

        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;

            bottomBar.PlayScene(storyScene, bottomBar.GetSentenceIndex());
          
            PlayAudio(storyScene.sentences[bottomBar.GetSentenceIndex()]);
        }
      //

    

    }
    public void SkipConversation()
    {
        confirmationPopupMenu.ActivateMenu("Are you sure you want go Skip The Conversation?",

     () => {

         PlayScene(beforeQuizScene);

     },

    () => {



    });


       
    }
    public void HandleUpdate(){

      

        if (state == State.IDLE)
        {
            if (!currentScene.name.Contains('Q'))
            {
                skipButton.SetActive(isFinishQuiz);
            }
           
            cancelButton.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) )
            {

                if (bottomBar.IsCompleted())
                {
                    bottomBar.StopTyping();
                    if (bottomBar.IsLastSentence())
                    {

                        if ((currentScene as StoryScene).nextScene.IsUnityNull())
                        {
                            Debug.Log("whuur");

                            if (currentScene.name.Equals("Tutorial") || currentScene.name.Equals("Tutorial2"))
                            {


                            }
                            else
                            {

                                // GameController1.Instance.StageCleared();
                                DataPersistenceManager.instance.SaveGame();

                                nextArrow.SetActive(true);
                                chooseController.DestroyLabels();
                                this.onFinished?.Invoke();
                                if (quizCorrectCount.CorrectCount >= 4)
                                {
                                    // nPCController.FinishQuiz();
                                    //
                                    //   quizCorrectCount.ResetCorrectCount();




                                    for (int i = 0; i < quizCorrectCount.CorrectCount; i++)
                                    {
                                        playerUnit.ScoreCounted();
                                    }

                                    //    StartCoroutine (HandleQuizEnd());

                                    ResetVisualNovel();
                                    //cancelButton.SetActive(false);
                                    GameController1.Instance.VisualOver(true);


                                    if (!isFinishQuiz)
                                        SetLeaderboardEntry(playerUnit.Player.Username, playerUnit.Player.Score);

                                }
                                if (SceneManager.GetActiveScene().name.Equals("Tutorial"))
                                {
                                    SaveManager.ClearSavedGame();

                                    //  SavingSystem.i.Save("saveSlot1");                                  
                                    OnCloseVisual?.Invoke();
                                    GameController1.Instance.VisualOver(true);
                                    //  ResetVisualNovel();
                                    // StartCoroutine(HandleQuizEnd());

                                }



                            }

                            if (!isFinishQuiz)
                                StartCoroutine(HandleQuizEnd(_npcUnit));

                            SaveManager.ClearSavedGame();
                            OnCloseVisual?.Invoke();
                            showChild(false);



                            // this.gameObject.SetActive(false);

                        }
                        else
                        {





                            if ((currentScene as StoryScene).name.Contains("!") && quizCorrectCount.correctCount > 3)
                            {
                                //    PlayScene((passedScene as StoryScene));
                                // PlayScene((currentScene as StoryScene).nextScene = passedScene);
                                // (currentScene as StoryScene).nextScene = currentScene;
                                PlayScene(passedScene);
                                GenerateNewSet();
                            }
                            else if ((currentScene as StoryScene).name.Contains("!") && (quizCorrectCount.correctCount < 4))
                            {
                                //   PlayScene((failScene as StoryScene));
                                //  PlayScene((currentScene as StoryScene).nextScene = failScene);
                                //  (currentScene as StoryScene).nextScene = currentScene;
                                PlayScene(failScene);
                                GenerateNewSet();
                            }
                            else if ((currentScene as StoryScene).nextScene.name.Contains("Question"))
                            {
                                int QuizNumber = GetNextNumber();
                                Debug.Log("Questions Number : " + QuizNumber);
                                if (QuizNumber >= 1)
                                {

                                    PlayScene(questions[QuizNumber - 1]);
                                    // PlayScene(failScene);
                                    Debug.Log("Questions Number : " + QuizNumber);
                                }
                                else
                                {
                                    // PlayScene(AfterQuiz);
                                    //tempScene = (currentScene as StoryScene).nextScene;
                                   // if()
                                  //  (currentScene as StoryScene).nextScene = AfterQuiz;
                                    PlayScene(AfterQuiz);
                                  
                                }

                            }


                            else
                            {
                                PlayScene((currentScene as StoryScene).nextScene);
                            }

                           

                        }


                    }
                    else
                    {
                        //

                        bottomBar.PlayNextSentence();
                        PlayAudio((currentScene as StoryScene)
                            .sentences[bottomBar.GetSentenceIndex()]);

                       
                    }
                }
                else
                {
                  
                    bottomBar.SpeedUp();
                    
                }
            }

            /* if (Input.GetMouseButtonDown(1))
             {
                 if (bottomBar.IsFirstSentence())
                 {
                     if (history.Count > 1)
                     {
                         bottomBar.StopTyping();
                         bottomBar.HideSprites();
                         history.RemoveAt(history.Count - 1);
                         StoryScene scene = history[history.Count - 1];
                         history.RemoveAt(history.Count - 1);
                         PlayScene(scene, scene.sentences.Count - 2, false);
                     }
                 }
                 else
                 {
                     bottomBar.GoBack();
                 }
             }*/
            /*   if (Input.GetKeyDown(KeyCode.Escape))
               {
                   List<int> historyIndicies = new List<int>();
                   history.ForEach(scene =>
                   {
                       historyIndicies.Add(this.data.scenes.IndexOf(scene));
                   });
                   SaveData data = new SaveData
                   {
                       sentence = bottomBar.GetSentenceIndex(),
                       prevScenes = historyIndicies
                   };
                   SaveManager.SaveGame(data);
                   SavingSystem.i.Save("saveSlot1");
                   SceneManager.LoadScene(menuScene);

                   }*/
        }
        
    }
    int expYield;
    int npc_level =1;
    //QUIZ SYSTEM 
    //need still reference for player for verification purpose
    [SerializeField] QuizUnit playerUnit;

    public void CancelPressed()
    {
        GameController1.Instance.PausedPrompt(true);
        confirmationPopupMenu.ActivateMenu("Are you sure you want go Close Conversation?",

            () => {

                ResetVisualNovel();
                GameController1.Instance.PausedPrompt(false);
               

            },

            () => {

                OnShowVisual?.Invoke();

            });
       
    }
    public void ResetVisualNovel()
    { 
       
        SaveManager.ClearSavedGame();
        chooseController.DestroyLabels();
        bottomBar.HideSprites();
        bottomBar.SetSentenceIndex(-1);
      //  SavingSystem.i.Save("saveSlot1");
        quizCorrectCount.ResetCorrectCount();
        cancelButton.SetActive(false);
        showChild(false);
        //  GameController1.Instance.VisualOver(true);
        scoreText.text = "";
        //
        // numbers.Clear();
        //  printedNumbers.Clear();
        GenerateNewSet();
        //
        SaveManager.ClearSavedGame();
        IsShowing = false;


          //    yield return new WaitForSeconds(0.36f);
          OnCloseVisual?.Invoke();
        this.onFinished?.Invoke();
        currentClipIndex = -1;

    }

    PlayerParty playerParty;

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score,
            ((msg) => {

               // GetLeaderboard();
            }));
    }
    public void StartQuiz(PlayerParty playerParty)
    {
        this.playerParty = playerParty;
        StartCoroutine(SetupQuiz());
    }
    // Quiz Setup for NPC 
    public IEnumerator SetupQuiz()
    {
        //_npcUnit.Setup();
        yield return new WaitForSeconds(0.3f);
        _npcUnit.Setup(playerParty.GetNPCPlayer(_npcIndex));
      
        // playerUnit.Setup();
        //   npcUnit.Setup();

        //     playerHud.SetData(playerUnit.Player);
        // playerUnit.Setup
        //   npcUnit.Setup();
        //   npcHud.SetData(npcUnit.Player);
        yield return 0;
    }
    IEnumerator HandleQuizEnd(QuizUnit quizEndUnit)
    {
        //Exp Gain

        if(!quizEndUnit.IsPlayerUnit)
        {
            int expYield = quizEndUnit.Player.Base.ExpYield;
            int enemyLevel = quizEndUnit.Player.Level;
            float bonus = 1f;
            int expGain = Mathf.FloorToInt((expYield * enemyLevel * bonus) / 7);
            playerUnit.Player.Exp += expGain;
            Debug.Log("Has gained EXP : " + expGain);
            quizClearCount = playerUnit.QuizCleared();
            yield return playerUnit.Hud.SetExpSmooth();

            //Check for level up
            while (playerUnit.Player.CheckForLevelUp())
            {
                Debug.Log("Grew Level: " + playerUnit.Player.Level);
                playerUnit.Hud.SetLevel();
                yield return playerUnit.Hud.SetExpSmooth(true);
            }


            //stageClearCount = playerUnit.QuizCleared(); nilipat sa taas
            yield return new WaitForSeconds(1f);


        }
      
       
      
      
       

        //Check for Level up
     /*   while(dataPlayer.CheckForLevelUp())
        {
            playerHUD.SetLevel();
         
            yield return playerHUD.SetExpSmooth(true);


        }


        yield return new WaitForSeconds(1f);
     */
        //Check for Level Up
     /*   while (CheckForLevelUp())
        {
            playerHUD.Setlevel();

            yield return playerHUD.SetExpSmooth(true);
            Debug.Log("grew "+level);
        }*/

    }
    [SerializeField] int level;
    [SerializeField] int exp;
    public DataPlayer dataPlayer { get; set; }

    
    public void Setupx()
    {
        dataPlayer = new DataPlayer(level);
    }


  //  [SerializeField] PlayerHUD playerHUD;


   /* void Start()
    {
        Setupx();
        playerHUD.SetData(dataPlayer);

    }*/


    //public int exp = 0;
    /// public int level = 1;


    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        

        StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated));

        if (scene.name.Contains('-'))
        {
            GameController1.Instance.CorrectCount();

        }
        if (scene.name.Contains('='))
        {
            currentClipIndex++;
            audioPlayBack.SetActive(true);

        }
        if (scene.name.Contains('Q'))
        {
            audioPlayBack.SetActive(false);
            skipButton.SetActive(false);
            scoreText.text = ("Score: ") + quizCorrectCount.CorrectCount + "/5";
           
        }        
    

        if (scene.name.Contains('F'))
        {
            quizCorrectCount.ResetCorrectCount();
        }

        if(currentScene == (scene as ChooseScene))
        {
            cancelButton.SetActive(false);
            skipButton.SetActive(false);
        }
        

        

    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true, bool isQuiz = false)
    {
        state = State.ANIMATE;
        currentScene = scene;

        if (scene.name.Contains('Q'))
        {        
            isQuiz = true;
            
        }
        else
        {
            isQuiz = false;
        }



        if (isAnimated)
        {
          //  bottomBar.Hide();
            yield return new WaitForSeconds(0);
        }
        if (scene is StoryScene)
        {
            StoryScene storyScene = scene as StoryScene;
            history.Add(storyScene);
            PlayAudio(storyScene.sentences[sentenceIndex + 1]);
            if (isAnimated)
            {
              //  backgroundController.SwitchImage(storyScene.background);
              //  yield return new WaitForSeconds(1f);
                bottomBar.ClearText();
              //  bottomBar.Show();
             //   yield return new WaitForSeconds(1f);
            }
            else
            {
              //  backgroundController.SetImage(storyScene.background);
                bottomBar.ClearText();
            }
            bottomBar.PlayScene(storyScene, sentenceIndex, isAnimated);
            state = State.IDLE;
        }
        else if (scene is ChooseScene)
        {
            state = State.CHOOSE;
            if(isQuiz)
                chooseController.SetupChoose(scene as ChooseScene, true);
            else
                chooseController.SetupChoose(scene as ChooseScene);


        }
      
    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }


    public void LoadData(GameData data)
    {
        this.level = data.level;
       // playerHUD.SetExp();
        // this.exp = data.exp;

    }

    public void SaveData(GameData data)
    {
       // data.level = this.level;
       // this.level = data.level;

    }

    public object CaptureState()
    {
        //  throw new NotImplementedException();
        return 0;
    }

    public void RestoreState(object state)
    {
        // throw new NotImplementedException();
      //  var saveData = (SaveDataPlayer)state;
        // var pos = saveData.position;
      //  saveData.level = level;
      //  saveData.exp = exp;
    }

    private List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
    private List<int> printedNumbers = new List<int>();

    public void ShuffleNumbers()
    {
        System.Random rand = new System.Random();

        for (int i = numbers.Count - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            int temp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = temp;
        }
    }

    public int GetNextNumber()
    {
        if (printedNumbers.Count < numbers.Count)
        {
            int nextNumber;

            do
            {
                ShuffleNumbers(); // Reshuffle until a new number is found
                nextNumber = numbers[printedNumbers.Count];
            } while (printedNumbers.Contains(nextNumber));

            printedNumbers.Add(nextNumber);
            Debug.Log($"Shuffled Next Number: {nextNumber}");
            return nextNumber;
        }
        else
        {
            Debug.Log("All numbers have been returned.");
            return -1; // or another value indicating no more numbers
        }
    }

    public void GenerateNewSet()
    {
        printedNumbers.Clear();
        ShuffleNumbers();
        Debug.Log("Generated a new set of shuffled numbers.");
    }

    // Example: Call this when you want to get and print the next number
    public void WhenReadyToPrint()
    {
        GetNextNumber(); // The number is printed inside the GetNextNumber method
    }

    // Example: Call this in response to a button click
    public void OnButtonClick()
    {
        WhenReadyToPrint();
    }

    // Example: Call this in response to another button click to generate a new set
    public void OnGenerateNewSetButtonClick()
    {
        GenerateNewSet();
    }
}
