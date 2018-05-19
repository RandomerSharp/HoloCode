using MixedRealityToolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadStartScene());
    }

    private IEnumerator LoadStartScene()
    {
        var async = SceneManager.LoadSceneAsync("Start", LoadSceneMode.Single);
        GameObject orb = Instantiate(Resources.Load("Prefab/RotatingOrbs")) as GameObject;
        orb.AddComponent<Tagalong>();
        orb.AddComponent<Billboard>();
        float d = 0f;
        while (!async.isDone)
        {
            yield return null;
            d += Time.deltaTime;
        }
        if (d < 4.9f)
        {
            yield return new WaitForSeconds(5f - d);
        }
    }
}
