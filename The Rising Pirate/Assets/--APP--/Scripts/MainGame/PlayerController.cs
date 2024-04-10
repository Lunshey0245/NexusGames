using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    /// <summary>
    ///  FUSION
    /// </summary>
    /// 
    private float vertical;
    private float horizontal;

    [Header("Control")]
    [SerializeField, FormerlySerializedAs("EnginePower")]
    public float _enginePower = 15;
    [SerializeField, FormerlySerializedAs("TurnPower")]
    float _turnPower = 0.5f;
    [SerializeField]
    public bool _playerControlled = true;

    [Tooltip("Used to automatically add throttle input")]
    float _engineBias = 0f;
    [Tooltip("Used to automatically add turning input")]
    float _turnBias = 0f;

    Rigidbody _rb;
    float forward;

    [Range(0, 1)]
    float _turningHeel = 0.35f;

    [Networked]private NetworkButtons buttonsPrev {  get; set; } 
    public enum PlayerInputButtons
    {
        none,
        Left,
        Right,
    }

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void BeforeUpdate()
    {
        //WE ARE THE LOCAL MACHINE
        if (Runner.LocalPlayer == Object.HasInputAuthority)
        {
            const string VERTICAL = "Vertical";
            vertical = Input.GetAxisRaw(VERTICAL);

            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxisRaw(HORIZONTAL);
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input))
        {
            float speed = _rb.velocity.magnitude / 1.85f;
            
            var forcePosition = _rb.position;

            forward = _engineBias;
            forward += input.VerticalInput;
            
            _rb.AddForceAtPosition(transform.forward * _enginePower * forward, forcePosition, ForceMode.Acceleration);
            CheckInputTurn(input);
        }
    }


    void CheckInputTurn(PlayerData input)
    {
        var pressed = input.NetworkButtons.GetPressed(buttonsPrev);
        var sideways = _turnBias;
        //sideways += (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Left) ? -1f : 0f) + (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Right) ? 1f : 0f);
       
        //sideways += (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Left) ? -1f : 0f) + (pressed.WasPressed(buttonsPrev, PlayerInputButtons.Right) ? 1f : 0f);
        if (input.HorizontalInput < 0)
        {
            sideways = -1;
        }
        if (input.HorizontalInput == 0)
        {
            sideways = 0;
        }
        if (input.HorizontalInput > 0)
        {
            sideways = 1;
        }
        var rotVec = transform.up + _turningHeel * transform.forward;
        _rb.AddTorque(rotVec * _turnPower * sideways, ForceMode.Acceleration);

        buttonsPrev = input.NetworkButtons;
    }

    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data= new PlayerData();
        data.VerticalInput = vertical;
        data.HorizontalInput = horizontal;

        //data.NetworkButtons.Set(PlayerInputButtons.Left, Input.GetKey(KeyCode.A));
       // data.NetworkButtons.Set(PlayerInputButtons.Right, Input.GetKey(KeyCode.D)); 
        return data;
    }

}
