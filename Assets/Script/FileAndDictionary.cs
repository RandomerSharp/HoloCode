using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Linq;
#if UNITY_WSA && NETFX_CORE 
using Windows.Storage;
#endif
public class FileAndDictionary
{
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
}
