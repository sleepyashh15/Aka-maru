using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
   [SerializeField] HumanoidLand _input;
   [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
   [SerializeField] int letterPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;
    public static DialogueManager Instance { get; private set; }
    public float countDownTime = 5f;

    private void Awake()
    {
        Instance = this;
    }
    Dialog dialog;
    int currentLine = 0;    
    public static bool isTyping;


    public IEnumerator ShowDialog(Dialog dialog)
    {
       yield return new WaitForEndOfFrame();       
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
 
    public void HandleUpdate()
    {
        float time = Time.deltaTime;
        // DialogueManager.Instance.HandleUpdate();        
    }

    public void ContinueButton()
    {
        if (!isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }
  
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach(var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;
    }

}
