using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class ThirdPersonAnimation : Singleton<ThirdPersonAnimation>
{
    private Animator _animator;
    private Rigidbody _rb;
    private float _maxSpeed;

    private new void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _maxSpeed = GetComponent<ThirdPersonController>().maxRunSpeed;
    }

    private void Update()
    {
        _animator.SetFloat("speed", _rb.velocity.magnitude / _maxSpeed);
    }

    public void SetTrigger(string triggerName)
    {
        _rb.velocity = Vector3.zero;
        _animator.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool state)
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool(boolName, state);
    }
}