using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStart : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("Start");
    }
}
