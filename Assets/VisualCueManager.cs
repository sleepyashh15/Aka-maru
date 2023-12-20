using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCueManager : MonoBehaviour
{
    public event Action OnShowVisualCue;
    public event Action OnCloseVisualCue;
    GameObject visualCue;
    public static VisualCueManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator ShowVisualCue(GameObject visualCue)
    {
        yield return new WaitForEndOfFrame();
        OnShowVisualCue?.Invoke();
        this.visualCue = visualCue;
        
    }
    public IEnumerator HideVisualCue( )
    {
        yield return new WaitForEndOfFrame();
        OnCloseVisualCue?.Invoke();
     //   this.visualCue = visualCue;

    }


}
