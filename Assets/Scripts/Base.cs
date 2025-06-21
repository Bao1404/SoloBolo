using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    [SerializeField] private Image HPBar;
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    private void Start()
    {
        currentHp = maxHp;
        UpdateHealthBar();
    }
    private void Update()
    {
        UpdateHealthBar();
    }
    public void TakeDame(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHealthBar();
        if(currentHp <= 0)
        {
            DestroyBase();
        }
    }
    public void RegenHP()
    {
        
    }
    public void UpdateHealthBar()
    {
        if (HPBar != null)
        {
            HPBar.fillAmount = currentHp / maxHp;
        }
    }
    public void DestroyBase()
    {
        Destroy(gameObject); 
    }
} 
