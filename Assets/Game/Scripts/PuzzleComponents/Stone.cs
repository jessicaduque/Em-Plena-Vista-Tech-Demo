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

    private Vector3 GetTurnDirection()
    {
        Vector3 inverseTransform = _player.transform.InverseTransformPoint(this.transform.position);

        if (inverseTransform.x < 0)
        {
            if (Mathf.Abs(inverseTransform.x) > Mathf.Abs(inverseTransform.z))
            {
                return -_player.transform.right;
            }
            else
            {
                if (inverseTransform.z > 0)
                {
                    return _player.transform.forward;
                }
                else
                {
                    return -_player.transform.forward;
                }
            }
        }
        else
        {
            if (Mathf.Abs(inverseTransform.x) > Mathf.Abs(inverseTransform.z))
            {
                return _player.transform.right;
            }
            else
            {
                if (inverseTransform.z > 0)
                {
                    return _player.transform.forward;
                }
                else
                {
                    return -_player.transform.forward;
                }
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

        Debug.Log(GetTurnDirection());
        //while (_player.transform.rotation.eulerAngles.y != inverseTransform)
        //{
        //    this.transform.localRotation = Quaternion.AngleAxis(yRotation, Vector3.up * Time.deltaTime);
        //    yield return null;
        //}

        //_thirdPersonAnimation.SetBool("Pushing", true);

        _thirdPersonController.EnableInputs();
        yield return null;
    }

    public void FinishInteract()
    {

    }

    #endregion
}