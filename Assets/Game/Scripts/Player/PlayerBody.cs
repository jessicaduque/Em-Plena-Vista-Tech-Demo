using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;

    public void EnableInputs()
    {
        _thirdPersonController.EnableInputs();
    }
}