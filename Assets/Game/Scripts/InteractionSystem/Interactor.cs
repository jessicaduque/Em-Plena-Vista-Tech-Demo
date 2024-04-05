using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    private int _previousNumFound;

    private UIManager _uiManager => UIManager.I;

    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            Debug.Log(_colliders[0].gameObject.name);
            var interactable = _colliders[0].GetComponent<IInteractable>();
            
            if (interactable != null && _previousNumFound == 0)
            {
                if(interactable.CanInteract())
                    _uiManager.ControlInteractionButton(true);
            }
        }
        else
        {
            if(_previousNumFound > 0)
            {
                _uiManager.ControlInteractionButton(false);
            }
        }

        _previousNumFound = _numFound;
    }

    public void InteractControl()
    {
        if(_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            if(interactable != null)
            {
                if (interactable.CanInteract())
                {
                    StartCoroutine(SnapPlayerDirectionToInteract(90, interactable, _colliders[0].gameObject));
                }
            }
        }
    }

    private IEnumerator SnapPlayerDirectionToInteract(float angleStep, IInteractable interactable, GameObject interactionObject)
    {
        Vector3 test = transform.InverseTransformPoint(interactionObject.transform.position);
        Debug.Log(test);
        //while (this.transform.rotation.eulerAngles.y != yRotation)
        //{
        //    this.transform.localRotation = Quaternion.AngleAxis(yRotation, Vector3.up * Time.deltaTime);
        //    yield return null;
        //}
        
        interactable.Interact(this);
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
