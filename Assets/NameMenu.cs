using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameMenu : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private GameObject Name;
    bool isFirstTime = true;
    private string publicLeaderboardKey = "63727d0500f70c208c9b128be840d4bd781ac501d0109c38ce5eed9fe604f7a5";
    [SerializeField]
    private TMP_InputField inputUserName;
    public Button confirmButton;
    public TextMeshProUGUI isUsernameTakenText;
    public ConfirmationPopupMenu confirmationPopupMenu;
    public PlayerController playerController;
    public QuizSystem quizSystem;
    [SerializeField] private string profileId = "";

    private void Start()
    {
        ShowNameMenu();
    }
    public void ShowNameMenu()
    {
        if (isFirstTime)
        {
            Name.SetActive(true);
        }
        else
        {
            Name.SetActive(false);
        }
    }

    public bool isUsernameAlreadyTaken(string username)
    {
        bool isTaken = false;
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {
            if (msg != null)
            {
                //  int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
                for (int i = 0; i < msg.Length; i++)
                {
                    if (msg[i].Username.Equals(username))
                    {

                        isTaken = true;
                    }
                    else
                    {
                        isTaken = false;
                    }

                }
            }

        }));
        return isTaken;
        
    }

    public void CheckUsername(string username)
    {
        username = inputUserName.text;
        if (isUsernameAlreadyTaken(username))
        {
            isUsernameTakenText.text = "Username is already taken.";
            confirmButton.interactable = false;
        }
        else
        {

            isUsernameTakenText.text = "Username is ready to use.";
            confirmButton.interactable = true;
        }
    }

    public void SetPlayerUsername()
    {
        string username = inputUserName.text;
        if (isFirstTime)
        {

            if (isUsernameAlreadyTaken(username))
            {
                isUsernameTakenText.text = "Username is already taken.";
            //    confirmButton.interactable = false;
            }
            else
            {


                confirmationPopupMenu.ActivateMenu("Will you choose [" + username + "]?",

                () => {
                    PlayerPrefs.DeleteAll();
                    var playerParty = playerController.GetComponent<PlayerParty>();

                    playerParty.GetDefaultPlayer().Username = username;
                 
                    quizSystem.StartQuiz(playerParty);

                   
                    tutorialCanvas.SetActive(true);
                    SavingSystem.i.Delete("saveSlot"+profileId);
                    Name.SetActive(false);

                },

                () => {



                });
            } 

            // StartCoroutine(VisualManager.Instance.ShowVisual(tutorial1));



            //   isFirstTime = false;
            //    isMovementFinish = true;
           
        }
        else
        {

            tutorialCanvas.SetActive(false);
           // SavingSystem.i.Load("saveSlot1");
        }
        DataPersistenceManager.instance.SaveGame();
        SaveManager.ClearSavedGame();

    }
    public void SaveData(GameData data)
    {
        data.isFirstTime = this.isFirstTime;
        data.saveCount = this.profileId;

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
        this.profileId = data.saveCount;

        //  if(!isFirstTime)
        // GetComponent<PlayerParty>().Players = saveData.dataPlayers.Select(s => new Player(s)).ToList();
    }

}
