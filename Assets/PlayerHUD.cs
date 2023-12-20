using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class PlayerHUD : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject expBar;
    [SerializeField] TextMeshProUGUI levelText;
    public float expBarScale = 0;
    // public int level;

    Player _player;
    private void Update()
    {
       // expBarScale = expBar.transform.lossyScale.x;

    }
    private void Start()
    {
        // expBar.transform.lossyScale.x = expBarScale;
        expBar.transform.localScale = new Vector3(0.5f, 1, 1);
    }
    public void SetLevel()
    {
        levelText.text = "" + _player.Level;
        expBar.transform.localScale = new Vector3(0.5f, 1, 1);
    }
    public void SetData(Player player)
    {
        _player = player;

     //   SetExp();
      //  SetLevel();
    }


    public void SetExp()
    {
        if (expBar == null) return;

        float normalizedExp = GetNormalizedExp();
        expBar.transform.localScale = new Vector3 (normalizedExp, 1, 1);
    }

    public IEnumerator SetExpSmooth(bool reset = false)
    {
        if (expBar == null) yield break;

        if (reset)        
            expBar.transform.localScale = new Vector3(0, 1, 1);
        
       float normalizedExp = GetNormalizedExp();
       yield return expBar.transform.DOScaleX(normalizedExp, 1.5f).WaitForCompletion();
    }
   // public DataPlayer _dataPlayer { get; set; }

    float GetNormalizedExp()
    {
        int currLevelExp = _player.Base.GetExpForLevel(_player.Level);
        int nextLevelExp = _player.Base.GetExpForLevel(_player.Level + 1);

        float normalizedExp = (float)(_player.Exp - currLevelExp) / (nextLevelExp - currLevelExp);
        return Mathf.Clamp01(normalizedExp);
    }

    public void LoadData(GameData data)
    {
     //   throw new System.NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        //  data.exp = this.expBarScale;
     //   data.level = int.Parse(levelText.text);

    }
}
