using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IInteractable
{
    private Vector3 _initialPosition;
    private GameObject _player;

    [SerializeField] private string _prompt;
    public string interactionPrompt => _prompt;

    void Awake()
    {
        _initialPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = _initialPosition;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(Interactor interactor)
    {
        Debug.Log("Stone push");
    }
}