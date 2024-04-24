using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stone : MonoBehaviour, IInteractable
{
    private Vector3 _initialPosition;
    private Vector3 _stoneDirection;
    private float _stoneMoveOffset = 8;
    private GameObject _player;

    [SerializeField] private string _prompt;
    public string interactionPrompt => _prompt;

    private ThirdPersonAnimation _thirdPersonAnimation => ThirdPersonAnimation.I;
    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;

    void Awake()
    {
        _initialPosition = transform.position;
        _player = Player.I.gameObject;
    }


    #region Define directions
    private float GetTurnAngles()
    {
        Vector3 inverseTransform = _player.transform.InverseTransformPoint(this.transform.position);

        if (inverseTransform.x < 0)
        {
            if (Mathf.Abs(inverseTransform.x) > Mathf.Abs(inverseTransform.z))
            {
                return -90;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            if (Mathf.Abs(inverseTransform.x) > Mathf.Abs(inverseTransform.z))
            {
                return 90;
            }
            else
            {
                return 0;
            }
        }
    }

    private float SnapPlayerDirection(float angleStep = 90)
    {
        float yRotation = _player.transform.rotation.eulerAngles.y;
        yRotation = (float)Mathf.RoundToInt(yRotation / angleStep) * angleStep;
        return yRotation;
    }

    #endregion

    #region IInteractable
    public bool CanInteract()
    {
        _stoneDirection = _player.transform.position - this.transform.position;

        // Z Axis
        if (Mathf.Abs(_stoneDirection.x) > Mathf.Abs(_stoneDirection.z))
        {
            if (_stoneDirection.x > 0)
            {
                _stoneDirection = new Vector3(-1, 0, 0);
            }
            else
            {
                _stoneDirection = new Vector3(1, 0, 0);
            }
        }
        // X Axis
        else if (Mathf.Abs(_stoneDirection.z) > Mathf.Abs(_stoneDirection.x))
        {
            if (_stoneDirection.z > 0)
            {
                _stoneDirection = new Vector3(0, 0, -1);
            }
            else
            {
                _stoneDirection = new Vector3(0, 0, 1);
            }
        }
        else
        {
            _stoneDirection = new Vector3(0, 0, 0);
        }

        Debug.DrawRay(this.transform.position, _stoneDirection * 10, Color.green);
        return !Physics.Raycast(this.transform.position, _stoneDirection, out RaycastHit meuRay, 10f);
    }

    public void InteractControl(Interactor interactor)
    {
        StartInteract();
    }

    private void StartInteract()
    {
        _thirdPersonController.DisableInputs();
        StartCoroutine(TurnToInteractable());
    }

    public IEnumerator TurnToInteractable()
    {
        float turnAngles = GetTurnAngles();
        float direction = SnapPlayerDirection();
        direction += turnAngles;

        Quaternion quatAngle = Quaternion.AngleAxis(direction, Vector3.up * Time.deltaTime);

        while (_player.transform.rotation.eulerAngles.y != direction)
        {
            _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, quatAngle, Time.deltaTime * 100);
            yield return null;
        }

        FinishInteract();

        yield return null;
    }

    public void FinishInteract()
    {
        _thirdPersonAnimation.SetBool("Pushing", true);

        this.transform.DOMove(this.transform.position + _stoneDirection * _stoneMoveOffset, 5);
        _player.transform.DOMove(_player.transform.position + _stoneDirection * _stoneMoveOffset, 5).OnComplete(() => {
           _thirdPersonAnimation.SetBool("Pushing", false);
            _thirdPersonController.EnableInputs();
        });
    }

    #endregion

    #region Set
    public void SetPosition()
    {
        transform.position = _initialPosition;
    }

    #endregion
}