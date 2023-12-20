using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class backtoMenu : MonoBehaviour
{
    public string menuScene;

    void Update()
    {
     
    }

    public void BacktoMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(menuScene);
        SavingSystem.i.Save("saveSlot1");
    }
}
