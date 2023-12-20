using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalMenu : MonoBehaviour
{
    private Portal[] portals;
    PlayerParty _playerParty;
    // Start is called before the first frame update
    void Start()
    {
       portals = this.GetComponentsInChildren<Portal>();
    }

    public void Portals(PlayerParty playerParty)
    {
        _playerParty = playerParty;

        if (portals == null) return;

        foreach (Portal portal in portals)
        {
            portal.SetData(_playerParty.GetDefaultPlayer());
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
