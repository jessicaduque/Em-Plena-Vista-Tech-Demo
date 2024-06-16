using UnityEngine;
/// <summary>
/// Script placed on players body, for access to its animator component
/// </summary>
public class PlayerBody : MonoBehaviour
{
    private ThirdPersonController _thirdPersonController => ThirdPersonController.I; // Gets the player third person controller script instance
    /// <summary>
    /// Method called from end of animation events to enable the player's inputs 
    /// </summary>
    public void EnableInputs()
    {
        _thirdPersonController.EnableInputs();
    }
}