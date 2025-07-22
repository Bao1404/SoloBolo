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

        // Debug.Log("name: " + hiddenPrefab.name);

        Color color = image.color;
        color.a = isTransparent ? 1f : 0.2f;

        if (!isSelected && ChampionSelector.Instance.CanSelectMore())
        {
            ChampionSelector.Instance.SelectChampion(hiddenPrefab.name);
            isSelected = true;

            isTransparent = true;
            image.color = color;
        }
        else if (isSelected)
        {
            ChampionSelector.Instance.DeselectChampion(hiddenPrefab.name);
            isSelected = false;

            isTransparent = false;
            image.color = color;
        }
    }

}
