using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class ThirdPersonAnimation : Singleton<ThirdPersonAnimation>
{
    private Animator animator;
    private Rigidbody rb;
    private float maxSpeed;

    private new void Awake()
    {
        animator = this.GetComponentInChildren<Animator>();
        rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxSpeed = GetComponent<ThirdPersonController>().maxRunSpeed;
    }

    private void Update()
    {
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
    }

    public void SetTrigger(string triggerName)
    {
        rb.velocity = Vector3.zero;
        animator.SetTrigger(triggerName);
    }
}