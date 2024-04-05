using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool roots1Active { get; private set; }
    public bool isLastPuzzleCheckpoint { get; private set; }
    public GameObject[] stonesToReset { get; private set; }

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