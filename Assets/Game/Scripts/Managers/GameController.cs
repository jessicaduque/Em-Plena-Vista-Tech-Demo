using UnityEngine;
using Utils.Singleton;

/// <summary>
/// Overall controller instance for the whole tech demo
/// </summary>
public class GameController : Singleton<GameController>
{
    public bool gameCompleted { get; private set; } // Indicates if all puzzles have been completed and player has reached the end cave

    /// <summary>
    /// Inherits singleton script Awake and the start of the game (main menu) makes sure mouse is unlocked
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        Helpers.LockMouse(false);
    }
}
