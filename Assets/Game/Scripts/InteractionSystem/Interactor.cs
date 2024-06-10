using UnityEngine;
/// <summary>
/// Interactor object in front of player that recognizes interactable objects, and checks if interaction is possible
/// </summary>
public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint; // Point where collider to check interactable objects will be originated from
    [SerializeField] private float _interactionPointRadius = 0.5f; // Interactor radius for its area
    [SerializeField] private LayerMask _interactableMask; // Layer the interactable will be able to collide with

    private readonly Collider[] _colliders = new Collider[3]; // Array to catch colliders that collide with the interactor
    [SerializeField] private int _numFound; // Amount of colliders found that collide with the interactor
    private int _previousNumFound; // Previous amount of colliders that collided with the interactor

    private UIManager _uiManager => UIManager.I; // Gets the UIManager Instance

    /// <summary>
    /// Resets previous amount of colliders detected and disables the UI interction button
    /// </summary>
    private void OnDisable()
    {
        _previousNumFound = 0;
        _uiManager.ControlInteractionButton(false);
    }
    /// <summary>
    /// Every frame, creates the interactor collider, checks if it collides with any interactable objects 
    /// and saves that information, controls the UI interaction button
    /// </summary>
    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            
            if (interactable != null && _previousNumFound == 0)
            {
                if (interactable.CanInteract())
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
    /// <summary>
    /// If any colliders are detected during Update, checks if object's interaction conditions are met and 
    /// executes it's interaction function
    /// </summary>
    public void InteractControl()
    {
        if(_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            if(interactable != null)
            {
                if (interactable.CanInteract())
                {
                    interactable.InteractControl(this);
                }
            }
        }
    }
    /// <summary>
    /// Draws in red the interactor collider generated during Update so it can be 
    /// seen as a gizmo in the scene window
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
