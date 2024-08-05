using UnityEngine;
using Utils.Singleton;

/// <summary>
/// Controls all player animation
/// </summary>
public class ThirdPersonAnimation : Singleton<ThirdPersonAnimation>
{
    private Animator _animator; // Player animator component
    private Rigidbody _rb; // Player rigidbody component
    private float _maxSpeed; // Max speed player can reach
    private bool _isGrounded = true; // Boolean value to be updated on if player is on the ground or not
    private Collider[] _colliders = new Collider[10]; // Array of colliders to catch what the player's feet collider picks uo

    [SerializeField] private float _feetPointRadius = 0.6f; // Radius size for the player's feet collider
    [SerializeField] private LayerMask _feetMask; // Mask of layers the feet collider can collide with

    /// <summary>
    /// Rewrites singleton Awake and gets inicial player components
    /// </summary>
    private new void Awake()
    {
        GetAnimator();
        _rb = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Get the max speed allowed for the player 
    /// </summary>
    private void Start()
    {
        _maxSpeed = GetComponent<ThirdPersonController>().maxRunSpeed;
    }

    /// <summary>
    /// Every physics frame, creates feet collider and gets objects it collides 
    /// with to determine if character if falling or not (on the ground or not) and
    /// the movement animation if is running, walking or in the idle state
    /// </summary>
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
    /// <summary>
    /// Method for activating a trigger animation
    /// </summary>
    public void SetTrigger(string triggerName)
    {
        _rb.velocity = Vector3.zero;
        _animator.SetTrigger(triggerName);
    }
    /// <summary>
    /// Method for activating a boolean animation
    /// </summary>
    public void SetBool(string boolName, bool state)
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool(boolName, state);
    }
    #endregion

    /// <summary>
    /// OnDrawGizmos method to draw the feet collider in scene
    /// </summary>
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, _feetPointRadius);
    //}

    /// <summary>
    /// Gets the player animator component
    /// </summary>
    public void GetAnimator()
    {
        _animator = this.GetComponentInChildren<Animator>();
    }

}