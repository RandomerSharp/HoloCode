using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    [SerializeField]
    private GameObject inputTarget;

    public GameObject InputTarget
    {
        get
        {
            return inputTarget;
        }

        set
        {
            inputTarget = value;
        }
    }

    private void Awake()
    {

    }

    public void ReceiveKey(KeyCode key)
    {
        //Debug.Log(key.ToString());
        if (key == KeyCode.LeftShift || key == KeyCode.RightShift)
        {
            foreach (var i in GetComponentsInChildren<KeyboardKey>())
            {
                i.Shift();
            }
            return;
        }
        inputTarget.GetComponent<IVKeyInput>().VKeyInput(key);
    }
}
