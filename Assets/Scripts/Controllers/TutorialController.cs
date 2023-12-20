using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TutorialController : MonoBehaviour, IDataPersistence
{
    

    //Transmigration
    public GameScene currentScene;
    public BottomBarController bottomBar;
    public SpriteSwitcher backgroundController;
    public ChooseController chooseController;
    public AudioController audioController;
   // public NPCController nPCController;
    public DataHolder data;
    public GameObject nextArrow;

    //public string menuScene;
    public enum GameState { FreeRoam, Dialog }

    private State state = State.IDLE;


    private List<StoryScene> history = new List<StoryScene>();



    private enum State
    {
        IDLE, ANIMATE, CHOOSE
    }

    //END



    void Start()
    {

  
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
        if (currentScene is StoryScene)
        {
            StoryScene storyScene = currentScene as StoryScene;
            history.Add(storyScene);
            bottomBar.PlayScene(storyScene, bottomBar.GetSentenceIndex());
            // backgroundController.SetImage(storyScene.background);
            PlayAudio(storyScene.sentences[bottomBar.GetSentenceIndex()]);
        }
    }
    public void Update(){

        

        if (state == State.IDLE)
        {
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

                            if (currentScene.name.Equals("Tutorial") ||currentScene.name.Equals("Tutorial2"))
                            {
                             
                                
                            }
                            else
                            {
                                GameController1.Instance.StageCleared();
                                DataPersistenceManager.instance.SaveGame();
                                nextArrow.SetActive(true);
                            }
                         
                            this.gameObject.SetActive(false);
                           
                        }
                        else
                        {
                       
                        PlayScene((currentScene as StoryScene).nextScene);
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
            if (Input.GetMouseButtonDown(1))
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
            }
            if (Input.GetKeyDown(KeyCode.Escape))
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
           //     SceneManager.LoadScene(menuScene);
               
            }
        }
    }
    public void PlayScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        StartCoroutine(SwitchScene(scene, sentenceIndex, isAnimated));
    }

    private IEnumerator SwitchScene(GameScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        state = State.ANIMATE;
        currentScene = scene;
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
            chooseController.SetupChoose(scene as ChooseScene);
        }
    }

    private void PlayAudio(StoryScene.Sentence sentence)
    {
        audioController.PlayAudio(sentence.music, sentence.sound);
    }


    public void LoadData(GameData data)
    {
     //   throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
      //  throw new System.NotImplementedException();
    }
}
