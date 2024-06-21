using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : Utils.Singleton.Singleton<ThirdPersonController>
{
    // Input fields
    private ThirdPersonActionsAsset _playerActionsAsset;
    private InputAction _move;

    // Movement fields
    private Rigidbody _rb;
    public float maxRunSpeed = 8f;
    private float _maxFinalSpeed = 4f;
    [SerializeField] private float _movementForce = 1f;
    [SerializeField] private float _maxWalkSpeed = 4f;
    [SerializeField] private Vector3 _forceDirection = Vector3.zero;
    [SerializeField] private float _lookAtSpeed = 10f;

    // Puzzle fields
    private Interactor _interactor;
    private bool _canInteract = true;

    [SerializeField] private Camera _playerMainCamera;
    [SerializeField] private bool _puzzleCameraMovement;

    private Player _player => Player.I;
    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;

    private new void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
        _interactor = this.GetComponent<Interactor>();
        _playerActionsAsset = new ThirdPersonActionsAsset();

        _move = _playerActionsAsset.Player.Move;
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    private void FixedUpdate()
    {
        if (_puzzleCameraMovement)
            PuzzleMovement();
        else
            MainMovement();
    }

    #region Input

    public void EnableInputs()
    {
        _playerActionsAsset.Player.ResetPuzzle.started += DoResetPuzzle;
        _playerActionsAsset.Player.Interact.started += DoInteractControl;
        _playerActionsAsset.Player.Run.started += StartRun;
        _playerActionsAsset.Player.Run.canceled += EndRun;
        _maxFinalSpeed = _maxWalkSpeed;

        _interactor.ControlActivationInteractor(true);

        _playerActionsAsset.Player.Enable();
    }

    public void DisableInputs()
    {
        _playerActionsAsset.Player.ResetPuzzle.started -= DoResetPuzzle;
        _playerActionsAsset.Player.Interact.started -= DoInteractControl;
        _playerActionsAsset.Player.Run.started -= StartRun;
        _playerActionsAsset.Player.Run.canceled -= EndRun;

        _interactor.ControlActivationInteractor(false);

        _playerActionsAsset.Player.Disable();
    }

    #endregion

    #region Camera

    private void LookAtWithCamera()
    {
        Vector3 direction = _rb.velocity;
        direction.y = 0;

        if (_move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this._rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            _rb.angularVelocity = Vector3.zero;
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

    private void MainMovement()
    {
        _forceDirection += _move.ReadValue<Vector2>().x * GetCameraRight(_playerMainCamera) * _movementForce;
        _forceDirection += _move.ReadValue<Vector2>().y * GetCameraForward(_playerMainCamera) * _movementForce;

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        if (_rb.velocity.y < 0f)
            _rb.velocity -= Vector3.down * Physics.gravity.y * 4 * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > _maxFinalSpeed * _maxFinalSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * _maxFinalSpeed + Vector3.up * _rb.velocity.y;
        }

        LookAtWithCamera();
    }

    private void PuzzleMovement() 
    {
        _forceDirection += _move.ReadValue<Vector2>().x * Vector3.left * _movementForce;
        _forceDirection += _move.ReadValue<Vector2>().y * Vector3.back * _movementForce;

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        if (_rb.velocity.y < 0f)
            _rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > _maxFinalSpeed * _maxFinalSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * _maxFinalSpeed + Vector3.up * _rb.velocity.y;
        }

        LookAtWithCamera();
    }

    private void StartRun(InputAction.CallbackContext obj)
    {
        _maxFinalSpeed = maxRunSpeed;
    }

    private void EndRun(InputAction.CallbackContext obj)
    {
        _maxFinalSpeed = _maxWalkSpeed;
    }

    public IEnumerator LookAtObject(Transform obj)
    {
        DisableInputs();

        Vector3 relativePos = obj.transform.position - transform.position;
        relativePos.y = 0;

        Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);

        var deltaAngle = Quaternion.Angle(this._rb.rotation, rot);

        while (deltaAngle != 0)
        {
            deltaAngle = Quaternion.Angle(this._rb.rotation, rot);
            this._rb.rotation = Quaternion.Slerp(transform.rotation, rot, _lookAtSpeed * Time.deltaTime);
            yield return null;
        }

        EnableInputs();

    }

    #endregion

    #region Puzzle


    private void DoResetPuzzle(InputAction.CallbackContext obj)
    {
        if (_player.isInPuzzle && _stonePuzzleManager.GetLastCheckpointTransform() != null)
        {
            StartCoroutine(_stonePuzzleManager.ResetStonePuzzle());
        }
    }

    #endregion

    #region Interaction
    private void DoInteractControl(InputAction.CallbackContext obj)
    {
        if (_canInteract)
        {
            _interactor.InteractControl();
            StartCoroutine(InteractCooldown());
            _canInteract = false;
        }
        
    }

    private IEnumerator InteractCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _canInteract = true;
    }

    #endregion

    #region SET

    public void SetPuzzleCameraMovement(bool state)
    {
        _puzzleCameraMovement = state;
    }

    #endregion
}