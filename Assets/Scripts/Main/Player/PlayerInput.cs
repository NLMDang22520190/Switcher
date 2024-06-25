using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputs;

    public bool canSwitch = true;
    public bool canTap = true;

    private void Awake()
    {
        inputs = new PlayerInputActions();
        inputs.Player.Enable();
    }

    public bool Switch()
    {
        if (canSwitch)
        {
            return inputs.Player.Switch.triggered;
        }
        return false;
    }

    public bool Tap()
    {
        if (canTap)
        {
            return inputs.Player.Tap.triggered;
        }
        return false;
    }

}
