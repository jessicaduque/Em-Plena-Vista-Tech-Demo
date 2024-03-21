using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private bool isRoot1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stone")
        {
            this.gameObject.SetActive(false);
        }
    }
}
