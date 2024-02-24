using UnityEngine;
using Utils.Singleton;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    GameObject Player;
    PlayerController PlayerScript;

    [SerializeField] CanvasGroup EndPanel_CanvasGroup;

    protected override void Awake()
    {
        base.Awake();

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<PlayerController>();
    }

    public void ControlEndPanel(bool state)
    {
        EndPanel_CanvasGroup.DOFade((state ? 1 : 0), Helpers.panelFadeTime);

        if (!state)
        {
            StartCoroutine(PlayerScript.SetCanMove(true));
        }
    }
}
