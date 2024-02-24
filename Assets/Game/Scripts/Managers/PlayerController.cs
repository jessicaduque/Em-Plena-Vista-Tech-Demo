using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public LayerMask semRC;

    private Rigidbody thisRb;
    public Animator thisAnimator;

    //TEMPORARIO
    [SerializeField] GameObject CorpoMonge;

    [SerializeField] float turnSensibility = 400;
    [SerializeField] float WalkSpeed = 4;

    float finalVelocity = 0;

    private bool recievedMovementInput;
    bool canMove = true;

    [SerializeField]
    private GameObject HandCollider;

    [SerializeField]
    private Vector3 initialPosition;

    private Canalizador Canalizador;

    // Sistema checkpoints
    Vector3[] infoCheckpoint = new Vector3[2];
    GameObject[] RocksToReset;
    int activatedRoots;

    // Puzzle das pedras
    bool pushingStone = false;
    Transform Pedra;
    Vector3 PedraPosInicial;
    public Vector3 frentePedra;

    [SerializeField]
    private Image BotaoInteracao;

    bool semPedra = true;
    bool longeCanalizador = true;

    protected CanvasGroup blackScreen_CanvasGroup;

    BlackScreenController _blackScreenController => BlackScreenController.I;
    LevelManager _levelManager => LevelManager.I;


    void Awake()
    {
        thisAnimator = CorpoMonge.GetComponent<Animator>();
        thisRb = GetComponent<Rigidbody>();
        //transform.position = initialPosition;
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
        MovementControl();
        ControleBotaoInteracao();
        // Puzzle das pedras
        ResetStonePuzzle();
        PushStone();
    }

    void FixedUpdate()
    {
        if (recievedMovementInput && canMove && !pushingStone)
        {
            Move();
        }
    }

    #region Movement

    void MovementControl()
    {
        if (canMove)
        {
            if (!pushingStone)
            {
                RecieveInputs();
                Turn();
                thisRb.constraints = RigidbodyConstraints.FreezeRotation;
                WalkAnimation();
                TurnPlayerMovement();
            }

        }
    }
    void RecieveInputs()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            recievedMovementInput = true;
        }
        else
        {
            recievedMovementInput = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetAxis("Run") > 0)
        {
            finalVelocity = WalkSpeed * 1.5f;
        }
        else
        {
            finalVelocity = WalkSpeed;
        }

    }
    void TurnPlayerMovement()
    {
        if (canMove)
        {
            if (Input.GetAxis("Vertical") < 0 && (Input.GetAxis("Horizontal") < 0.3 && Input.GetAxis("Horizontal") > -0.3))
            {
                TurnTowards(CorpoMonge, -transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && (Input.GetAxis("Horizontal") < 0.3 && Input.GetAxis("Horizontal") > -0.3))
            {
                TurnTowards(CorpoMonge, transform.forward, 10f);
            }
            else if (Input.GetAxis("Vertical") > -0.3f && Input.GetAxis("Vertical") < 0.3f && Input.GetAxis("Horizontal") > 0)
            {
                TurnTowards(CorpoMonge, transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > -0.3f && Input.GetAxis("Vertical") < 0.3f && Input.GetAxis("Horizontal") < 0)
            {
                TurnTowards(CorpoMonge, -transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0)
            {
                TurnTowards(CorpoMonge, -transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0)
            {
                TurnTowards(CorpoMonge, -transform.forward + -transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0)
            {
                TurnTowards(CorpoMonge, transform.forward + transform.right, 10f);
            }
            else if (Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0)
            {
                TurnTowards(CorpoMonge, transform.forward + -transform.right, 10f);
            }
            else if (Input.GetAxis("Mouse X") == 0 && this.gameObject.transform.forward != CorpoMonge.transform.forward)
            {
                TurnTowards(this.gameObject, CorpoMonge.transform.forward, 2f);
            }
            else
            {
                CorpoMonge.transform.rotation = transform.rotation;
            }
        }
        else
        {
            TurnTowards(CorpoMonge.gameObject, transform.forward, 2.5f);
        }

    }

    void WalkAnimation()
    {
        if (canMove)
        {
            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !Input.GetKey(KeyCode.LeftShift))
            {
                thisAnimator.SetBool("Correndo", false);
                thisAnimator.SetBool("Andando", true);

            }
            else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Input.GetKey(KeyCode.LeftShift))
            {
                thisAnimator.SetBool("Andando", false);
                thisAnimator.SetBool("Correndo", true);
            }
            else
            {
                thisAnimator.SetBool("Correndo", false);
                thisAnimator.SetBool("Andando", false);
            }

        }
    }

    void Move()
    {
        float velocidadeZ;
        float velocidadeX;

        velocidadeZ = Input.GetAxis("Vertical") * finalVelocity;
        velocidadeX = Input.GetAxis("Horizontal") * finalVelocity;

        //velocidadeX = 0;
        Vector3 velocidadeCorrigida = velocidadeX * transform.right + velocidadeZ * transform.forward;

        thisRb.velocity = new Vector3(velocidadeCorrigida.x, thisRb.velocity.y, velocidadeCorrigida.z);
    }

    void Turn()
    {
        float GiroY = Input.GetAxis("Mouse X") * turnSensibility * Time.deltaTime;
        transform.Rotate(Vector3.up * GiroY);
    }

    public void TurnTowards(GameObject obj, Vector3 ondeOlhar, float velocidadeGiro)
    {
        Vector3 newDirection = Vector3.RotateTowards(obj.transform.forward, ondeOlhar, velocidadeGiro * Time.deltaTime, 0.0f);
        obj.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    #endregion

    #region Collision

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
                RocksToReset = colidiu.gameObject.GetComponent<Checkpoint>().GetStonesToResetList();
                activatedRoots = _levelManager.GetActiveRoots();
                Destroy(colidiu.gameObject);
            }
        }

    }

    #endregion

    void ControleBotaoInteracao()
    {
        NearCanalizer();
        if (longeCanalizador && semPedra)
        {
            BotaoInteracao.gameObject.SetActive(false);
        }
    }

    #region Stone Puzzle

    void PushStone()
    {
        if (!pushingStone)
        {
            Pedra = IsNearStone();
            if (Pedra != null)
            {
                if (CheckIfMovingStone() && EncontrarFrentePedra(Pedra) != new Vector3(0, 0, 0))
                {
                    semPedra = false;
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)))
                    {
                        frentePedra = EncontrarFrentePedra(Pedra);
                        PedraPosInicial = Pedra.transform.position;
                        PushingStoneAnimation(0);
                        thisRb.transform.position -= thisRb.transform.forward * 0.8f;
                        HandCollider.SetActive(true);
                        BotaoInteracao.gameObject.SetActive(false);
                        pushingStone = true;
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

            TurnTowards(this.gameObject, frentePedra, velocidadeGiroParaPedra);
            TurnTowards(CorpoMonge, frentePedra, velocidadeGiroParaPedra);
            StartCoroutine(SetCanMove(true));
            Pedra.GetComponent<Rigidbody>().mass = 1;

            // Para prender o movimento da pedra nos eixos não desejado

            if (frentePedra.x == 0)
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                thisRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                Pedra.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                thisRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            }

            thisRb.velocity = WalkSpeed * rapidezEmpurrar * frentePedra;


            float tpzI = PedraPosInicial.z;
            float tpz = Pedra.transform.position.z;
            float tpxI = PedraPosInicial.x;
            float tpx = Pedra.transform.position.x;

            if (System.Math.Round(tpz, 0) == System.Math.Round(tpzI - 8, 0) || System.Math.Round(tpz, 0) == System.Math.Round(tpzI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI + 8, 0) || System.Math.Round(tpx, 0) == System.Math.Round(tpxI - 8, 0))
            {
                thisRb.velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                Pedra.GetComponent<Rigidbody>().mass = 1000000f;
                PushingStoneAnimation(1);
                HandCollider.SetActive(false);
                canMove = true;
                pushingStone = false;
            }
        }
    }
    void PushingStoneAnimation(int estado)
    {
        if (estado == 0)
        {
            thisAnimator.SetBool("Andando", false);
            thisAnimator.SetBool("Correndo", false);
            thisAnimator.SetBool("Empurrando", true);
        }
        else
        {
            thisAnimator.SetBool("Empurrando", false);
        }
    }

    void NearCanalizer()
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

    public void ReceberCanalizador(GameObject canali)
    {
        Canalizador = canali.GetComponent<Canalizador>();
    }

    public void CanalizingFinished()
    {
        StartCoroutine(Canalizador.FinishCanalizingCutscene());
    }


    Transform IsNearStone()
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

    bool CheckIfMovingStone()
    {
        //Pedra.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        frentePedra = EncontrarFrentePedra(Pedra);
        Vector3 direction = frentePedra;

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

    void ResetStonePuzzle()
    {
        if (canMove && !pushingStone && canMove && _levelManager.IsInPuzzleArea())
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                if (infoCheckpoint[0] != new Vector3(0f, 0f, 0f))
                {
                    thisRb.velocity = new Vector3(0f, 0f, 0f);
                    thisAnimator.SetBool("Correndo", false);
                    thisAnimator.SetBool("Andando", false);
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

    public void FinsihPuzzleReset()
    {
        _levelManager.ActivateRoots(activatedRoots);

        for (int i = 0; i < RocksToReset.Length; i++)
        {
            RocksToReset[i].transform.position = RocksToReset[i].GetComponent<Pedra>().PosicaoInicial;
        }
    }

    #endregion

    public IEnumerator SetCanMove(bool state)
    {
        if (state)
        {
            yield return new WaitForSeconds(1);
            canMove = true;
        }
        else
        {
            thisAnimator.SetBool("Correndo", false);
            thisAnimator.SetBool("Andando", false);
            thisRb.velocity = new Vector3(0, 0, 0);
            canMove = false;
        }
    }

    public GameObject GetPlayerBody()
    {
        return CorpoMonge;
    }

}