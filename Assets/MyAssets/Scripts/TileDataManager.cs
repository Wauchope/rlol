using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TileDataManager
{
    public static TileDataManager Instance;
    private string path = Application.dataPath + "MyAssets/Tiles/";

    public TileDataManager()
    {
        Instance = this;
    }

    private void WriteJson(Tile tile)
    {
        var json = JsonUtility.ToJson(tile);
        File.WriteAllText(path + tile.name + ".json", json);
    }

    private Tile ReadJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<Tile>(json);
    }

    //Read and Write 
}
