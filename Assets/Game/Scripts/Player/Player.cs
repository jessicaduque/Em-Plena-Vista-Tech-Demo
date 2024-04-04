using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Player : Singleton<Player>
{
    private Rigidbody _rb;

    // Puzzle variables
    public bool isInPuzzle { get; private set; }

    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;

    private new void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    

    #region SET

    public void SetIsInPuzzle(bool isInPuzzle)
    {
        this.isInPuzzle = isInPuzzle;
    }

    #endregion
}