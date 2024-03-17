using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Player : Singleton<Player>
{
    // Puzzle variables
    public bool isInPuzzle {get; private set; }
}
