using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemClick : MonoBehaviour, IInputClickHandler
{
    [SerializeField]
    protected UnityEvent onClick;

    private bool isFocused;

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        onClick.Invoke();
    }

    public void SelectWorkspace()
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene("SelectWorkspace", LoadSceneMode.Single);
    }

    public void GotoSettings()
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene("Settings", LoadSceneMode.Single);
    }

    public void Quit()
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene(fromScene, LoadSceneMode.Single);
    }

    public void GenerateBS()
    {
        FindObjectsOfType<MenuWheelSelector>()[0].Generate();
    }

    private void Update()
    {
        if (isFocused && (Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            onClick.Invoke();
        }
    }
}
