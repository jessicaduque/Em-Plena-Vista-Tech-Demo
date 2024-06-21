using UnityEngine;

public class TurnOffMovementIndicationTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.I.ControlIndicationMovementInfo();
            gameObject.SetActive(false);
        }
    }
}
