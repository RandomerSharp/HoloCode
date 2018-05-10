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
public class FileAndDirectory : MixedRealityToolkit.Common.Singleton<FileAndDirectory>
{
    private string workspacePath;
    private string projectName;

    /// <summary>
    /// Workspace的路径
    /// </summary>
    public string WorkspacePath
    {
        get
        {
            return workspacePath;
        }
    }
    /// <summary>
    /// 当前工作目录路径
    /// </summary>
    public string ProjectName
    {
        get
        {
            //#if UNITY_EDITOR
            //          return "Demo";
            //#else
            return projectName;
            //#endif
        }

        set
        {
            projectName = value;
        }
    }

    public string FolderPath
    {
        get
        {
            //return Path.Combine(rootPath, "Workspace", workspacePath);
            return Path.Combine(workspacePath, projectName);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //#if UNITY_EDITOR
        workspacePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Workspace");
        //workspacePath = "Demo";
        //#else
        //rootPath = KnownFolders.DocumentsLibrary.Path;
        //#endif
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="fileName">相对项目的路径</param>
    public void CreateFile(string fileName)
    {
        //File.Create(Path.Combine(rootPath, "Workspace", workspacePath, fileName));
        var fd = fileName.Split('/');
        string t = FolderPath;
        for (int i = 0; i < fd.Length - 1; i++)
        {
            t = Path.Combine(t, fd[i]);
            if (!Directory.Exists(t))
            {
                Directory.CreateDirectory(t);
            }
        }
        //File.Create(Path.Combine(rootPath, workspacePath, fileName));
        File.Create(Path.Combine(t, fd.LastOrDefault()));
    }

    /// <summary>
    /// 使用绝对路径打开文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 使用相对路径打开文件
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
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
        //using (Stream stream = new FileStream(Path.Combine(RootPath, "Workspace", "BrainScript", fileName), FileMode.OpenOrCreate, FileAccess.Write))
        using (Stream stream = new FileStream(Path.Combine(WorkspacePath, "BrainScript", fileName), FileMode.OpenOrCreate, FileAccess.Write))
        {
            using (StreamWriter sw = new StreamWriter(stream))
            {
                //sw.Write(transform.parent.GetComponentInChildren<TMPro.TextMeshPro>().text);
                sw.Write(brainScript);
            }
        }
    }

    /// <summary>
    /// 使用绝对路径创建文件夹
    /// </summary>
    /// <param name="absPath"></param>
    public bool CreateFolder(string absPath)
    {
        if (Directory.Exists(absPath))
        {
            return false;
        }
        Directory.CreateDirectory(absPath);
        return true;
    }

    public bool CreateProject(string name)
    {
        if (Directory.Exists(Path.Combine(WorkspacePath, name)))
        {
            return false;
        }
        Directory.CreateDirectory(Path.Combine(WorkspacePath, name));
        return true;
    }

    public string[] GetFilesInFolder(string path)
    {
        return Directory.GetFiles(path);
    }

    public string[] GetFoldersInFolder(string path)
    {
        return Directory.GetDirectories(path);
    }

    public string FullFilePath(string fileName)
    {
        return Path.Combine(FolderPath, fileName);
    }
}
