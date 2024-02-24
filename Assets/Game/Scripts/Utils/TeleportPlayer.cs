using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportPlayer : MonoBehaviour
{
    protected GameObject Player;
    protected PlayerController PlayerScript;
    protected CanvasGroup blackScreen_CanvasGroup;
    protected GameObject CorpoMonge;

    [SerializeField] protected Vector3 finalPosition;
    [SerializeField] protected Vector3 lookingDirection;

    BlackScreenController _blackScreenController => BlackScreenController.I;


    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerController>();
    }
    private void Start()
    {
        blackScreen_CanvasGroup = BlackScreenController.I.GetBlackPanelCanvasGroup();
        CorpoMonge = PlayerScript.GetPlayerBody();
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PlayerScript.SetCanMove(false));
            blackScreen_CanvasGroup.DOFade(1, Helpers.blackFadeTime).OnComplete(() =>
            {
                Player.transform.position = finalPosition;
                CorpoMonge.transform.eulerAngles = lookingDirection;
                blackScreen_CanvasGroup.DOFade(0, Helpers.blackFadeTime).OnComplete(() => StartCoroutine(PlayerScript.SetCanMove(true)));
            });
            
        }
    }
}
