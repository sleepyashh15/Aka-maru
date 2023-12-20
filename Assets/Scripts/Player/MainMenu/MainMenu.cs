using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu, IDataPersistence
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
   // [SerializeField] private Button loadGameButton;
    Scene m_Scene;
    string sceneName;
    Animator animator;
    private void Start() 
    {
        m_Scene = SceneManager.GetActiveScene();
        DisableButtonsDependingOnData();
        animator = GetComponent<Animator>();
    }

    private void DisableButtonsDependingOnData() 
    {
        if (!DataPersistenceManager.instance.HasGameData()) 
        {
            continueGameButton.interactable = false;
           // loadGameButton.interactable = false;
        }
    }

    public void LoadData(GameData data)
    {
        this.sceneName = data.SceneLevelSaved;
      
    }

    public void SaveData(GameData data)
    {
    
      
    }

    public void OnNewGameClicked() 
    {
       saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGameClicked() 
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }

    public void OnContinueGameClicked() 
    {
        DisableMenuButtons();
        // save the game anytime before loading a new scene
        DataPersistenceManager.instance.SaveGame();
        // load the next scene - which will in turn load the game because of 
        // OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync(sceneName);
    }

    private void DisableMenuButtons() 
    {
       // newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    public void ActivateMenu() 
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }

    public void DeactivateMenu() 
    {
       // HideMainMenu();
      ///  this.gameObject.SetActive(false);
      ///  

    }
    public void HideMainMenu()
    {
        // Debug.Log("Pressed");
        // animator.SetTrigger("HidePlayMenu");
        animator.SetTrigger("HideMainMenu");
        // StartCoroutine(isActiveMenu(true));

        // _window = 1;
        // _window = 0;
    }
    public void ExitGame()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }

    public void ActivateMainMenuxxxxx()
    {
        
        this.gameObject.SetActive(false);

    }

}
