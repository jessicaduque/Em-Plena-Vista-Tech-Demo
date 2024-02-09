using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private Canalizador Canalizador;

    public void ReceberCanalizador(GameObject canali)
    {
        Canalizador = canali.GetComponent<Canalizador>();
    }

    public void CanalizingFinished()
    {
        StartCoroutine(Canalizador.FinishCanalizingCutscene());
    }
}
