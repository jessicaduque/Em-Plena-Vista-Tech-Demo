using System.Collections;
using UnityEngine;
using Utils.Singleton;

/// <summary>
/// Player main manager instance to control overall player information
/// </summary>
public class Player : Singleton<Player>
{
    [SerializeField] GameObject _normalPlayerBody;
    [SerializeField] GameObject _occuludedPlayerBody;
    // Puzzle variables
    public bool isInPuzzle { get; private set; } = false; // Indicates if player is inside puzzle area (to reduce update checks)

    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;
    private UIManager _uiManager => UIManager.I;
    /// <summary>
    /// Rewrites singleton Awake and get player's rigidbody component
    /// </summary>
    private new void Awake()
    {
    }

    private IEnumerator ControlIndicationWait()
    {
        yield return new WaitForSeconds(Helpers.blackFadeTime);

        _uiManager.ControlIndicationResetInfo(true);
    }

    #region SET
    /// <summary>
    /// Defines if player is inside the puzzle space or not
    /// </summary>
    /// <param name="isInPuzzle">Bool to set if player is inside the puzzle space or not</param>
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
    
    public void SetPlayerBody(bool state)
    {
        _normalPlayerBody.SetActive(!state);
        _occuludedPlayerBody.SetActive(state);
        ThirdPersonAnimation.I.GetAnimator();
    }

    #endregion
}