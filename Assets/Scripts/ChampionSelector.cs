using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChampionSelector : MonoBehaviour
{
    public static ChampionSelector Instance;

    public List<string> selectedChampions = new List<string>();
    public int maxChampion = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi chuyển scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectChampion(string name)
    {
        if (!selectedChampions.Contains(name) && selectedChampions.Count < maxChampion)
        {
            selectedChampions.Add(name);
            Debug.Log("Đã chọn: " + name);
        }
    }

    public void DeselectChampion(string name)
    {
        if (selectedChampions.Contains(name))
        {
            selectedChampions.Remove(name);
            Debug.Log("Bỏ chọn: " + name);
        }
    }

    public bool CanSelectMore()
    {
        return selectedChampions.Count < maxChampion;
    }

    public void GoToNextScene(string sceneName)
    {
        int count = selectedChampions.Count;
        if (count > 0 && count <= maxChampion)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Bạn phải chọn ít nhất 1 và không quá " + maxChampion + " tướng để tiếp tục.");
            // (Tùy chọn) hiển thị thông báo lỗi trên UI nếu bạn muốn
        }
    }

}
