using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Personagem : MonoBehaviour
{
    public LayerMask semRC;

    private Rigidbody Corpo;
    public Animator Anim;
    [SerializeField] GameObject CorpoMonge;

    [SerializeField]
    float sensibilidadeGiro = 400;
    [SerializeField]
    float velocidadeAndar = 4;
    private float velocidadeFinal = 0;
    [SerializeField]
    bool esperandoSegundos = false;

    private bool recebeuInputMover;
    public bool estaNoChao = true;
    bool canMove = true;
    bool touroDomado = false;

    [SerializeField]
    private GameObject MaoColisor;

    [SerializeField]
    private Vector3 posInicial;

    float tempo = 0.0f;
    float segundosParaEsperar;

    // Sistema checkpoints
    Vector3[] infoCheckpoint = new Vector3[2];
    GameObject[] PedrasParaReset;
    int raizesAtivadosCheckpoint;

    // Puzzle das pedras
    bool empurrandoPedra = false;
    Transform Pedra;
    Vector3 PedraPosInicial;
    public Vector3 frentePedra;

    [SerializeField]
    private Image BotaoInteracao;

    bool semPedra = true;
    bool longeCanalizador = true;

    protected CanvasGroup blackScreen_CanvasGroup;

    BlackScreenController _blackScreenController => BlackScreenController.I;


    void Awake()
    {
        Anim = CorpoMonge.GetComponent<Animator>();
        Corpo = GetComponent<Rigidbody>();
        //transform.position = posInicial;
        //CorpoMonge = this.gameObject.transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        blackScreen_CanvasGroup = BlackScreenController.I.GetBlackPanelCanvasGroup();
    }

    void Update()
    {
        CorpoMonge.transform.position = transform.position;

        // O player só pode se mover se não estiver no meio de empurrar uma pedra
        ControleMovimento();
        ControleBotaoInteracao();
        // Puzzle das pedras
        ResetarPuzzlePedras();
        EmpurrarPedra();
    }

    void FixedUpdate()
    {
        if (recebeuInputMover && canMove && !esperandoSegundos && !empurrandoPedra)
        {
            Mover();
        }
    }

    void ControleMovimento()
    {
        if (canMove)
        {
            if (!esperandoSegundos)
            {
                if (!empurrandoPedra)
                {
                    ReceberInputs();
                    Girar();
                    Corpo.constraints = RigidbodyConstraints.FreezeRotation;
                    AnimacaoAndar();
                    VirarPersonagemMovimento();
                }


            }
        }
    }
    void ReceberInputs()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            recebeuInputMover = true;
        }
        else
        {
            recebeuInputMover = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Run") > 0)
        {
            velocidadeFinal = velocidadeAndar * 1.5f;
        }
        else
        {
            velocidadeFinal = velocidadeAndar;
        }

    }
    void VirarPersonagemMovimento()
    {
        if (canMove)
        {
            if (Input.GetAxis("Vertical") < 0 && (Input.GetAxis("Horizontal") < 0.3 && Input.GetAxis("Horizontal") > -0.3))
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && (Input.GetAxis("Horizontal") < 0.3 && Input.GetAxis("Horizontal") > -0.3))
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") > -0.3f && Input.GetAxis("Vertical") < 0.3f && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > -0.3f && Input.GetAxis("Vertical") < 0.3f && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.right, 10f);
            }
            else if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, -transform.forward + -transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                RotacionarEmDirecaoAAlgo(CorpoMonge, transform.forward + -transform.right, 10f);
            }
            else if(Input.GetAxis("Mouse X") == 0 && this.gameObject.transform.forward != CorpoMonge.transform.forward)
            {
                RotacionarEmDirecaoAAlgo(this.gameObject, CorpoMonge.transform.forward, 2f);
            }
            else
            {
                CorpoMonge.transform.rotation = transform.rotation;
            }
        }
        else
        {
            RotacionarEmDirecaoAAlgo(CorpoMonge.gameObject, transform.forward, 2.5f);
        }

    }

    void AnimacaoAndar()
    {
        if (canMove)
        {
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !Input.GetKey(KeyCode.LeftShift))
            {
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Andando", true);
                
            }
            else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                Anim.SetBool("Andando", false);
                Anim.SetBool("Correndo", true);
            }
            else
            {
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Andando", false);
            }

        }
    }

    void Mover()
    {
        float velocidadeZ;
        float velocidadeX;

        velocidadeZ = Input.GetAxis("Vertical") * velocidadeFinal;
        velocidadeX = Input.GetAxis("Horizontal") * velocidadeFinal;

        //velocidadeX = 0;
        Vector3 velocidadeCorrigida = velocidadeX * transform.right + velocidadeZ * transform.forward;

        Corpo.velocity = new Vector3(velocidadeCorrigida.x, Corpo.velocity.y, velocidadeCorrigida.z);
    }

    void Girar()
    {
        float GiroY = Input.GetAxis("Mouse X") * sensibilidadeGiro * Time.deltaTime;
        transform.Rotate(Vector3.up * GiroY);
    }

    void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Checkpoint")
        {
            if (colidiu.gameObject.GetComponent<Checkpoint>().GetIsLastCheckpoint())
            {
                infoCheckpoint[0] = new Vector3(0, 0, 0);
                infoCheckpoint[1] = new Vector3(0, 0, 0);
                Destroy(colidiu.gameObject);

            }
            else
            {
                infoCheckpoint[0] = new Vector3(colidiu.gameObject.transform.position.x, transform.position.y, colidiu.gameObject.transform.position.z);
                infoCheckpoint[1] = colidiu.gameObject.transform.eulerAngles;
                PedrasParaReset = colidiu.gameObject.GetComponent<Checkpoint>().GetStonesToResetList();
                raizesAtivadosCheckpoint = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().GetActiveRoots();
                Destroy(colidiu.gameObject);
            }
        }

    }
    public void RotacionarEmDirecaoAAlgo(GameObject obj, Vector3 ondeOlhar, float velocidadeGiro)
    {
        Vector3 newDirection = Vector3.RotateTowards(obj.transform.forward, ondeOlhar, velocidadeGiro * Time.deltaTime, 0.0f);
        obj.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public bool SeTouroEstaDomado()
    {
        return touroDomado;
    }

    void ControleBotaoInteracao()
    {
        PertoCanalizador();
        if(longeCanalizador && semPedra)
        {
            BotaoInteracao.gameObject.SetActive(false);
        }
    }

    void EmpurrarPedra()
    {
        if (!empurrandoPedra)
        {
            Pedra = ChecarSePertoDePedra();
            if (Pedra != null)
            {
                if (ChecarSePodeMoverPedra() && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
                {
                    semPedra = false;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)))
                    {
                        frentePedra = EncontrarFrentePedra(Pedra);
                        PedraPosInicial = Pedra.transform.position;
                        AnimacaoEmpurrarPedra(0);
                        Corpo.transform.position -= Corpo.transform.forward * 0.8f;
                        MaoColisor.SetActive(true);
                        BotaoInteracao.gameObject.SetActive(false);
                        empurrandoPedra = true;
                    }
                    else
                    {
                        BotaoInteracao.gameObject.SetActive(true);
                    }
                }
                else
                {
                    semPedra = true;
                }
            }
        }
        else
        {
            float velocidadeGiroParaPedra = 1f;
            float rapidezEmpurrar = 0.5f;

            RotacionarEmDirecaoAAlgo(this.gameObject, frentePedra, velocidadeGiroParaPedra);
            RotacionarEmDirecaoAAlgo(CorpoMonge, frentePedra, velocidadeGiroParaPedra);
            StartCoroutine(SetCanMove(true));
            Pedra.GetComponent<Rigidbody>().mass = 1;

            // Para prender o movimento da pedra nos eixos não desejado

            if (frentePedra.x == 0)
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; 
            }
            else
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                Corpo.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            Corpo.velocity = velocidadeAndar * rapidezEmpurrar * frentePedra;


            float tpzI = PedraPosInicial.z;
            float tpz = Pedra.transform.position.z;
            float tpxI = PedraPosInicial.x;
            float tpx = Pedra.transform.position.x;

            if (System.Math.Round(tpz, 0) == System.Math.Round(tpzI - 8, 0) || System.Math.Round(tpz, 0) == System.Math.Round(tpzI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI - 8, 0))
            {
                Corpo.velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().mass = 1000000f;
                AnimacaoEmpurrarPedra(1);
                MaoColisor.SetActive(false);
                canMove = true;
                empurrandoPedra = false;
            }
        }
    }
    void AnimacaoEmpurrarPedra(int estado)
    {
        if (touroDomado)
        {
            // Sequência de animações com touro
        }
        else
        {
            if(estado == 0)
            {
                Anim.SetBool("Andando", false);
                Anim.SetBool("Correndo", false);
                Anim.SetBool("Empurrando", true);
            }
            else
            {
                Anim.SetBool("Empurrando", false);
            }
        }
    }

    void PertoCanalizador()
    {
        GameObject[] Canalizadores;
        Canalizadores = GameObject.FindGameObjectsWithTag("Canalizador");

        float minimumDistance = 5f;

        Transform CanalizadorMaisPerto = null;

        foreach (GameObject can in Canalizadores)
        {
            float distance = Vector3.Distance(transform.position, can.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                CanalizadorMaisPerto = can.transform;
            }
        }

        if (CanalizadorMaisPerto != null)
        {
            longeCanalizador = false;
        }
        else
        {
            longeCanalizador = true;
        }
    }

    Transform ChecarSePertoDePedra()
    {
        GameObject[] PedrasLeves;
        PedrasLeves = GameObject.FindGameObjectsWithTag("PedraLeve");

        float minimumDistance = 6.5f;

        Transform PedraMaisPerto = null;

        foreach (GameObject pedra in PedrasLeves)
        {
            float distance = Vector3.Distance(transform.position, pedra.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                PedraMaisPerto = pedra.transform;
            }
        }

        if (PedraMaisPerto != null)
        {
            return PedraMaisPerto;
        }
        else
        {
            return null;
        }
    }

    bool ChecarSePodeMoverPedra()
    {
        //Pedra.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        frentePedra = EncontrarFrentePedra(Pedra);
        Vector3 direction = frentePedra;

        // Verificação que pode mover a pedra se for pesada
        if (Pedra.gameObject.tag == "PedraPesada")
        {
            if (!touroDomado)
            {
                return false;
            }
        }
        if (Vector3.Angle(CorpoMonge.transform.forward, direction) > 45)
        {
            return false;
        }

        RaycastHit meuRay;
        if (Physics.Raycast(Pedra.transform.position, direction, out meuRay, 10f, ~semRC))
        {
            string colisor = meuRay.collider.gameObject.tag;
            if (colisor != "Parede" && colisor != "PedraLeve" && colisor != "PedraPesada" && colisor != "Raiz1" && colisor != "Raiz2" && colisor != "Canalizador")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }

    }

    Vector3 EncontrarFrentePedra(Transform Pedra)
    {
        Vector3 direction = CorpoMonge.transform.position - Pedra.position;

        // No eixo Z
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z) && direction.z > -1.7f && direction.z < 1.7f)
        {
            if (direction.x > 0)
            {
                return new Vector3(-1, 0, 0);
            }
            else
            {
                return new Vector3(1, 0, 0);
            }
        }
        // No eixo X
        else if (Mathf.Abs(direction.z) > Mathf.Abs(direction.x) && direction.x > -1.7f && direction.x < 1.7f)
        {
            if (direction.z > 0)
            {
                return new Vector3(0, 0, -1);
            }
            else
            {
                return new Vector3(0, 0, 1);
            }
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }

    void ResetarPuzzlePedras()
    {
        if(!esperandoSegundos && !empurrandoPedra && canMove && LevelManager.I.IsInPuzzleArea())
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                if(infoCheckpoint[0] != new Vector3(0f, 0f, 0f))
                {
                    Corpo.velocity = new Vector3(0f, 0f, 0f);
                    Anim.SetBool("Correndo", false);
                    Anim.SetBool("Andando", false);
                    BotaoInteracao.gameObject.SetActive(false);
                    StartCoroutine(SetCanMove(true));
                    blackScreen_CanvasGroup.DOFade(1, Helpers.blackFadeTime).OnComplete(() =>
                    {
                        transform.position = infoCheckpoint[0];
                        CorpoMonge.transform.eulerAngles = infoCheckpoint[1];
                        blackScreen_CanvasGroup.DOFade(0, Helpers.blackFadeTime).OnComplete(() => StartCoroutine(SetCanMove(false)));
                    });
                }
            }
        }
    }

    public IEnumerator SetCanMove(bool state)
    {
        if (state)
        {
            yield return new WaitForSeconds(1);
            canMove = true;
        }
        else
        {
            Anim.SetBool("Correndo", false);
            Anim.SetBool("Andando", false);
            Corpo.velocity = new Vector3(0, 0, 0);
            canMove = false;
        }
    }

    public void TerminarResetPuzzle()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().ActivateRoots(raizesAtivadosCheckpoint);

        for (int i = 0; i < PedrasParaReset.Length; i++)
        {
            PedrasParaReset[i].transform.position = PedrasParaReset[i].GetComponent<Pedra>().PosicaoInicial;
        }
    }
}