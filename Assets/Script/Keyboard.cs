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
        inputTarget.GetComponent<IVKeyInput>().VKeyInput(key);
    }
}
