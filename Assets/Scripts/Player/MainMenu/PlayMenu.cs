using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject playMenu;
    public void ActivatePlayMenu()
    {
        this.gameObject.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
        // DisableButtonsDependingOnData();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
