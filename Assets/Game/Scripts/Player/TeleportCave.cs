using System.Collections.Generic;
using UnityEngine;

public class TeleportCave : TeleportPlayer
{
    [SerializeField] private List<GameObject> _stonesToDisable = new List<GameObject>();
    private UIManager _uiManager => UIManager.I;
    private GameController _gameController => GameController.I;


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

    protected override void TeleportFinish()
    {
        if (!_gameController.gameCompleted)
        {
            _thirdPlayerController.EnableInputs();
        }
    }
}