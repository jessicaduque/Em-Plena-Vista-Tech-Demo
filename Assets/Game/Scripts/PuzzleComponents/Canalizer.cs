using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canalizer : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    private GameObject _player;
    public string interactionPrompt => _prompt;
    private ThirdPersonAnimation _thirdPersonAnimation => ThirdPersonAnimation.I;
    private ThirdPersonController _thirdPersonController => ThirdPersonController.I;
    private StonePuzzleManager _stonePuzzleManager => StonePuzzleManager.I;

    private void Awake()
    {
        _player = Player.I.gameObject;
    }

    #region Interaction
    public bool CanInteract()
    {
        return true;
    }

    public void InteractControl(Interactor interactor)
    {
        _thirdPersonController.DisableInputs();
        StartCoroutine(TurnToInteractable());
    }

    public IEnumerator TurnToInteractable()
    {
        Vector3 relativePos = new Vector3(this.transform.position.x, _player.transform.position.y, this.transform.position.z) - _player.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);

        while (_player.transform.rotation != toRotation)
        {
            _player.transform.rotation = Quaternion.RotateTowards(_player.transform.rotation, toRotation, Time.deltaTime * 100);
            yield return null;
        }

        _thirdPersonAnimation.SetTrigger("Canalize");
        _stonePuzzleManager.SwitchActiveRoots();
        _stonePuzzleManager.ActivateRootsAnimation();

        yield return null;
    }

    #endregion
}