using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private float coin = 0f;
    [SerializeField] private float coinPerSecond = 1f;

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
