using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    [SerializeField]
    private GameObject inputTarget;

    private void Awake()
    {

    }

    public void ReceiveKey(KeyCode key)
    {
        Debug.Log(key.ToString());
        inputTarget.GetComponent<IVKeyInput>().VKeyInput(key);
    }
}
