using Utils.Singleton;

/// <summary>
/// Overall controller instance for the whole tech demo
/// </summary>
public class GameController : Singleton<GameController>
{
    public bool gameCompleted { get; private set; } // Indicates if all puzzles have been completed and player has reached the end cave

}
