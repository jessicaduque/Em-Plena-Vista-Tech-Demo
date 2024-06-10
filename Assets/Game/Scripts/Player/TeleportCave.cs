using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from TeleportPlayer, controls teleportation for the player when it enters the cave from either end
/// </summary>
public class TeleportCave : TeleportPlayer
{
    [SerializeField] private List<GameObject> _stonesToDisable = new List<GameObject>(); // List of stones in front of one cave's entrance 
    [SerializeField] private bool _isBottomCaveEntrance;
    private UIManager _uiManager => UIManager.I; // Gets the UI Manager script instance
    private GameController _gameController => GameController.I; // Gets the overall game controller script instance

    /// <summary>
    /// If trigger activated is from the player, handles the configured teleportation, 
    /// and if game hasn't been completed yet, enables game end UI panel
    /// </summary>
    /// <param name="collision">Collision detected from other object</param>
    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(HandleTeleport());
          
            if (!_gameController.gameCompleted)
            {
                _uiManager.ControlEndPanel(true);

                for (int i = 0; i < _stonesToDisable.Count; i++)
                {
                    if (_stonesToDisable[i] != null)
                    {
                        _stonesToDisable[i].SetActive(false);
                    }
                }
            }
        }
    }
    /// <summary>
    /// When player teleportation finished, if game hasn't been finished 
    /// and player enters the end cave entrance, disables players controls for the end UI stuff to appear
    /// </summary>
    protected override void TeleportFinish()
    {
        if (!_isBottomCaveEntrance && !_gameController.gameCompleted)
        {
            _thirdPlayerController.DisableInputs();
        }
    }
}