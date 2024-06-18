using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private bool isRoot1;
    [SerializeField] private GameObject pss;

    private void Awake()
    {
        pss.transform.parent = null;
    }

    private void OnEnable()
    {
        pss.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            this.gameObject.SetActive(false);
            pss.SetActive(false);
        }
    }
    public bool GetIsRoot1()
    {
        return isRoot1;
    }
}
