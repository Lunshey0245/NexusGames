using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShipMovement : NetworkBehaviour
{

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
    public float _turnBiasInitial;

    Rigidbody _rb;
    float forward;

    [Range(0, 1)]
    float _turningHeel = 0.35f;


    float engineHalf;
    float engineNormal;

    float _turnBiasPowered;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        engineHalf = _enginePower / 2;
        engineNormal = _enginePower;
        _turnBiasPowered = _turnPower * 2;
        _turnBiasInitial = _turnPower;
    }

    // Update is called once per frame
    void Update()
    {

        float speed = _rb.velocity.magnitude / 1.85f;
    }
    public override void FixedUpdateNetwork()
    {
        FixedUpdateEngine();
    }
    /*private void FixedUpdate()
    {

        FixedUpdateEngine();
    }*/

    void FixedUpdateEngine()
    {
        var forcePosition = _rb.position;

        forward = _engineBias;
        if (_playerControlled) forward += Input.GetAxis("Vertical");
        _rb.AddForceAtPosition(transform.forward * _enginePower * forward, forcePosition, ForceMode.Acceleration);

        var sideways = _turnBias;
        if (_playerControlled) sideways += (Input.GetKey(KeyCode.A) ? -1f : 0f) + (Input.GetKey(KeyCode.D) ? 1f : 0f);
        var rotVec = transform.up + _turningHeel * transform.forward;
        _rb.AddTorque(rotVec * _turnPower * sideways, ForceMode.Acceleration);
    }

    public void NormalTurnBias()
    {
        _turnPower = _turnBiasInitial;
    }

    public void TurnBiasPowered()
    {
        _turnPower = _turnBiasPowered;
    }

    public void InAttackMode()
    {
        _enginePower = engineHalf;
    }
    public void ExitAttackMode()
    {
        _enginePower = engineNormal;
    }
}
