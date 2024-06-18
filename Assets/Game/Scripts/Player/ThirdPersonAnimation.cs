using System.Collections;
using UnityEngine;
using Utils.Singleton;

public class ThirdPersonAnimation : Singleton<ThirdPersonAnimation>
{
    private Animator _animator;
    private Rigidbody _rb;
    private float _maxSpeed;
    private bool _isGrounded = true;
    private Collider[] _colliders = new Collider[10];

    [SerializeField] private float _feetPointRadius = 0.6f;
    [SerializeField] private LayerMask _feetMask;

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
        bool newIsGrounded = Physics.OverlapSphereNonAlloc(transform.position, _feetPointRadius, _colliders, _feetMask) > 0;
        if (!_isGrounded && newIsGrounded)
        {
            SetBool("Falling", false);
        }
        else if(_isGrounded && !newIsGrounded)
        {
            SetBool("Falling", true);
        }
        
        _isGrounded = newIsGrounded;

        if (_isGrounded)
            _animator.SetFloat("speed", _rb.velocity.magnitude / _maxSpeed);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _feetPointRadius);
    }

}