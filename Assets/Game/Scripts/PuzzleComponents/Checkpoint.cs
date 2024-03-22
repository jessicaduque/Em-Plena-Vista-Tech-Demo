using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isLastPuzzleCheckpoint;
    public GameObject[] stonesToReset;

    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _stonePuzzleManager.SetLastCheckpoint(this);
        }
    }
}