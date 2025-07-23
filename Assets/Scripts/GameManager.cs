using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coin = 0f;
    [SerializeField] private float coinPerSecond = 1f;
    [SerializeField] TextMeshProUGUI gameoverText;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject menuButton;
    [SerializeField] TextMeshProUGUI winText;
    private bool isGameOver = false;
    private bool isWin = false;

    private void Start()
    {
        Transform targetParent = GameObject.Find("Canvas/Panel")?.transform;

        gameoverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);

        List<string> champions = ChampionSelector.Instance.selectedChampions;

        if (champions != null)
        {
            for (int i = 0; i < champions.Count; i++)
            {
                GameObject prefab = Resources.Load<GameObject>("Spawn" + champions[i]);
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab, targetParent);
                    obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    obj.transform.localPosition = new Vector3(-200f + (150f * i), -390f, -85);
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
        if (!isGameOver)
        {
            UpdateCoin();
        }
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
    public void GameOver()
    {
            isGameOver = true;
            gameoverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
            Time.timeScale = 0f;
    }
    public void Win()
    {
            
    }
}
