public interface IInteractable
{ 
    public string interactionPrompt { get; }
    public bool CanInteract();
    public void Interact(Interactor interactor);
}
