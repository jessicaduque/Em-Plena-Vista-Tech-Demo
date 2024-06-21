using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private GameObject _cameraToTurnOff;
    [SerializeField] private GameObject _cameraToTurnOn;
    [SerializeField] private bool _turnOnMainCamera;

    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;
    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private UIManager _uiManager => UIManager.I;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_uiManager.GetActiveCamera() != _cameraToTurnOn)
            {
                _blackScreenController.CameraChangeFade(_cameraToTurnOff, _cameraToTurnOn);
                _thirdPersonController.SetPuzzleCameraMovement(!_turnOnMainCamera);
                _uiManager.SetActiveCamera(_cameraToTurnOn);
            }
        }
    }
}
