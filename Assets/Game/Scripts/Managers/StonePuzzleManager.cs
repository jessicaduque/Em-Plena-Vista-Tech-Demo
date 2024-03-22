using UnityEngine;
using Utils.Singleton;
using System.Collections.Generic;

public class StonePuzzleManager : Singleton<StonePuzzleManager>
{
    private Checkpoint lastCheckpointScript;
    private bool roots1Active = true;

    private List<GameObject> roots1;
    private List<GameObject> roots2;

    private int roots1Amount = 0;
    private int roots2Amount = 0;
    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;

    private new void Awake()
    {
        // TEMPORARY
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        roots1 = new List<GameObject>();
        roots2 = new List<GameObject>();
    }

    private void Start()
    {
        GameObject[] roots = GameObject.FindGameObjectsWithTag("Root");
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].GetComponent<Root>().GetIsRoot1())
            {
                roots1.Add(roots[i]);
                roots1Amount++;
            }
            else
            {
                roots2.Add(roots[i]);
                roots2Amount++;
            }
        }

        ActivateRoots(roots1Active);
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

    #region Roots

    public void ActivateRoots(bool activateRoots1)
    {
        for (int i = 0; i < roots1Amount; i++)
        {
            roots1[i].SetActive(activateRoots1);
        }

        for (int i = 0; i < roots2Amount; i++)
        {
            roots2[i].SetActive(!activateRoots1);
        }
    }

    public void SwitchActiveRoots()
    {
        roots1Active = !roots1Active;
    }

    #endregion

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