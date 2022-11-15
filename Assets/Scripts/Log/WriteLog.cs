using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Storage;
using Windows.System;
using Windows.Storage.Streams;
#endif

public class WriteLog : MonoBehaviour
{
    [SerializeField]
    private GameObject Cube;
    //define filePath


#if WINDOWS_UWP
Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
Windows.Storage.StorageFolder storageFolder =  Windows.Storage.KnownFolders.DocumentsLibrary;
#endif


    //private string saved line;
    private string saveInformation = "";

    private static string fileName = "AABBCC.txt";

    //private save counter
    private static bool firstSave = false;

    //Hashtable declaration
    //private static Dictionary<string, IconProperty> iconCollection = new Dictionary<string, IconProperty>();

    private void Start()
    {
        Job();
    }

#if WINDOWS_UWP
    async Task WriteData()
    {
        Cube.SetActive(true);
        if (firstSave){
        StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.AppendTextAsync(sampleFile, saveInformation + "\r\n");
        firstSave = false;
        }
    else{
        Cube.SetActive(true);
        StorageFile sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        await FileIO.AppendTextAsync(sampleFile, saveInformation + "\r\n");
    }
    }
#endif

    public async Task addToCounter(string iconName)
    {
        Debug.Log(iconName + "\n" + iconName + "2");
        saveInformation = iconName;
#if WINDOWS_UWP
        await WriteData();
#endif
    }

    public async Task Job()
    {
        await addToCounter("QQPR");
#if WINDOWS_UWP
        await addToCounter("QQPR");
        await addToCounter("QQPR1");
        await addToCounter("QQPR2");
        await addToCounter("QQPR3");
        await addToCounter("QQPR4");
        await addToCounter("QQPR5");
        await addToCounter("QQPR6");
#endif
    }
}