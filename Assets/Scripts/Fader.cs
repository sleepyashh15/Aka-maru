using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        image.raycastTarget = false;
    }
    


    public IEnumerator FadeIn(float time)
    {
        //  image.raycastTarget = true;
        Debug.Log("natatawg ba");
        yield return image.DOFade(1f, time).WaitForCompletion();

    }
    public IEnumerator FadeOut(float time)
    {
       // image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        //   image.raycastTarget = false;
        yield return image.DOFade(0f, time).WaitForCompletion();
      
    }


}
