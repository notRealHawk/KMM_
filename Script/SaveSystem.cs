/* 
    ------------------- Code Monkey -------------------
    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!
               unitycodemonkey.com
    --------------------------------------------------
 */
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";
    private const string SAVE_EXTENSION = "txt";
    public static void Init()
    {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER))
        {
            // Create Save Folder
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }
    public static void Save(string filname, string saveString)
    {
        string path = Path.Combine(Application.persistentDataPath, filname + "." + SAVE_EXTENSION);
        Debug.Log(path);
        // Make sure the Save Number is unique so it doesnt overwrite a previous save file
        if (File.Exists(path))
        {
            Debug.Log("File Exists : ");
            File.Delete(path);
            Debug.Log("File Deleted");
            File.WriteAllText(path, saveString);
            Debug.Log("Created New File");

        }
        else
        {
            File.WriteAllText(path, saveString);
        }
    }
    public static string Load(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename + "." + SAVE_EXTENSION);
        Debug.Log(path);
        if (File.Exists(path))
        {
            Debug.Log("FileExists");
            string saveString = File.ReadAllText(path);
            return saveString;
        }
        else
        {
            return null;
        }
    }
}
