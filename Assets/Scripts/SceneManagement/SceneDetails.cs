        using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour, IDataPersistence
{


    Scene m_Scene;
    public string sceneName;
    public string menuScene;

    private void Start()
    {
        m_Scene = SceneManager.GetActiveScene();
        sceneName = m_Scene.name;
        // set the desired aspect ratio, I set it to fit every screen 
       
    }

    private void Update()
    {
     /*   if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene(menuScene);
            SavingSystem.i.Save("saveSlot1");
        }*/
    }
    public void LoadData(GameData data)
    {
        this.sceneName = data.SceneLevelSaved;
    }

    public void SaveData(GameData data)
    {
        data.SceneLevelSaved = this.sceneName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log(sceneName);



        }
    }


}
