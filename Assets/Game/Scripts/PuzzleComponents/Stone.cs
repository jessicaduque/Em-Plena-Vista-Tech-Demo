using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IInteractable
{
    private Vector3 _initialPosition;
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

    public void ResetPosition()
    {
        transform.position = _initialPosition;
    }

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

    #region IInteractable
    public bool CanInteract()
    {
        return true;
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

        while (_player.transform.rotation.eulerAngles.y != direction)
        {
            _player.transform.rotation = Quaternion.AngleAxis(direction, Vector3.up * Time.deltaTime);
            yield return null;
        }

        //_thirdPersonAnimation.SetBool("Pushing", true);

        _thirdPersonController.EnableInputs();
        yield return null;
    }

    private float SnapPlayerDirection(float angleStep = 90) 
    { 
        float yRotation = _player.transform.rotation.eulerAngles.y; 
        yRotation = (float)Mathf.RoundToInt(yRotation / angleStep) * angleStep;
        return yRotation;
    }

    public void FinishInteract()
    {

    }

    #endregion
}