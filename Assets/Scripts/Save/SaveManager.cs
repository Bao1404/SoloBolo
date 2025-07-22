using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/save.json";
    }

    public void SaveLevel(int level)
    {
        SaveData data = new SaveData();
        data.playerLevel = level;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Saved level: " + level);
    }

    public int LoadLevel()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Level have Loaded: " + data.playerLevel);
            return data.playerLevel;
        }

        return 1; // mặc định nếu chưa có file
    }
}
