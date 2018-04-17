using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ItemSelect : MonoBehaviour, IInputClickHandler, IFocusable
{
    [SerializeField]
    protected UnityEvent onClick;
    [SerializeField]
    private GameObject boundingBox;

    private bool isFocused;

    public virtual void OnFocusEnter()
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Highlight");
            //Debug.Log(child.name);
        }
        isFocused = true;
        boundingBox?.SetActive(true);
        //if (onFocusEnter != null) onFocusEnter.Invoke();
    }

    public virtual void OnFocusExit()
    {
        isFocused = false;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        boundingBox?.SetActive(false);
        //if (onFocusExit != null) onFocusExit.Invoke();
    }

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
        Application.Quit();
    }

    public void Return(string fromScene)
    {
        //var as1 = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //while (!as1.isDone) ;
        SceneManager.LoadScene(fromScene, LoadSceneMode.Single);
    }

    public void CreateFile()
    {
        Transform root = GameObject.Find("SometimeUse").transform;
        GameObject keyboard = root.Find("Keyboard").gameObject;
        keyboard.SetActive(true);

        GameObject singleLine = root.Find("SignleLineInput").gameObject;
        singleLine.SetActive(true);
        singleLine.GetComponent<SingleLineInput>().InputComplate += () =>
        {
            FileAndDictionary.Instance.CreateFile(singleLine.GetComponent<SingleLineInput>().GetContent());
            singleLine.SetActive(false);
            keyboard.SetActive(false);

            GetComponentInParent<ScanDictionary>().UpdateDictionaryTree();
        };
        keyboard.GetComponent<Keyboard>().InputTarget = singleLine;
    }

    public void CreateFolder()
    {

    }

    private void Update()
    {
        if (isFocused && (Input.GetKeyUp(KeyCode.Return) || Input.GetMouseButtonDown(0)))
        {
            onClick.Invoke();
        }
    }
}
