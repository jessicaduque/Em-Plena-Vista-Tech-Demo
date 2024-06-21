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

    private Transform _lastCheckpointTransform = null; // Saves last checkpoint position player passed by
    public GameObject _cameraCheckpoint; // Save last checkpoint camera to be active
    private GameObject[] _stonesToReset; // Save last checkpoint stones to reset
    private bool _lastRoots1Active; // Save last checkpoints active roots
    private bool _roots1Active = true; // Indicates if type 1 roots are active at the moment

    private List<GameObject> _roots1 = new List<GameObject>(); // List to store type 1 roots in the scene
    private List<GameObject> _roots2 = new List<GameObject>(); // List to store type 2 roots in the scene

    [SerializeField] ParticleSystem[] _rootsParticles; // Particles for roots

    private int _roots1Amount = 0; // Caches amount of type 1 roots in the scene
    private int _roots2Amount = 0; // Caches amount of type 2 roots in the scene

    private GameObject _playerObject; // Gets the player gameobject

    private UIManager _uiManager => UIManager.I; // Gets the player gameobject
    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I; // Gets the player's third person controller script instance
    private BlackScreenController _blackScreenController => BlackScreenController.I; // Gets the UI black screen controller script instance


    /// <summary>
    /// Rewrites singleton Awake
    /// </summary>
    protected override void Awake()
    {
    }

    /// <summary>
    /// Gets player gameobject, finds all roots in scene and separates them by their types, 
    /// does initial activation for the beginning of the scene
    /// </summary>
    private void Start()
    {
        _playerObject = Player.I.gameObject;

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
        _uiManager.DisableInput();
        _thirdPlayerController.DisableInputs();
        _blackScreenController.FadeInBlack();

        while (!_blackScreenController.GetBlackScreenOn())
        {
            yield return null;
        }

        for (int i = 0; i < _stonesToReset.Length; i++)
        {
            _stonesToReset[i].GetComponent<Stone>().SetPosition();
        }

        for (int i = 0; i < _rootsParticles.Length; i++)
        {
            if (_rootsParticles[i].gameObject.activeInHierarchy)
                _rootsParticles[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        SetRoots1Active(_lastRoots1Active);
        ActivateRoots();
        yield return new WaitForSeconds(0.1f);

        _playerObject.transform.position = new Vector3(_lastCheckpointTransform.position.x, _playerObject.transform.position.y, _lastCheckpointTransform.position.z);
        _playerObject.transform.rotation = _lastCheckpointTransform.rotation;

        _cameraCheckpoint.SetActive(true);

        if(_uiManager.GetActiveCamera() != _cameraCheckpoint)
            _uiManager.GetActiveCamera().SetActive(false);
        
        _uiManager.SetActiveCamera(_cameraCheckpoint);

        yield return new WaitForSeconds(0.1f);

        _blackScreenController.FadeOutBlack();

        while (!_blackScreenController.GetBlackScreenOff())
        {
            yield return null;
        }
        _uiManager.EnableInput();
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
            if (_roots1Active)
                _roots1[i].SetActive(true);
            _roots1[i].transform.DOLocalMoveY(_roots1[i].transform.localPosition.y + (_roots1Active ? _activationYOffset : -_activationYOffset), _activationAnimationTime).OnComplete(() =>
            {
                if (_roots1Active)
                    _roots1[i].SetActive(false);
            });
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            if (!_roots1Active)
                _roots2[i].SetActive(true);
            _roots2[i].transform.DOLocalMoveY(_roots2[i].transform.localPosition.y + (_roots1Active ? -_activationYOffset : _activationYOffset), _activationAnimationTime).OnComplete(() =>
            {
                if (!_roots1Active)
                    _roots2[i].SetActive(false);
            });
        }

        for(int i = 0; i < _rootsParticles.Length; i++)
        {
            if(_rootsParticles[i].gameObject.activeInHierarchy)
                _rootsParticles[i].Play();
        }
    }
    /// <summary>
    /// Without any form of animation, activates the type of roots that are supposed to be activated
    /// </summary>
    public void ActivateRoots()
    {
        for (int i = 0; i < _roots1Amount; i++)
        {
            _roots1[i].SetActive(_roots1Active);
            _roots1[i].transform.localPosition = new Vector3(_roots1[i].transform.localPosition.x, 3 + (_roots1Active ? 0 : -1) * _activationYOffset, _roots1[i].transform.localPosition.z);
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            _roots2[i].SetActive(!_roots1Active);
            _roots2[i].transform.localPosition = new Vector3(_roots2[i].transform.localPosition.x, 3 + (!_roots1Active ? 0 : -1) * _activationYOffset, _roots2[i].transform.localPosition.z);

        }
    }
    /// <summary>
    /// Switches the type of roots activated in scene
    /// </summary>
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
            _lastCheckpointTransform = new GameObject().transform;
            _lastCheckpointTransform.position = lastCheckpointScript.gameObject.transform.position;
            _lastCheckpointTransform.rotation = lastCheckpointScript.gameObject.transform.rotation;
            _lastRoots1Active = lastCheckpointScript.roots1Active;
            _stonesToReset = lastCheckpointScript.stonesToReset;
            _cameraCheckpoint = lastCheckpointScript.cameraCheckpoint;
        }
        else
            _lastCheckpointTransform = null;    

        Destroy(lastCheckpointScript.gameObject);
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
    /// Get method to get if type 1 roots are activated
    /// </summary>
    /// <returns>Returns bool that indicates if type 1 roots are activated</returns>
    public bool GetRoots1Active()
    {
        return _roots1Active;
    }
    /// <summary>
    /// Get method to get the transform of the last checpoint passed by the player
    /// </summary>
    /// <returns>Returns Transform with last passed checkpoint's Transform</returns>
    public Transform GetLastCheckpointTransform()
    {
        return _lastCheckpointTransform;
    }

    #endregion
}