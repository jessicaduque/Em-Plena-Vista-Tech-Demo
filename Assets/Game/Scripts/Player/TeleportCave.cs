using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TeleportCave : TeleportPlayer
{
    //[SerializeField] List<GameObject> rocksToDisable = new List<GameObject>();

    //bool playerAlreadyPassed;

    //UIManager _uiManager => UIManager.I;

    //protected override void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        StartCoroutine(PlayerScript.SetCanMove(false));
    //        blackScreen_CanvasGroup.DOFade(1, Helpers.blackFadeTime).OnComplete(() =>
    //        {
    //            Player.transform.position = finalPosition;
    //            CorpoMonge.transform.eulerAngles = lookingDirection;
    //            blackScreen_CanvasGroup.DOFade(0, Helpers.blackFadeTime).OnComplete(() => {
    //                if (playerAlreadyPassed)
    //                {
    //                    StartCoroutine(PlayerScript.SetCanMove(true));
    //                }
    //                else
    //                {
    //                    _uiManager.ControlEndPanel(true);
    //                }
    //            });
    //        });
    //    }

    //    if (!playerAlreadyPassed)
    //    {
    //        for (int i = 0; i < rocksToDisable.Count; i++)
    //        {
    //            if (rocksToDisable[i] != null)
    //            {
    //                rocksToDisable[i].SetActive(false);
    //            }

    //        }
    //    }

    //    playerAlreadyPassed = true;
    //}

}