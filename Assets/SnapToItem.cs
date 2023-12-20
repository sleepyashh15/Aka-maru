using JetBrains.Annotations;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SnapToItem : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;

    public VerticalLayoutGroup VLG;

    public TextMeshProUGUI DescLabel;
    public string[] ItemDesc;
    private Animator animator;
    private bool isSnapped;
    public float snapForce;
    private float snapSpeed;
      private bool isCoroutineRunning;

    void Start()
    {
        isSnapped = false;
        animator = GetComponent<Animator>();
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }


    public void OnClickScrollTo(RectTransform target) {

        Canvas.ForceUpdateCanvases();
        Vector2 viewPortLocalPosition = scrollRect.viewport.localPosition;
        Vector2 targetLocalPosition = target.localPosition;

       
        contentPanel.localPosition = new Vector2(
            0 - (viewPortLocalPosition.x + targetLocalPosition.x),
            //  Mathf.MoveTowards(contentPanel.localPosition.y, 0 + currentItem * (sampleListItem.rect.height + VLG.spacing), snapSpeed)
            //0 - (viewPortLocalPosition.y + targetLocalPosition.y) + (scrollRect.viewport.rect.height / 2) - (target.rect.height / 2)
            Mathf.MoveTowards(0 - viewPortLocalPosition.y, targetLocalPosition.y, snapSpeed
            
            ));

    }

    void Update()
    {
        int currentItem = Mathf.RoundToInt((0 + contentPanel.localPosition.y) / (sampleListItem.rect.height + VLG.spacing));

        if (!isSnapped && !isCoroutineRunning)
        {
            scrollRect.velocity = Vector2.zero;
            snapSpeed += snapForce * Time.deltaTime;
            contentPanel.localPosition = new Vector3(
                contentPanel.localPosition.x,
                Mathf.MoveTowards(contentPanel.localPosition.y, 0 + currentItem * (sampleListItem.rect.height + VLG.spacing), snapSpeed),
                contentPanel.localPosition.z);

            if (Mathf.Approximately(contentPanel.localPosition.y, 0 + currentItem * (sampleListItem.rect.height + VLG.spacing)))
            {
                StartCoroutine(SnapCoroutine());
            }
        }

        if (currentItem >= 0)
            DescLabel.text = ItemDesc[currentItem];

        
            if (currentItem >= (contentPanel.childCount - 2))
            {
                Debug.Log("Testing bobo");
                OnClickScrollTo((RectTransform)contentPanel.GetChild(2).transform);
            }
        
    }

    public void nextLabelButton()
    {
        Canvas.ForceUpdateCanvases();
        Vector2 viewPortLocalPosition = scrollRect.viewport.localPosition;
        Vector2 targetLocalPosition = contentPanel.GetChild(4).position;
       // Vector2 targetLocalPosition = new Vector2(contentPanel.GetChild(4).position.x, contentPanel.GetChild(4).position.y * 0.90f);

        contentPanel.localPosition = new Vector2(
            0 - (viewPortLocalPosition.x + targetLocalPosition.x),
            //  Mathf.MoveTowards(contentPanel.localPosition.y, 0 + currentItem * (sampleListItem.rect.height + VLG.spacing), snapSpeed)
            //0 - (viewPortLocalPosition.y + targetLocalPosition.y) + (scrollRect.viewport.rect.height / 2) - (target.rect.height / 2)
            Mathf.MoveTowards(0 - viewPortLocalPosition.y, targetLocalPosition.y, snapSpeed

            ));
    }

    IEnumerator SnapCoroutine()
    {
        isCoroutineRunning = true;

        while (snapSpeed > 0.1f)
        {
            snapSpeed = Mathf.Lerp(snapSpeed, 0f, 5f * Time.deltaTime);
            yield return null;
        }

        // Perform additional snapping logic here if needed

        isSnapped = true;
        isCoroutineRunning = false;
    }

    void OnScrollValueChanged(Vector2 value)
    {
        // Reset isSnapped when the user starts scrolling
        isSnapped = false;
    }

    public void testButton()
    {
        Debug.Log("Pressed");
    }

    public void ShowPlayMenu()
    {
        animator.SetTrigger("HideMainMenu");
    }

    public void ShowMainMenu()
    {
        animator.SetTrigger("ShowMainMenu");
    }

    public void HidePlayMenu()
    {
        animator.SetTrigger("HidePlayMenu");
    }

    public void HideMainMenu()
    {
        animator.SetTrigger("HideMainMenu");
    }
}