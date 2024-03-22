using UnityEngine;
using DG.Tweening;

public static class Helpers
{
    public const float tempoPretoFade = 0.4f;
    public const float tempoPanelFade = 0.3f;
    public static Transform cam => Camera.main.transform;

    public static void FadeInPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.GetComponent<CanvasGroup>().DOFade(1, tempoPanelFade).SetUpdate(true);
    }

    public static void FadeOutPanel(GameObject panel)
    {
        panel.GetComponent<CanvasGroup>().DOFade(0, tempoPanelFade).OnComplete(() => panel.SetActive(false)).SetUpdate(true);
    }

    public static void FadeCrossPanel(GameObject panelDesligar, GameObject panelLigar)
    {
        panelDesligar.GetComponent<CanvasGroup>().DOFade(0, tempoPanelFade).OnComplete(() => {
            panelDesligar.SetActive(false);
            panelLigar.SetActive(true);
            panelLigar.GetComponent<CanvasGroup>().DOFade(1, tempoPanelFade);
        });
    }
}