using System.Collections;
/// <summary>
/// Interface for objects that are interactable to the player
/// </summary>
public interface IInteractable
{
    public string interactionPrompt { get; } // Makes communicating specific interactions prompts to the player possible
    /// <summary>
    /// Checks if object's specific conditions are met for the player to be able to interact with it
    /// </summary>
    /// <returns>Returns if interactor can interact with object.</returns>
    public bool CanInteract();
    /// <summary>
    /// Object's intended interaction is executed
    /// </summary>
    /// <param name="interactor">Interactor that's trying to interact with object</param>
    public void InteractControl(Interactor interactor);
    /// <summary>
    /// Makes interactor's object turn towards the interactable object
    /// </summary>
    public IEnumerator TurnToInteractable();
}
