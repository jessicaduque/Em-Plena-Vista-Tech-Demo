using System.Collections;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    protected GameObject _player;

    [SerializeField] protected Vector3 _finalPosition;
    [SerializeField] protected Vector3 _lookingDirection;

    protected ThirdPersonController _thirdPlayerController => ThirdPersonController.I;
    private BlackScreenController _blackScreenController => BlackScreenController.I;

    private void Awake()
    {
        _player = Player.I.gameObject;
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(HandleTeleport());
        }
    }

    protected virtual IEnumerator HandleTeleport()
    {
        _thirdPlayerController.DisableInputs();
        _blackScreenController.FadeInBlack();

        while (!_blackScreenController.GetBlackScreenOn())
        {
            yield return null;
        }

        _player.transform.position = _finalPosition;
        _player.transform.localEulerAngles = _lookingDirection;

        _blackScreenController.FadeOutBlack();

        while (!_blackScreenController.GetBlackScreenOff())
        {
            yield return null;
        }

        TeleportFinish();
    } 

    protected virtual void TeleportFinish()
    {
        _thirdPlayerController.EnableInputs();
    }
}