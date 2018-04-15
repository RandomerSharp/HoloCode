using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif
public class FileAndDictionary : HoloToolkit.Unity.Singleton<FileAndDictionary>
{
    private string rootPath;
    private string workspacePath;

    public string RootPath
    {
        get
        {
            return rootPath;
        }
    }
    public string WorkspacePath
    {
        get
        {
#if UNITY_EDITOR
            return "Demo";
#else
            return workspacePath;
#endif
        }

        set
        {
            workspacePath = value;
        }
    }

#if UNITY_WSA && NETFX_CORE
    public async Task<string[]> ReadFolder()
    {
        StorageFolder document = await KnownFolders.DocumentsLibrary.GetFolderAsync("Workspace");
        var l = (await document.GetFoldersAsync()).ToList();
        return (from i in l
                select i.Name).ToArray();
    }
#else
    public void ReadFolder()
    {

    }
#endif

    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        rootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#else
        rootPath = KnownFolders.DocumentsLibrary.Path;
#endif
    }

    public void CreateFolder(string folderName)
    {
        string absPath;
        System.IO.Directory.CreateDirectory(folderName);
    }
}
