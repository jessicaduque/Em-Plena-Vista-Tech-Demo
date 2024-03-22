using UnityEngine;
using Utils.Singleton;
using System.Collections.Generic;

public class StonePuzzleManager : Singleton<StonePuzzleManager>
{
    private Checkpoint lastCheckpointScript;
    private int initialRoots = 1;
    private int activeRoots;
    private List<GameObject> Roots1;
    private List<GameObject> Roots2;
    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;

    private new void Awake()
    {
        GameObject[] roots = GameObject.FindGameObjectsWithTag("Root");
        for (int i=0; i < roots.Length; i++)
        {
            if (roots[i].GetComponent<Root>().GetIsRoot1())
                Roots1.Add(roots[i]);
            else
                Roots2.Add(roots[i]);
        }
        
    }

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