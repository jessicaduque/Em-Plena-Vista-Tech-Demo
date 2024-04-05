using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool roots1Active { get; private set; }
    public bool isLastPuzzleCheckpoint;
    public GameObject[] stonesToReset;

    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            roots1Active = _stonePuzzleManager.GetRoots1Active();
            _stonePuzzleManager.SetLastCheckpoint(this);
        }
    }
}