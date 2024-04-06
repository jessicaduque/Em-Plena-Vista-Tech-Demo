using UnityEngine;
using Utils.Singleton;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;

public class StonePuzzleManager : Singleton<StonePuzzleManager>
{
    [SerializeField] private float _activationYOffset = 6.5f;
    private float _activationAnimationTime = 1;

    private GameObject _lastCheckpoint;
    private Checkpoint _lastCheckpointScript;
    private bool _roots1Active = true;

    private List<GameObject> _roots1;
    private List<GameObject> _roots2;

    private int _roots1Amount = 0;
    private int _roots2Amount = 0;

    private GameObject _player;
    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;
    private BlackScreenController _blackScreenController => BlackScreenController.I;

    private new void Awake()
    {
        // TEMPORARY
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _player = Player.I.gameObject;
        _roots1 = new List<GameObject>();
        _roots2 = new List<GameObject>();
    }

    private void Start()
    {
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

        ActivateRoots(_roots1Active);
    }

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
                _lastCheckpointScript.stonesToReset[i].GetComponent<Stone>().ResetPosition();
            }
        }

        ActivateRoots(_lastCheckpointScript.roots1Active);

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

    public void ActivateRootsAnimation(bool activateRoots1)
    {
        for (int i = 0; i < _roots1Amount; i++)
        {
            if(activateRoots1)
                _roots1[i].SetActive(true);
            _roots1[i].transform.DOMoveY(_roots1[i].transform.position.y + (activateRoots1 ? _activationYOffset : -_activationYOffset), _activationAnimationTime).OnComplete(() => 
            { 
                if (activateRoots1) 
                    _roots1[i].SetActive(false);
            });
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            if (!activateRoots1)
                _roots2[i].SetActive(true);
            _roots2[i].transform.DOMoveY(_roots2[i].transform.position.y + (activateRoots1 ? -_activationYOffset : _activationYOffset), _activationAnimationTime).OnComplete(() =>
            {
                if (!activateRoots1)
                    _roots2[i].SetActive(false);
            });
        }
    }

    public void ActivateRoots(bool activateRoots1)
    {
        for (int i = 0; i < _roots1Amount; i++)
        {
            _roots1[i].SetActive(activateRoots1);
        }

        for (int i = 0; i < _roots2Amount; i++)
        {
            _roots2[i].SetActive(!activateRoots1);
        }
    }

    public void SwitchActiveRoots()
    {
        _roots1Active = !_roots1Active;
    }

    #endregion

    #region SET

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

    #endregion

    #region Get

    public bool GetRoots1Active()
    {
        return _roots1Active;
    }

    #endregion
}