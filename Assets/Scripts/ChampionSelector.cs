using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChampionSelector : MonoBehaviour
{
    public static ChampionSelector Instance;

    public List<string> selectedChampions = new List<string>();
    public int maxChampion = 3;

    private void Awake()
    {
        // int level = 1;
        // if (SaveManager.Instance != null)
        // {
        //     level = SaveManager.Instance.LoadLevel();
        // }

        // if (level <= 4)
        // {
        //     maxChampion = level + 1;
        // }
        // else
        // {
        //     maxChampion = 4;
        // }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SelectChampion(string name)
    {
        if (!selectedChampions.Contains(name) && selectedChampions.Count < maxChampion)
        {
            selectedChampions.Add(name);
            Debug.Log("Add: " + name);
        }
    }

    public void DeselectChampion(string name)
    {
        if (selectedChampions.Contains(name))
        {
            selectedChampions.Remove(name);
            Debug.Log("Remove: " + name);
        }
    }

    public bool CanSelectMore()
    {
        return selectedChampions.Count < maxChampion;
    }

    public void GoToNextScene()
    {
        int count = selectedChampions.Count;
        if (count > 0 && count <= maxChampion)
        {
            // int level = SaveManager.Instance.LoadLevel();
            // Debug.Log("Level"+level);
            // SceneManager.LoadScene("SceneGameLevel" + SaveManager.Instance.LoadLevel());
            SceneManager.LoadScene("SceneGame");

        }
        else
        {
            Debug.LogWarning("Bạn phải chọn ít nhất 1 và không quá " + maxChampion + " tướng để tiếp tục.");
        }
    }
    public void GoToNextScene2()
    {
        int count = selectedChampions.Count;
        if (count > 0 && count <= maxChampion)
        {
            // int level = SaveManager.Instance.LoadLevel();
            // Debug.Log("Level"+level);
            // SceneManager.LoadScene("SceneGameLevel" + SaveManager.Instance.LoadLevel());
            SceneManager.LoadScene("SceneGame2");
            GameManager.instance.RestartGame("SceneGame2");
        }
        else
        {
            Debug.LogWarning("Bạn phải chọn ít nhất 1 và không quá " + maxChampion + " tướng để tiếp tục.");
        }
    }
   

}
