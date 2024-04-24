using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private bool isRoot1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Stone"))
        {
            this.gameObject.SetActive(false);
        }
    }

    public bool GetIsRoot1()
    {
        return isRoot1;
    }
}
