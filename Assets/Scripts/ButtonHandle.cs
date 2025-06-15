using UnityEngine;

public class ButtonHandle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickSpawn()
    {
        CharacterSpawner spawner = Object.FindFirstObjectByType<CharacterSpawner>();

        if (spawner != null)
        {
            spawner.SpawnCharacter();
        }
    }
}
