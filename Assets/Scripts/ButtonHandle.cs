using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickSpawn(string characterType)
    {
        CharacterSpawner spawner = Object.FindFirstObjectByType<CharacterSpawner>();

        if (spawner != null)
        {
            spawner.SpawnCharacter(characterType);
            Debug.Log("sdadasd");
        }
        else Debug.Log("Null nè e");
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}