using UnityEngine;
using UnityEngine.UI;
using Utils.Singleton;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] int initialRootsActivated = 1;
    [SerializeField] Image InteractionButton;
    // First element: Superior left corner
    // Second element: Inferior right corner
    [SerializeField] Vector3[] stonePuzzleArea;

    int currentRootsActivated;
    GameObject[] Roots1;
    GameObject[] Roots2;
    GameObject Player;
    private bool playerInteracting = false;

    protected override void Awake()
    {
        base.Awake();

        Player = GameObject.FindGameObjectWithTag("Player");

        InteractionButton.gameObject.SetActive(false);

        CursorSetup();

        currentRootsActivated = initialRootsActivated;
        ActivateRoots(initialRootsActivated);
        SetupGetRoots();
    }

    void CursorSetup()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsInPuzzleArea()
    {
        if ((Player.transform.position.x >= stonePuzzleArea[0].x && Player.transform.position.x <= stonePuzzleArea[1].x) && (Player.transform.position.z >= stonePuzzleArea[1].z && Player.transform.position.z <= stonePuzzleArea[0].z))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Roots

    void SetupGetRoots()
    {
        Roots1 = GameObject.FindGameObjectsWithTag("Root1");
        Roots2 = GameObject.FindGameObjectsWithTag("Root2");
    }

    public void ActivateRoots(int roots)
    {
        if (roots == 2)
        {
            for (int i = 0; i < Roots1.Length; i++)
            {
                Roots1[i].SetActive(false);
            }

            for (int i = 0; i < Roots2.Length; i++)
            {
                Roots2[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < Roots1.Length; i++)
            {
                Roots1[i].SetActive(true);
            }

            for (int i = 0; i < Roots2.Length; i++)
            {
                Roots2[i].SetActive(false);
            }
        }
    }
    public void SwitchActiveRoots()
    {
        if (currentRootsActivated == 1)
        {
            currentRootsActivated = 2;
        }
        else
        {
            currentRootsActivated = 1;
        }

    }
    public int GetActiveRoots()
    {
        return currentRootsActivated;
    }

    #endregion 
}
