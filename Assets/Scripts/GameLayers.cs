using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{

    [SerializeField] LayerMask solidObjectLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask portalLayer;
    [SerializeField] LayerMask playerLayer;

    private void Start()
    {
        i = this;
    }
    public static GameLayers i { get; set; }

    public LayerMask PortalLayer
    {
        get => portalLayer;
    }

    public LayerMask InteractableLayer
    {
        get => interactableLayer;
    }

    public LayerMask SolidObjectLayer
    {
        get => solidObjectLayer;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }

}
