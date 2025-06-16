using UnityEngine;
using UnityEngine.UI;

public class ChampionSlot : MonoBehaviour
{
    private bool isSelected = false;
    private Image image;
    private bool isTransparent = false;

    [SerializeField] private GameObject hiddenPrefab;

    void Start()
    {
        image = GetComponent<Image>();
    }

    // Gắn hàm này vào OnClick() của Button
    public void OnClick()
    {
        if (image == null) return;

        Color color = image.color;
        color.a = isTransparent ? 1f : 0.2f;
        image.color = color;

        isTransparent = !isTransparent;

        Debug.Log($"[ChampionSlot] OnClick() called - GameObject: {gameObject.name}");

        if (ChampionSelector.Instance == null) return;
        Debug.Log($"[ChampionSlot] Prefab found: {hiddenPrefab.name}");

        if (!isSelected && ChampionSelector.Instance.CanSelectMore())
        {
            isSelected = true;
            transform.localScale = Vector3.one * 1.2f;
            SetAlpha(0.5f);

            // ✅ Kiểm tra prefab ẩn
            if (hiddenPrefab != null)
            {
                Debug.Log($"[ChampionSlot] Prefab found: {hiddenPrefab.name}");
            }
            else
            {
                Debug.LogWarning("[ChampionSlot] No hidden prefab found under this button.");
            }
        }
        else if (isSelected)
        {
            isSelected = false;
            transform.localScale = Vector3.one;
            SetAlpha(1f);
        }
    }


    private void SetAlpha(float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }
}
