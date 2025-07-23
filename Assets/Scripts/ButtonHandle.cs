using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandle : MonoBehaviour
{
    public string characterType;
    public void OnClickSpawn()
    {
        Debug.Log(characterType);
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