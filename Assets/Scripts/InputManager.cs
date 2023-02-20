using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject inputManager = new GameObject("Input Manager");
                _instance = inputManager.AddComponent<InputManager>();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public delegate bool InputCode(float code);
    public InputCode HorizontalPress;
    public InputCode VerticalPress;

    private static InputManager _instance;
    [SerializeField]
    private float _horizontal = 0;
    [SerializeField]
    private float _vertical = 0;

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if ((HorizontalPress != null) && (horizontal != 0) && (_horizontal == 0))
        {
            if (HorizontalPress.Invoke(horizontal))
            {
                _horizontal = horizontal;
                _vertical = 0;
            }
        }
        else if ((VerticalPress != null) && (vertical != 0) && (_vertical == 0))
        {
            if (VerticalPress.Invoke(vertical))
            {
                _vertical = vertical;
                _horizontal = 0;
            }
        }
    }

    public void ResetInputs()
    {
        _horizontal = 0;
        _vertical = 0;
    }
}
