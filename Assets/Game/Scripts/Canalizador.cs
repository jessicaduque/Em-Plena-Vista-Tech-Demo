using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canalizador : MonoBehaviour
{
    GameObject Player;
    EfeitoVisualCanalizar canalizingEffectScript;

    [SerializeField]
    private GameObject CorpoMonge;
    [SerializeField]
    private Image BotaoInteracao;

    [SerializeField]
    GameObject ParticlesEffects;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnValidate()
    {
        if(ParticlesEffects != null)
        {
            ParticlesEffects = transform.GetChild(1).gameObject;
        }
        if(canalizingEffectScript != null)
        {
            canalizingEffectScript = GetComponent<EfeitoVisualCanalizar>();
        }
    }

    //void Update()
    //{
    //    if (trocandoRaizes)
    //    {
    //        GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().AtivacaoRaizes(GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().GetRaizesAtivados());
    //        Vector3 relativePos = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z) - Player.transform.position;
    //        Quaternion toRotation = Quaternion.LookRotation(relativePos);
    //        CorpoMonge.transform.rotation = Quaternion.Lerp(CorpoMonge.transform.rotation, toRotation, 3 * Time.deltaTime);
    //        GetComponent<EfeitoVisualCanalizar>().ZoomInEffects();

    //    }
    //    else
    //    {
    //        if (Vector3.Distance(transform.position, Player.transform.position) < 5f)
    //        {
    //            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
    //            {
    //                StartCanalize();
    //                AnimatePlayerActivation();
    //                GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().TrocarRaizesAtivos();
    //                Player.GetComponent<Personagem>().PrenderPersonagem();
    //                CorpoMonge.GetComponent<PlayerBody>().ReceberCanalizador(this.gameObject);
    //                BotaoInteracao.gameObject.SetActive(false);
    //                trocandoRaizes = true;
    //                ParticlesEffects.SetActive(true);
    //            }
    //            else
    //            {
    //                BotaoInteracao.gameObject.SetActive(true);
    //            }
    //        }
    //    }
    //}

    #region Canalizing process start

    void StartCanalize()
    {
        // To turn the player towards the canalizer
        Vector3 relativePos = new Vector3(transform.position.x, Player.transform.position.y, transform.position.z) - Player.transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        CorpoMonge.transform.rotation = Quaternion.Lerp(CorpoMonge.transform.rotation, toRotation, 3 * Time.deltaTime);
        ParticlesEffects.SetActive(true);
        StartCoroutine(StartCanalizingCutscene());
    }

    IEnumerator StartCanalizingCutscene()
    {
        while (!canalizingEffectScript.ZoomInEffects())
        {
            yield return null;
        }
    }

    #endregion

    #region Canalizing process finish

    void FinishCanalize()
    {
        Player.GetComponent<Personagem>().DesprenderPersonagem();
        ParticlesEffects.SetActive(false);
    }

    public IEnumerator FinishCanalizingCutscene()
    {
        while (!canalizingEffectScript.ZoomOutEffects())
        {
            yield return null;
        }
        FinishCanalize();
    }

    #endregion

    #region Player animation canalize
    void AnimatePlayerActivation()
    {
        Player.GetComponent<Personagem>().Anim.SetTrigger("Canalizar");
    }

    #endregion

    #region Trigger collision

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")){
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                Player.GetComponent<Personagem>().PrenderPersonagem();
                StartCanalize();
                AnimatePlayerActivation();
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().TrocarRaizesAtivos();
                CorpoMonge.GetComponent<PlayerBody>().ReceberCanalizador(this.gameObject);
                BotaoInteracao.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BotaoInteracao.gameObject.SetActive(false);
    }

    #endregion
}
