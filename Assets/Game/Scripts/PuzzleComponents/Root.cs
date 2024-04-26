using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private bool isRoot1;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stone"))
        {
            this.gameObject.SetActive(false);
        }
    }
    public bool GetIsRoot1()
    {
        return isRoot1;
    }
}
