using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ChooseLabelController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor;
    public Color hoverColor;
    private StoryScene scene;
    private TextMeshProUGUI textMesh;
    private ChooseController controller;
    private ConfirmationPopupAnswer confirmationPopupAnswer;

    private List<ChooseLabelController> siblingLabels;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = defaultColor;

        siblingLabels = new List<ChooseLabelController>(transform.parent.GetComponentsInChildren<ChooseLabelController>());
    }

    void Start()
    {
        // Set the text of the TextMeshProUGUI component
        TextMeshProUGUI textMeshPro = transform.Find("LabelText").GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = textMesh.text;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on the child GameObject.");
        }
    }

    private void ShufflePositions()
    {
        for (int i = siblingLabels.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector3 tempPosition = siblingLabels[i].transform.localPosition;
            siblingLabels[i].transform.localPosition = siblingLabels[j].transform.localPosition;
            siblingLabels[j].transform.localPosition = tempPosition;
        }
    }

    private void DisableLabels()
    {
        foreach (ChooseLabelController label in siblingLabels)
        {
            label.SetInteractable(false);
        }
    }

    public float GetHeight()
    {
        return textMesh.rectTransform.sizeDelta.y * textMesh.rectTransform.localScale.y;
    }

    public void Setup(ChooseScene.ChooseLabel label, ChooseController controller, ConfirmationPopupAnswer confirmationPopupAnswer, float y)
    {
        scene = label.nextScene;
        textMesh.text = label.text;
        this.controller = controller;
        this.confirmationPopupAnswer = confirmationPopupAnswer;

        Vector3 position = textMesh.rectTransform.localPosition;
        position.y = y;
        textMesh.rectTransform.localPosition = position;
        this.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.ShowLabel(false);

        confirmationPopupAnswer.ActivateMenu(
            "Is This your Final Answer? : " + textMesh.text,
            // function to execute if we select 'yes'
            () => { controller.PerformChoose(scene); },
            // function to execute if we select 'cancel'
            () => { controller.ShowLabel(true); }
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = defaultColor;
    }

    private void SetInteractable(bool interactable)
    {
        // Your code to set interactability
    }
}
