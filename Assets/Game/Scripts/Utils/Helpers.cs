using UnityEngine;
using DG.Tweening;

public static class Helpers
{
    public const float blackFadeTime = 0.6f;
    public const float panelFadeTime = 0.4f;

    public static void FadeInPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.GetComponent<CanvasGroup>().DOFade(1, panelFadeTime).SetUpdate(true);
    }

    public static void FadeOutPanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().DOFade(0, panelFadeTime).OnComplete(() => panel.SetActive(false)).SetUpdate(true);
    }

    public static void FadeCrossPanel(GameObject panelOff, GameObject panelOn)
    {
        panelOff.GetComponent<CanvasGroup>().DOFade(0, panelFadeTime).OnComplete(() => {
            panelOff.SetActive(false);
            panelOn.SetActive(true);
            panelOn.GetComponent<CanvasGroup>().DOFade(1, panelFadeTime);
        });
    }
}