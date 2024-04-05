using System.Collections;

public interface IInteractable
{ 
    public string interactionPrompt { get; }
    public bool CanInteract();
    public void InteractControl(Interactor interactor);
    public IEnumerator TurnToInteractable();
}
