using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidLand : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; } = Vector2.zero;
   // public bool MoveIsPressed = false;

    public bool InteractIsPressed { get; private set; } = false;

    public Vector2 pressed { get; private set; } = Vector2.zero;


    InputActions _input = null;
    public float timeBetweenSpawns = 0.25f;
    float currentTime;

    private void OnEnable()
    {
        _input = new InputActions();
        _input.humanoidLand.Enable();

        _input.humanoidLand.Move.performed += SetMove;
        _input.humanoidLand.Move.canceled += SetMove;


        _input.humanoidLand.Fire.started += SetInteract;
        _input.humanoidLand.Fire.canceled += SetInteract;
        _input.humanoidLand.Fire.performed += SetInteract;

    }

    private void OnDisable()
    {
        _input.humanoidLand.Move.performed -= SetMove;
        _input.humanoidLand.Move.canceled -= SetMove;

        _input.humanoidLand.Fire.started -= SetInteract;
        _input.humanoidLand.Fire.canceled -= SetInteract;
        _input.humanoidLand.Fire.performed -= SetInteract;

        _input.humanoidLand.Disable();
    }

    private void Update()
    {

    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        //MoveIsPressed = !(MoveInput == Vector2.zero);
    }



    private void SetInteract(InputAction.CallbackContext ctx)
    {
        InteractIsPressed = ctx.performed;

    }
}
