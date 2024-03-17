using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    // Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    // Movement fields
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private Vector3 forceDirection = Vector3.zero;

    // Puzzle fields
    [SerializeField] private float pushStoneTime = 2f;

    [SerializeField] private Camera playerCamera;

    private Player _player;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.ResetPuzzle.started += DoResetPuzzle;
        playerActionsAsset.Player.Interact.started += DoInteractControl;
        
        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.ResetPuzzle.started -= DoResetPuzzle;
        playerActionsAsset.Player.Interact.started -= DoInteractControl;

        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;
    
        if(rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;


        LookAt();
    }

    #region Camera

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    #endregion

    private void DoInteractControl(InputAction.CallbackContext obj)
    {
        Ray ray = new Ray(this.transform.position + Vector3.right * 0.25f, Vector3.right);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            if (hit.transform.CompareTag("Stone"))
            {
                // Script to get stone and push
            }
            else if (hit.transform.CompareTag("Canalizer"))
            {
                // Script to get canalizer and canalize
            }
        }
    }

    #region Puzzle

    private void DoResetPuzzle(InputAction.CallbackContext obj)
    {
        if (IsInPuzzle())
        {
            // Script to reset puzzle
        }
    }

    private bool IsInPuzzle()
    {
        return _player.isInPuzzle;
    }

    #endregion 
}
