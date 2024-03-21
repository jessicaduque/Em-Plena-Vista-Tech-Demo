using Utils.Singleton;

public class StonePuzzleManager : Singleton<StonePuzzleManager>
{
    private Checkpoint lastCheckpointScript;

    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;
    public void ResetStonePuzzle()
    {
        _thirdPlayerController.enabled = false;

        if (lastCheckpointScript != null)
        {
            for (int i = 0; i < lastCheckpointScript.stonesToReset.Length; i++)
            {
                lastCheckpointScript.stonesToReset[i].GetComponent<Stone>().ResetPosition();
            }
        }
    }

    #region SET

    public void SetLastCheckpoint(Checkpoint lastCheckpointScript)
    {
        if (!lastCheckpointScript.isLastPuzzleCheckpoint)
            this.lastCheckpointScript = lastCheckpointScript;
        else
            this.lastCheckpointScript = null;
    }

    #endregion
}