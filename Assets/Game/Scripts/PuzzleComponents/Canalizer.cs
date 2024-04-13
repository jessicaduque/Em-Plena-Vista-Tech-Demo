using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizer : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string interactionPrompt => _prompt;

    #region Interaction
    public bool CanInteract()
    {
        return true;
    }

    public void InteractControl(Interactor interactor)
    {
        StartCoroutine(TurnToInteractable());
    }

    public IEnumerator TurnToInteractable()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
