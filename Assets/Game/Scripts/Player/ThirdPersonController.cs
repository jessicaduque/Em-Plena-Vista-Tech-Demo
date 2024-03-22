using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : Utils.Singleton.Singleton<ThirdPersonController>
{
    // Input fields
    private ThirdPersonActionsAsset playerActionsAsset;
    private InputAction move;

    // Movement fields
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxWalkSpeed = 4f;
    public float maxRunSpeed = 8f;
    private float maxFinalSpeed = 4f;
    [SerializeField] private Vector3 forceDirection = Vector3.zero;

    // Puzzle fields
    [SerializeField] private float pushStoneTime = 2f;
    [SerializeField] private float interactionDistance = 1f;

    [SerializeField] private Camera playerCamera;

    private Player _player;
    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;

    private new void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerActionsAsset = new ThirdPersonActionsAsset();
    }

    private void OnEnable()
    {
        playerActionsAsset.Player.ResetPuzzle.started += DoResetPuzzle;
        playerActionsAsset.Player.Interact.started += DoInteractControl;
        playerActionsAsset.Player.Run.started += StartRun;
        playerActionsAsset.Player.Run.canceled += EndRun;

        move = playerActionsAsset.Player.Move;
        playerActionsAsset.Player.Enable();
    }

    private void OnDisable()
    {
        playerActionsAsset.Player.ResetPuzzle.started -= DoResetPuzzle;
        playerActionsAsset.Player.Interact.started -= DoInteractControl;
        playerActionsAsset.Player.Run.started -= StartRun;
        playerActionsAsset.Player.Run.canceled -= EndRun;

        playerActionsAsset.Player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > maxFinalSpeed * maxFinalSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxFinalSpeed + Vector3.up * rb.velocity.y;
        }

        LookAtWithCamera();
    }

    #region Camera

    private void LookAtWithCamera()
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

    #region Movement

    private void StartRun(InputAction.CallbackContext obj)
    {
        maxFinalSpeed = maxRunSpeed;
    }

    private void EndRun(InputAction.CallbackContext obj)
    {
        maxFinalSpeed = maxWalkSpeed;
    }

    #endregion


    private void DoInteractControl(InputAction.CallbackContext obj)
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 2f, this.transform.forward);
        Debug.DrawRay(this.transform.position + Vector3.up * 2f, this.transform.forward * interactionDistance, Color.red, 2);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            Transform objTransform = hit.transform;
            if (hit.transform.CompareTag("Stone"))
            {
                Debug.Log("Stone hit");
                StartCoroutine(_player.LookAtObject(objTransform));
                // Script to get stone and push
            }
            else if (hit.transform.CompareTag("Canalizer"))
            {
                Debug.Log("Canalizer hit");
                StartCoroutine(_player.LookAtObject(objTransform));
                // Script to get canalizer and canalize
            }
        }
    }

    #region Puzzle

    private void DoResetPuzzle(InputAction.CallbackContext obj)
    {
        if (IsInPuzzle())
        {
            _stonePuzzleManager.ResetStonePuzzle();
        }
    }

    private bool IsInPuzzle()
    {
        return _player.isInPuzzle;
    }

    #endregion 
}