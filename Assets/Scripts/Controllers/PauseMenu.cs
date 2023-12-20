using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour, IDataPersistence
{
    // Start is called before the first frame update
    public GameObject pausePanel;
    public string menuScene;
    public ConfirmationPopupMenu confirmationPopupMenu;
    [SerializeField] private string profileId = "";


    public event Action OnPause;
    public event Action OnPauseEnd;

    public static PauseMenu Instance { get; private set; }
    public void Pause()
    {
       // Time.timeScale = 0;
     
        GameController1.Instance.Paused(true);
        pausePanel.SetActive(true);
     
    }


    public void Resume()
    {
        //  Time.timeScale = 1;
     
        GameController1.Instance.Paused(false);
        pausePanel.SetActive(false);
       
    }

    public void BacktoMenu()
    {
        confirmationPopupMenu.ActivateMenu("Are you sure you want go back in Main Menu?",
            
         () => {

             Time.timeScale = 1;
             DataPersistenceManager.instance.SaveGame();           
             SavingSystem.i.Save("saveSlot" + profileId);
             SaveManager.ClearSavedGame();
             SceneManager.LoadScene(menuScene);

         },
        
        () => {
        
        
        
        });

      

    }
    public void ExitGame()
    {
        confirmationPopupMenu.ActivateMenu("Are you sure you want Quit the Game?",

       () => {

           SaveManager.ClearSavedGame();
           Time.timeScale = 1;
           DataPersistenceManager.instance.SaveGame();
           SavingSystem.i.Save("saveSlot" + profileId);
           Application.Quit();

       },

      () => {



      });

        
    }

    public void LoadData(GameData data)
    {
        this.profileId = data.profileId;
    }

    public void SaveData(GameData data)
    {
        data.profileId = this.profileId;
    }
}
