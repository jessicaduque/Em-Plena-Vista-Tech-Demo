using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInsidePuzzle : MonoBehaviour
{
    [SerializeField] bool enteringPuzzle;

    private Player _player => Player.I;

    private void OnTriggerExit(Collider other)
    {
        _player.SetIsInPuzzle(enteringPuzzle);
    }
}