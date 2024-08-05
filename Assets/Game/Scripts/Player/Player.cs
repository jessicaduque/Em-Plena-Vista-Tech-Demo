using System.Collections;
using UnityEngine;
using Utils.Singleton;

/// <summary>
/// Player main manager instance to control overall player information
/// </summary>
public class Player : Singleton<Player>
{
    [SerializeField] GameObject _normalPlayerBody; // Player body which won't show silhouette behind walls 
    [SerializeField] GameObject _occuludedPlayerBody; // Play body that will show silhouette behind walls for puzzle
    // Puzzle variables
    public bool isInPuzzle { get; private set; } = false; // Indicates if player is inside puzzle area (to reduce update checks)

    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I; // Gets the StonePuzzleManager script instance
    private UIManager _uiManager => UIManager.I; // Gets the UIManager script instance
    /// <summary>
    /// Rewrites singleton Awake 
    /// </summary>
    private new void Awake()
    {
    }
    /// <summary>
    /// Once game starts, IEnumerator for turning on the control indications on screen
    /// </summary>
    private IEnumerator ControlIndicationWait()
    {
        yield return new WaitForSeconds(Helpers.blackFadeTime);

        _uiManager.ControlIndicationResetInfo(true);
    }

    #region SET
    /// <summary>
    /// Defines if player is inside the puzzle space or not
    /// </summary>
    /// <param name="isInPuzzle"> Boolean value that indicates if Player is in puzzle </param>
    /// <param name="lastPuzzleEnter"> Boolean value that indicates if a last checkpoint has been passed in puzzle </param>
    public void SetIsInPuzzle(bool isInPuzzle, bool lastPuzzleEnter)
    {
        this.isInPuzzle = isInPuzzle;
        SetPlayerBody(isInPuzzle);
        if (!lastPuzzleEnter)
        {
            if (isInPuzzle && _stonePuzzleManager.GetLastCheckpointTransform() != null)
            {
                StartCoroutine(ControlIndicationWait());
            }
            else
            {
                StopAllCoroutines();
                _uiManager.ControlIndicationResetInfo(false);
            }
        }
        else
        {
            this.isInPuzzle = false;
        }
    }
    /// <summary>
    /// Changes activated play body according to sitation
    /// </summary>
    public void SetPlayerBody(bool state)
    {
        _normalPlayerBody.SetActive(!state);
        _occuludedPlayerBody.SetActive(state);
        ThirdPersonAnimation.I.GetAnimator();
    }

    #endregion
}