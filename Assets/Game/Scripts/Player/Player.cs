using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class Player : Singleton<Player>
{
    private Rigidbody rb;

    // Puzzle variables
    public bool isInPuzzle { get; private set; }

    private ThirdPersonController _thirdPlayerController => ThirdPersonController.I;

    private new void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public IEnumerator LookAtObject(Transform obj)
    {
        _thirdPlayerController.enabled = false;

        Vector3 relativePos = obj.transform.position - transform.position;
        relativePos.y = 0;

        Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);

        while (this.rb.rotation != rot)
        {
            this.rb.rotation = Quaternion.Slerp(transform.rotation, rot, 0.4f * Time.deltaTime);
            yield return null;
        }

        _thirdPlayerController.enabled = true;
    }

    #region SET

    public void SetIsInPuzzle(bool isInPuzzle)
    {
        this.isInPuzzle = isInPuzzle;
    }

    #endregion
}