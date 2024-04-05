using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singleton;

public class GameController : Singleton<GameController>
{
    protected override void Awake()
    {
        base.Awake();

        LockMouse(false);
    }

    public void LockMouse(bool state)
    {
        Cursor.visible = state;
        Cursor.lockState = (state ? CursorLockMode.Locked : CursorLockMode.None);
    }
}
