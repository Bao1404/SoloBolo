using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI; // <- thêm dòng này

public class StoryManager : MonoBehaviour
{
    public TextMeshProUGUI storyTextUI;
    public string[] storyLines;
    public float typingSpeed = 0.05f;
    public float autoAdvanceTime = 2f;

    public GameObject eventImage; // <- thêm dòng này

    private int currentLine = 0;
    private bool isTyping = false;
    private bool skipTyping = false;
    private Coroutine autoAdvanceCoroutine;

    void Start()
    {
        eventImage.SetActive(false);
        if (eventImage != null)
        {
            eventImage.SetActive(false); // ẩn lúc đầu
        }
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isTyping)
            {
                skipTyping = true;
            }
            else
            {
                if (autoAdvanceCoroutine != null)
                    StopCoroutine(autoAdvanceCoroutine);

                currentLine++;
                if (currentLine < storyLines.Length)
                    StartCoroutine(TypeLine());
                else
                    LoadNextScene();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        storyTextUI.text = "";

        // Hiển thị hình nếu đang ở dòng 1 hoặc 2
        if (currentLine == 1 || currentLine == 2)
        {
            if (eventImage != null) eventImage.SetActive(true);
        }
        else
        {
            if (eventImage != null) eventImage.SetActive(false);
        }

        foreach (char letter in storyLines[currentLine].ToCharArray())
        {
            if (skipTyping)
            {
                storyTextUI.text = storyLines[currentLine];
                break;
            }

            storyTextUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipTyping = false;

        autoAdvanceCoroutine = StartCoroutine(AutoAdvance());
    }

    IEnumerator AutoAdvance()
    {
        yield return new WaitForSeconds(autoAdvanceTime);

        currentLine++;
        if (currentLine < storyLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SelectChampion1");
    }
}
