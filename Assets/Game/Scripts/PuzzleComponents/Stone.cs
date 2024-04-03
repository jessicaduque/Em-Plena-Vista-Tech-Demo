using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour, IInteractable
{
    private Vector3 _initialPosition;

    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    void Awake()
    {
        _initialPosition = transform.position;
    }

    public void ResetPosition()
    {
        transform.position = _initialPosition;
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Stone push");
        return true;
    }
}