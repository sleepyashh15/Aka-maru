using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeStageCleared : MonoBehaviour
{
    PlayerParty _playerParty;
    // Start is called before the first frame update
    void Awake()
    {
      //  portals = this.GetComponentsInChildren<Portal>();
    }

    Player _player;
    public void SetData(Player playerx)
    {
        //  stageCount = _player.stageCount;
        //  playerx = GetComponent<Player>();
        _player = playerx;
    }

    public void Portals(PlayerParty playerParty)
    {
       
           SetData(_playerParty.GetDefaultPlayer());
        
    }
    // Update is called once per frame
   
}
