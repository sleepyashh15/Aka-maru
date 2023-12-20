using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu , IDataPersistence
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private ConfirmationPopupMenu confirmationPopupMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;
    public string loaderScene;

    Scene m_Scene;
    string sceneName;
   // public string profId;
    public string profileId;
    private void Start()
    {
        m_Scene = SceneManager.GetActiveScene();
    }

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        string id = saveSlot.GetProfileId();
        // disable all buttons
        DisableMenuButtons();

        // case - loading game
        if (isLoadingGame)
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            // SaveGameAndLoadScene();
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(sceneName);
        }
        // case - new game, but the save slot has data
        else if (saveSlot.hasData)
        {
            confirmationPopupMenu.ActivateMenu(
                "Starting a New Game with this slot will override the currently saved data. Are you sure?",
                // function to execute if we select 'yes'
                () => {
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

                    DataPersistenceManager.instance.NewGame();
                    //  SaveGameAndLoadScene();
                    // profId = ""+saveSlot.GetProfileId();
                    //Debug.Log(saveSlot.GetProfileId() + " NOK NOK NOK1");
                    profileId += saveSlot.GetProfileId();
                    DataPersistenceManager.instance.SaveGame();

                    // SceneManager.LoadSceneAsync("Loader");
                    SceneManager.LoadSceneAsync(loaderScene, LoadSceneMode.Additive);
                   // Debug.Log(saveSlot.GetProfileId() + " NOK NOK NOK2");
                   // selectedID = saveSlot.GetProfileId();
                    //  DataPersistenceManager.instance.NewGame();
                    //  PlayerPrefs.DeleteKey(saveSlot.GetProfileId());
                    //  setProfileId(saveSlot.GetProfileId());
                    // Debug.Log(saveSlot.GetProfileId() + "eto na ngaaaa");
                    // setProfileId(DataPersistenceManager.instance.GetSelectedProfileId());

                    // DataPersistenceManager.instance.SaveGame();

                },
                // function to execute if we select 'cancel'
                () => {

                    this.ActivateMenu(isLoadingGame);
                    // profId = "" + saveSlot.GetProfileId();
                }
            );
        }
        // case - new game, and the save slot has no data
        else
        {
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

            DataPersistenceManager.instance.NewGame();
            // PlayerPrefs.DeleteKey(saveSlot.GetProfileId());
            Debug.Log(saveSlot.GetProfileId() + " NOK NOK NOK2");
            profileId += DataPersistenceManager.instance.GetSelectedProfileId();

            //  setProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.SaveGame();

            // SceneManager.LoadScene("Loader");
            SceneManager.LoadScene(loaderScene, LoadSceneMode.Additive);
          //  selectedID += DataPersistenceManager.instance.GetSelectedProfileId();
            //selectedID = saveSlot.GetProfileId();
        }
    }

  //  [SerializeField] private string profileId = "";

    public void setProfileId(string id)
    {
        profileId = id;
    }
    public void LoadData(GameData data)
    {
        this.sceneName = data.SceneLevelSaved;
        // this.profileId = data.profileId;
        //if (!selectedID.Equals(""))
      //  this.selectedID = data.selectedID; nagbubug dahil dito


    }

    public void SaveData(GameData data)
    {


        data.SceneLevelSaved = this.sceneName;

        if (this.profileId != "")
        {
            data.profileId = this.profileId;
            Debug.Log("save dapat to e " + this.profileId);
        }
        //   Debug.Log("save dapat to e " + this.selectedID);






        // data.profileId = this.profileId;
    }
    private void SaveGameAndLoadScene()
    {
        // save the game anytime before loading a new scene

        Debug.Log("The Scene: " + sceneName);
        // load the scene
        if (sceneName != null)
        {
            SceneManager.LoadSceneAsync(sceneName);

        }
        else
        {
            SceneManager.LoadScene("Loader");
        }
        DataPersistenceManager.instance.SaveGame();

    }

    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActivateMenu(
            "Are you sure you want to delete this saved data?",
            // function to execute if we select 'yes'
            () => {
                PlayerPrefs.DeleteKey(saveSlot.GetProfileId());
                //setProfileId(saveSlot.GetProfileId());
                Debug.Log(profileId);
                DataPersistenceManager.instance.DeleteProfileData(saveSlot.GetProfileId());

                ActivateMenu(isLoadingGame);
            },
            // function to execute if we select 'cancel'
            () => {
                ActivateMenu(isLoadingGame);
            }
        );
    }

    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        // set this menu to be active
        this.gameObject.SetActive(true);

        // set mode
        this.isLoadingGame = isLoadingGame;

        // load all of the profiles that exist
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();

        // ensure the back button is enabled when we activate the menu
        backButton.interactable = true;

        // loop through each save slot in the UI and set the content appropriately
        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        // set the first selected button
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
