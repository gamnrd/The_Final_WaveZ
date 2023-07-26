using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(GameData data)
    {
        //Create new file or overwrite existing file
        FileStream fs = new FileStream(GetPath(), FileMode.Create);

        //Format the data to binary
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static GameData Load()
    {
        //If file does not exist, create and save empty data
        if (!File.Exists(GetPath()))
        {
            GameData emptyData = new GameData();
            Save(emptyData);
            return emptyData;
        }

        //Open file from path
        FileStream fs = new FileStream(GetPath(), FileMode.Open);
        
        //Convert save data from binary back to GameData
        BinaryFormatter formatter = new BinaryFormatter();
        GameData data = formatter.Deserialize(fs) as GameData;
        fs.Close();

        return data;
    }

    public static string GetPath()
    {
        //Path to save data
        return Application.persistentDataPath + "/data.GN";
    }
}
