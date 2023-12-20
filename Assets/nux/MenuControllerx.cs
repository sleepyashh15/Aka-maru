using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class MenuControllerx : MonoBehaviour
{
    //public string loaderScene;

    public TextMeshProUGUI musicValue;
    public AudioMixer musicMixer;
    public TextMeshProUGUI soundsValue;
    public AudioMixer soundsMixer;
    public GameObject playMenu;
   // public Button loadButton;

    private Animator animator;
    private int _window = 0;

    public void Start()
    {
        animator = GetComponent<Animator>();
       // loadButton.interactable = SaveManager.IsGameSaved();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _window == 1)
        {
            animator.SetTrigger("HideOptions");
            _window = 0;
        }
    }



    public void HideOptions()
    {
        animator.SetTrigger("HideOptions");
        _window = 0;
    }

    public void TouchToStart()
    {
        Debug.Log("Pindot");
        animator.SetTrigger("TouchToStart");
        _window = 0;
    }

    public void ShowOptions()
     {
         animator.SetTrigger("ShowOptions");
        _window = 1;
     }

    public void ShowPlayMenu()
    {
        Debug.Log("Pressed");
        // animator.SetTrigger("HidePlayMenu");
        animator.SetTrigger("ShowPlayMenu");
        StartCoroutine(isActiveMenu(true));

        // _window = 1;
        // _window = 0;
    }


    public void HideMainMenu()
    {
        animator.SetTrigger("HideMainMenu");
        Debug.Log("Pressto ah");
    }


    public void ShowMainMenuAfter()
    {
        animator.SetTrigger("ShowMainMenuAfter");
        Debug.Log("Pressto ah222");
    }

    public void ShowMainMenu()
    {
        Debug.Log("Pressed");
       // animator.SetTrigger("HidePlayMenu");
        animator.SetTrigger("ShowMainMenu");
        StartCoroutine(isActiveMenu(false));

        // _window = 1;
    }

    public void ShowMainMenuOnLoad()
    {
        animator.SetTrigger("ShowMainMenuOnLoad");
    }

    public void HideMainMenuOnLoad()
    {
        animator.SetTrigger("HideMainMenuOnLoad");
    }

    public IEnumerator isActiveMenu(bool isTrue)
    {
        if(!isTrue)
            yield return new WaitForSeconds(0.50f);

        playMenu.SetActive(isTrue);
    }

    public void ExitGame()
    {
        DataPersistenceManager.instance.SaveGame();
        Application.Quit();
    }


    public void OnMusicChanged(float value)
    {
        musicValue.SetText(value + "%");
        musicMixer.SetFloat("volume", -50 + value / 2);
    }
    
    public void OnSoundsChanged(float value)
    {
        soundsValue.SetText(value + "%");
        soundsMixer.SetFloat("volume", -50 + value / 2);
    }
}
