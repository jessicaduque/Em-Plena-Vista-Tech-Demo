using UnityEngine;
using Utils.Singleton;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

/// <summary>
/// Manager instance to control the stone puzzles
/// </summary>
public class StonePuzzleManager : Singleton<StonePuzzleManager>
{
    [SerializeField] private float _activationYOffset = 6.5f; // Y offset to alter stone positions, so they appear or dissapear
    private float _activationAnimationTime = 2; // Time for stones to appear or dissapear

    private GameObject _lastCheckpoint; // Saves last checkpoint player passed by
    private Checkpoint _lastCheckpointScript; // Gets the Checkpoint script from the last checkpoint the player passed by
    private bool _roots1Active = true; // Indicates if type 1 roots are active at the moment

    private List<GameObject> _roots1; // List to store type 1 roots in the scene
    private List<GameObject> _roots2; // List to store type 2 roots in the scene

    private int _roots1Amount = 0; // Caches amount of type 1 roots in the scene
    private int _roots2Amount = 0; // Caches amount of type 2 roots in the scene

    private GameObject _player; // Gets the player gameobject
    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I; // Gets the player's third person controller script instance
    private BlackScreenController _blackScreenController => BlackScreenController.I; // Gets the UI black screen controller script instance


    /// <summary>
    /// Rewrites singleton Awake and initializes root lists
    /// </summary>
    private new void Awake()
    {
        // TEMPORARY
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _roots1 = new List<GameObject>();
        _roots2 = new List<GameObject>();
    }
    /// <summary>
    /// Gets player gameobject, finds all roots in scene and separates them by their types, 
    /// does initial activation for the beginning of the scene
    /// </summary>
    private void Start()
    {
        _player = Player.I.gameObject;

        GameObject[] roots = GameObject.FindGameObjectsWithTag("Root");
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].GetComponent<Root>().GetIsRoot1())
            {
                _roots1.Add(roots[i]);
                _roots1Amount++;
            }
            else
            {
                _roots2.Add(roots[i]);
                _roots2Amount++;
            }
        }

        ActivateRoots();
    }
    /// <summary>
    /// Uses the last checkpoint the player passed by to reset the puzzles state - all stones,
    /// roots that were activated at that point and player position
    /// </summary>
    public IEnumerator ResetStonePuzzle()
    {
        _thirdPlayerController.DisableInputs();
        _blackScreenController.FadeInBlack();

        while (!_blackScreenController.GetBlackScreenOn())
        {
            yield return null;
        }

        if (_lastCheckpointScript != null)
        {
            for (int i = 0; i < _lastCheckpointScript.stonesToReset.Length; i++)
            {
                _lastCheckpointScript.stonesToReset[i].GetComponent<Stone>().SetPosition();
            }
        }

        SetRoots1Active(_lastCheckpointScript.roots1Active);
        ActivateRoots();

        _player.transform.position = new Vector3(_lastCheckpoint.transform.position.x, _player.transform.position.y, _lastCheckpoint.transform.position.z);
        _player.transform.rotation = _lastCheckpoint.transform.rotation;

        _blackScreenController.FadeOutBlack();

        while (!_blackScreenController.GetBlackScreenOff())
        {
            yield return null;
        }

        _thirdPlayerController.EnableInputs();
    }

    #region Roots
    /// <summary>
    /// Uses a Y offset to show an animation of the stones going down or up (dissapearing or appearing) depending on the activated roots
    /// </summary>
    public void ActivateRootsAnimation()
    {
        for (int i = 0; i < _roots1Amount; i++)
        {
            if(_roots1Active)
                _roots1[i].SetActive(true);
            _roots1[i].transform.DOMoveY(_roots1[i].transform.position.y + (_roots1Active ? _activationYOffset : -_activationYOffset), _activationAnimationTime).OnComplete(() => 
            { 
                if (_roots1Active) 
                    _roots1[i].SetActive(false);
            });
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            if (!_roots1Active)
                _roots2[i].SetActive(true);
            _roots2[i].transform.DOMoveY(_roots2[i].transform.position.y + (_roots1Active ? -_activationYOffset : _activationYOffset), _activationAnimationTime).OnComplete(() =>
            {
                if (!_roots1Active)
                    _roots2[i].SetActive(false);
            });
        }
    }

    public void ActivateRoots()
    {
        for (int i = 0; i < _roots1Amount; i++)
        {
            _roots1[i].SetActive(_roots1Active);
            _roots1[i].transform.position = new Vector3(_roots1[i].transform.position.x, 2.5f + (_roots1Active ? 0 : -1) * _activationYOffset, _roots1[i].transform.position.z);
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            _roots2[i].SetActive(!_roots1Active);
            _roots2[i].transform.position = new Vector3(_roots2[i].transform.position.x, 2.5f + (!_roots1Active ? 0 : -1) * _activationYOffset, _roots2[i].transform.position.z);

        }
    }

    public void SwitchActiveRoots()
    {
        _roots1Active = !_roots1Active;
    }

    #endregion

    #region Set
    /// <summary>
    /// When player passes a checkpoint, saves the checkpoint information
    /// </summary>
    /// <param name="lastCheckpointScript">Script found on the last checkpoint passed by the player</param>
    public void SetLastCheckpoint(Checkpoint lastCheckpointScript)
    {
        if (!lastCheckpointScript.isLastPuzzleCheckpoint)
        {
            this._lastCheckpointScript = lastCheckpointScript;
            _lastCheckpoint = _lastCheckpointScript.gameObject;
        }
        else
            this._lastCheckpointScript = null;
    }

    /// <summary>
    /// Gets player gameobject, finds all roots in scene and separates them by their types, 
    /// does initial activation for the beginning of the scene
    /// </summary>
    /// <param name="state">Indicates new state for the bool that indicates if type 1 roots are activated or not</param>
    private void SetRoots1Active(bool state)
    {
        _roots1Active = state;
    }

    #endregion

    #region Get

    /// <summary>
    /// Get function to get if type 1 roots are activated
    /// </summary>
    /// <returns>Returns bool that indicates if type 1 roots are activated</returns>
    public bool GetRoots1Active()
    {
        return _roots1Active;
    }

    #endregion
}