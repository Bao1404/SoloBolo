using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coin = 0f;
    [SerializeField] private float coinPerSecond = 1f;

    private void Start()
    {
        Transform targetParent = GameObject.Find("Canvas/SelectChampion")?.transform;

        List<string> champions = ChampionSelector.Instance.selectedChampions;

        if (targetParent != null && champions != null)
        {
            for (int i = 0; i < champions.Count; i++)
            {
                GameObject prefab = Resources.Load<GameObject>("Spawn" + champions[i]);
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, targetParent);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = new Vector3(-400f + (400f * i), -770f, -85);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy prefab: Prefabs/Spawn{champions[i]} trong thư mục Resources");
                }
            }
        }
        else Debug.Log("không tìm thấy Canvas/SelectChampion hoặc champions null");
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        UpdateCoin();
    }
    private void UpdateCoin()
    {
        coin += coinPerSecond * Time.deltaTime;
        coinText.text = Mathf.FloorToInt(coin).ToString("0000");
    }
    public void AddCoin(float amount)
    {
        coin += amount;
    }
    public bool TrySpendCoin(float cost)
    {
        if (coin >= cost)
        {
            coin -= cost;
            return true;
        }
        return false;
    }
}
