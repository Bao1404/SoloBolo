using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject character;
    [SerializeField] private int spawnCost = 10;
    private Character champ;
    // Update is called once per frame
    private void Start()
    {
        champ = character.GetComponent<Character>();
    }
    void Update()
    {

    }
    public void SpawnCharacter(string type)
    {
        if (GameManager.instance.TrySpendCoin(spawnCost))
        {
            if (champ.characterType.Equals(type))
            {
                GameObject champ = Instantiate(character, GetRandomPosition(), Quaternion.identity);
            }
        }
    }
    private Vector2 GetRandomPosition()
    {
        return new Vector2(-9.5f, Random.Range(-1f, -2f));
    }
}