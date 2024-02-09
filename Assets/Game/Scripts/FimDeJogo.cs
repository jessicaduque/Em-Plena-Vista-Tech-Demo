using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDeJogo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        SceneManager.LoadScene(0);
        
    }
}
