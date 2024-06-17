using System.Collections;
using UnityEngine;
using Utils.Singleton;

public class ThirdPersonAnimation : Singleton<ThirdPersonAnimation>
{
    private Animator _animator;
    private Rigidbody _rb;
    private float _maxSpeed;
    private bool _isGrounded = true;

    private new void Awake()
    {
        _animator = this.GetComponentInChildren<Animator>();
        _rb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _maxSpeed = GetComponent<ThirdPersonController>().maxRunSpeed;
    }

    private void FixedUpdate()
    {
        if (_isGrounded)
            _animator.SetFloat("speed", _rb.velocity.magnitude / _maxSpeed);
    }
    
    #region Trigger colliders
    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        SetBool("Falling", false);
        _isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!_isGrounded)
        {
            StopAllCoroutines();
            SetBool("Falling", false);
            _isGrounded = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(MinTimeForNotGrounded());
    }
    #endregion

    private IEnumerator MinTimeForNotGrounded()
    {
        _isGrounded = false;
        yield return new WaitForSeconds(0.2f);

        SetBool("Falling", true);
    }


    #region Set Animation Parameters
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
    #endregion
}