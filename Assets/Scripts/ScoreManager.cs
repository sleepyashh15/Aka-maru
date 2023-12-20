
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;
    [SerializeField]
    private GameObject leaderboard;

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
        Debug.Log("submit naman dapat "+ inputName.text + " tas eto : "+int.Parse(inputScore.text).ToString());
    }


}
