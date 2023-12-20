using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject expBar;


    Player _player;
    public void SetData(Player player)
    {
        _player = player;
        nameText.text = player.Base.Name + "("+player.Username+")";
        // expBar.transform.localScale = new Vector3(0.5f, 1, 1);
        // levelText.text = "Lvl" + player.Level;
      
        SetExp();
        SetLevel();
        //
        //   Debug.Log("Nalagay aman HUD");
    }

    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        //Debug.Log(normalizedExp+" bobo neto");
        expBar.transform.localScale = new Vector3(normalizedExp, 1, 1);
    }
    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if (reset)
            expBar.transform.localScale = new Vector3(0, 1, 1);

        float normalizedExp = GetNormalizedExp();
        yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }

    float GetNormalizedExp()
    {
        int currLevelExp = _player.Base.GetExpForLevel(_player.Level);
        int nextLevelExp = _player.Base.GetExpForLevel(_player.Level + 1);

        float normalizedExp = (float) (_player.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public void SetLevel()
    {
        levelText.text = "" + _player.Level;
    }
}
