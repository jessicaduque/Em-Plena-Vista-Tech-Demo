using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] bool isLastCheckpoint;
    [SerializeField] GameObject[] stonesToResetList;

    #region GET
    public bool GetIsLastCheckpoint()
    {
        return isLastCheckpoint;
    }

    public GameObject[] GetStonesToResetList()
    {
        return stonesToResetList;
    }
    #endregion
}
