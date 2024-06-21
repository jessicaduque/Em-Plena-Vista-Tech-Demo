using UnityEngine;

public class PlayerInsidePuzzle : MonoBehaviour
{
    [SerializeField] bool _enteringPuzzle;
    [SerializeField] bool _lastPuzzleEnter;

    private Player _player => Player.I;

    private void OnTriggerExit(Collider other)
    {
        _player.SetIsInPuzzle(_enteringPuzzle, _lastPuzzleEnter);
    }
}