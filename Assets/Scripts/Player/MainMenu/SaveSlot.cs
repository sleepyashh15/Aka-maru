using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SaveSlot : MonoBehaviour, IDataPersistence
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    [SerializeField] private string profileIdforLoad = "";
    string selectedProfileId = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;

    [Header("Clear Data Button")]
    [SerializeField] private Button clearButton;
    [SerializeField] private Button clearButtonBack;

    public bool hasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        // there's no data for this profileId
        if (data == null)
        {
            hasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            clearButton.gameObject.SetActive(false);
           clearButtonBack.gameObject.SetActive(false);
            //    SavingSystem.i.Save("saveSlot0");
            //   SavingSystem.i.Save("saveSlot1");
            //   SavingSystem.i.Save("saveSlot2");

        }
        // there is data for this profileId
        else
        {
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            clearButton.gameObject.SetActive(true);
           clearButtonBack.gameObject.SetActive(true);

            if(!data.isFirstTime)
                percentageCompleteText.text = "Load # " + (Int32.Parse(GetProfileId()) + 1) + " \nPlayer : " + data.dataPlayers[0].username + "\n" + data.SceneLevelSaved;

            //   deathCountText.text = "DEATH COUNT: " + data.deathCount;
        }
    }

    public string GetProfileId()
    {
        return this.profileId;
    }

    public string SetProfileIdSelected(string id)
    {
        selectedProfileId = id;

        return "";
        //  return this.profileId;
    }


    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        //      clearButton.interactable = interactable;
    }

    public void LoadData(GameData data)
    {
        //throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        data.selectedID = this.selectedProfileId;
    }
}

