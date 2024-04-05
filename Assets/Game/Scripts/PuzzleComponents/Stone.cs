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

    void Awake()
    {
        _initialPosition = transform.position;
        _player = Player.I.gameObject;
    }

    public void ResetPosition()
    {
        transform.position = _initialPosition;
    }

    #region IInteractable
    public bool CanInteract()
    {
        return true;
    }
    public void TurnToInteractable()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Interactor interactor)
    {
        _thirdPersonAnimation.SetBool("Pushing", true);
    }

    #endregion
}