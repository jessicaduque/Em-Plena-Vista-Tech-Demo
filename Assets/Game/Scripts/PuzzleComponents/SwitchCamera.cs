using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private GameObject _cameraToTurnOff;
    [SerializeField] private GameObject _cameraToTurnOn;
    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _blackScreenController.CameraChangeFade(_cameraToTurnOff, _cameraToTurnOn);
        }
    }
}
