using UnityEngine;
using Cinemachine;

public class EfeitoVisualCanalizar : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vcam;
    [SerializeField]
    float zOriginal;
    private float zMin = -5.5f;
    [SerializeField]
    float Zdec;
    [SerializeField]
    private CanvasGroup pretoCanvasGroup;
    [SerializeField]
    float Pretodec;

    CinemachineTransposer transposer;

    bool finalizando = false;

    bool cam = false;
    bool preta = false;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    //private void Update()
    //{
    //    if (finalizando)
    //    {
    //        ZoomOutEffects();
    //    }
    //    else
    //    {
    //        preta = false;
    //        cam = false;
    //    }
    //}

    public bool ZoomInEffects()
    {
        // Parte da cam
        float zAtual = transposer.m_FollowOffset.z;
        if (zAtual < zMin)
        {
            transposer.m_FollowOffset += new Vector3(0, 0, Zdec) * Time.deltaTime;
        }
        Debug.Log(zAtual);

        // Parte da tela preta
        if (pretoCanvasGroup.alpha < 1)
        {
            pretoCanvasGroup.alpha += Pretodec * Time.deltaTime;
        }

        return pretoCanvasGroup.alpha >= 1 && zAtual >= zMin;
    }

    public bool ZoomOutEffects()
    {
        // Parte da cam
        finalizando = true;
        float zAtual = transposer.m_FollowOffset.z;

        if (zAtual > zOriginal)
        {
            transposer.m_FollowOffset -= new Vector3(0, 0, Zdec) * Time.deltaTime;
            //cam = false;
        }
        //else
        //{
        //    transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, zOriginal);
        //    cam = true;
        //}

        // Parte da tela preta
        if (pretoCanvasGroup.alpha > 0)
        {
            //preta = false;
            pretoCanvasGroup.alpha -= Pretodec * Time.deltaTime;
        }
        //else
        //{
        //    preta = true;
        //}

        //if(preta && cam)
        //{
        //    finalizando = false;
        //}

        return zAtual <= zOriginal && pretoCanvasGroup.alpha == 0;
    }

}
