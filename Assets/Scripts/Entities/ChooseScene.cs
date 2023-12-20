using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose Scene")]
[System.Serializable]
public class ChooseScene : GameScene
{
    public List<ChooseLabel> labels;

    [System.Serializable]
    public struct ChooseLabel
    {
        public string text;
        public StoryScene nextScene;
    }

    public void ShuffleLabels()
    {
        for (int i = labels.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            ChooseLabel temp = labels[i];
            labels[i] = labels[j];
            labels[j] = temp;
        }
    }
}
