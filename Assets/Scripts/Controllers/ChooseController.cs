using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChooseController : MonoBehaviour
{
    public ChooseLabelController label;
    public VisualManager visualManager;
    private RectTransform rectTransform;
    public Animator animator;
    private float labelHeight = -1;
    public ConfirmationPopupAnswer confirmationPopupAnswer;

    void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetInteractable(bool interactable)
    {
        label.enabled = interactable;
    }

    public void SetupChoose(ChooseScene scene, bool isQuiz=false)
    {
        DestroyLabels();
        animator.SetTrigger("Show");

        // Shuffle the order of labels
        if (isQuiz)
        {
            scene.ShuffleLabels();
            Debug.Log("Truelaleyyy");
        }

        for (int index = 0; index < scene.labels.Count; index++)
        {
            ChooseLabelController newLabel = Instantiate(label.gameObject, transform).GetComponent<ChooseLabelController>();
            if (labelHeight == -1)
            {
                labelHeight = newLabel.GetHeight();
            }

            newLabel.Setup(scene.labels[index], this, confirmationPopupAnswer, CalculateLabelPosition(index, scene.labels.Count));
        }

        Vector2 size = rectTransform.sizeDelta;
        size.y = (scene.labels.Count + 2) * labelHeight;
        rectTransform.sizeDelta = size;
    }

    public void PerformChoose(StoryScene scene)
    {
        visualManager.PlayScene(scene);
        animator.SetTrigger("Hide");
    }

    private float CalculateLabelPosition(int labelIndex, int labelCount)
    {
        if (labelCount % 2 == 0)
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex - 1) + labelHeight / 2;
            }
            else
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2) + labelHeight / 2);
            }
        }
        else
        {
            if (labelIndex < labelCount / 2)
            {
                return labelHeight * (labelCount / 2 - labelIndex);
            }
            else if (labelIndex > labelCount / 2)
            {
                return -1 * (labelHeight * (labelIndex - labelCount / 2));
            }
            else
            {
                return 0;
            }
        }
    }

    public void DestroyLabels()
    {
        for (int i = 1; i < this.gameObject.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void ShowLabel(bool isChoosen)
    {
        for (int i = 1; i < this.gameObject.transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            transform.GetChild(i).gameObject.SetActive(isChoosen);
        }
    }
}
