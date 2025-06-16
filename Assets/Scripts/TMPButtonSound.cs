using UnityEngine;
using UnityEngine.EventSystems;

public class TMPButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private AudioMenuManager audioManager;
    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioMenuManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager?.PlayHoverSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioManager?.PlayClickSound();
    }
}
