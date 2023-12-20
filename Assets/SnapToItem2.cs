using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SnapToItem2 : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;

    public HorizontalLayoutGroup HLG;

    public TextMeshProUGUI NameLabel;
    public string[] ItemNames;

     void Update()
    {
        int currentItem = Mathf.RoundToInt((0 - contentPanel.localPosition.x / (sampleListItem.rect.width + HLG.spacing)));
        Debug.Log(sampleListItem.rect.width);
        
            Debug.Log(HLG.spacing);
        Debug.Log(currentItem);

    }

}
