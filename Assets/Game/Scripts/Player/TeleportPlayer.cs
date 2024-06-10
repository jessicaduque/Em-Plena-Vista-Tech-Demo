using System.Collections;
using UnityEngine;

/// <summary>
/// Controls general teleportation for the player
/// </summary>
public class TeleportPlayer : MonoBehaviour
{
    protected GameObject _player; // Player gameobject

    [SerializeField] protected Vector3 _finalPosition; // Position for player to be teleported to
    [SerializeField] protected Vector3 _lookingDirection; // Direction player should be facing when teleported

    protected ThirdPersonController _thirdPlayerController => ThirdPersonController.I; // Gets the player's third person controller script instance
    private BlackScreenController _blackScreenController => BlackScreenController.I; // Gets the UI black screen controller script instance

    /// <summary>
    /// Gets player gameobject
    /// </summary>
    private void Start()
    {
        _player = Player.I.gameObject;
    }

    /// <summary>
    /// If trigger activated is from the player, handles the configured teleportation
    /// </summary>
    /// <param name="collision">Collision detected from other object</param>
    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(HandleTeleport());
        }
    }
    /// <summary>
    /// Handles the teleportation for the player, blackening the screen and disabling its controls 
    /// before changing its position and rotation to the new place
    /// </summary>
    protected virtual IEnumerator HandleTeleport()
    {
        _thirdPlayerController.DisableInputs();
        _blackScreenController.FadeInBlack();

        while (!_blackScreenController.GetBlackScreenOn())
        {
            yield return null;
        }

        _player.transform.position = _finalPosition;
        _player.transform.localEulerAngles = _lookingDirection;

        _blackScreenController.FadeOutBlack();

        while (!_blackScreenController.GetBlackScreenOff())
        {
            yield return null;
        }

        TeleportFinish();
    }
    /// <summary>
    /// Method that runs at the end of the player teleportation
    /// </summary>
    protected virtual void TeleportFinish()
    {
        _thirdPlayerController.EnableInputs();
    }
}