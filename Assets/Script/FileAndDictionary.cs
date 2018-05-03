using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Linq;
using System.IO;

#if UNITY_WSA && NETFX_CORE
using Windows.Storage;
#endif
public class FileAndDictionary : MixedRealityToolkit.Common.Singleton<FileAndDictionary>
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
    public string FolderPath
    {
        get
        {
            return Path.Combine(rootPath, "Workspace", workspacePath);
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
        //workspacePath = "Demo";
#else
        rootPath = KnownFolders.DocumentsLibrary.Path;
#endif
    }

    public void CreateFile(string fileName)
    {
        File.Create(Path.Combine(rootPath, "Workspace", workspacePath, fileName));
    }

    public string OpenFile(string path, string name)
    {
        string code = string.Empty;
        using (Stream stream = new FileStream(Path.Combine(path, name), FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                code = reader.ReadToEnd();
            }
        }
        return code;
    }

    public string OpenFile(string name)
    {
        string code = string.Empty;
        using (Stream stream = new FileStream(Path.Combine(FolderPath, name), FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                code = reader.ReadToEnd();
                //reader.Close();
            }
            //stream.Close();
        }
        return code;
    }

    public void SaveFile(string fileName, string content)
    {
        using (Stream stream = new FileStream(Path.Combine(FolderPath, fileName), FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.Write(content);
            }
        }
    }

    public void SaveBrainScript(string brainScript)
    {
        string fileName = string.Format("{0}.bs", System.DateTime.Now.Ticks.ToString());
        using (Stream stream = new FileStream(Path.Combine(RootPath, "Workspace", "BrainScript", fileName), FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                //sw.Write(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
                sw.Write(brainScript);
            }
        }
    }

    public void ScanFolder(string path)
    {
        var folders = System.IO.Directory.GetFiles(path);
    }
}
