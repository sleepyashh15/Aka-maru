using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
    private Animator animator;
    public GameObject MapMenu;
    public GameObject roadmaps;
    PlayerParty _playerParty;
  //  public int stageIndex = -1;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Roadmaps(PlayerParty playerParty)
    {
        _playerParty = playerParty;

      //  if (portals == null) return;
       SetData(_playerParty.GetDefaultPlayer());
        
    }
    Player _player;
    public void SetData(Player playerx)
    {
        //  stageCount = _player.stageCount;
        //  playerx = GetComponent<Player>();
        _player = playerx;
    }
    public void StageUnlock()
    {
        // for(int i = 0; i <= roadmaps.)
        
        for (int i = 2; i < roadmaps.transform.childCount; i++)
        {
          //  Debug.Log(i + "+++");
              Transform child = roadmaps.transform.GetChild(i);
            //  child.transform.gameObject.SetActive(true);

         //   if (_player.stageCount >= stageIndex)
          //  {
                ////   for (int x = 0; x < stageIndex - 1; i++)
                //     {
                //if (portals == null) return;
                for (int x = 2; x < _player.stageCount + 2; x++)
                {
                  //  Debug.Log("eto dapat stage lang" + x);
                    roadmaps.transform.GetChild(x).GetChild(0).gameObject.SetActive(true);
                    roadmaps.transform.GetChild(x).GetChild(1).gameObject.SetActive(false);
                }
               // }
            //}
        }
    }

    public void ShowMapandHelp()
    {
       // MapMenu.SetActive(true);
        animator.SetTrigger("ShowMapandHelp");
       // StartCoroutine(RoadmapShowAndHide(true));
    }

    public void HideMapandHelp()
    {
        animator.SetTrigger("HideMapandHelp");
        //  Roadmap.SetActive(false);
        //StartCoroutine(RoadmapShowAndHide(false));
    }

    IEnumerator RoadmapShowAndHide(bool isTrue)
    {     

        yield return new WaitForSeconds(0.30f);
        MapMenu.SetActive(isTrue);
    }

}
