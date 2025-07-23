using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject[] character;
    // Update is called once per frame
    private void Start()
    {
        
    }
    void Update()
    {

    }
    public void SpawnCharacter(string type)
    {

        if (type.Equals("Guard"))
        {
            if (GameManager.instance.TrySpendCoin(10))
            {
                GameObject obstacle = Instantiate(character[0], GetRandomPosition(), Quaternion.identity);
            }
        }
        if (type.Equals("Archer"))
        {
            if (GameManager.instance.TrySpendCoin(15))
            {
                GameObject obstacle = Instantiate(character[1], GetRandomPosition(), Quaternion.identity);
            }
        }
        if (type.Equals("Tanker"))
        {
            if (GameManager.instance.TrySpendCoin(30))
            {
                GameObject obstacle = Instantiate(character[2], GetRandomPosition(), Quaternion.identity);
            }
        }
        if (type.Equals("Car"))
        {
            if (GameManager.instance.TrySpendCoin(40))
            {
                GameObject obstacle = Instantiate(character[3], GetRandomPosition(), Quaternion.identity);
            }
        }
        if (type.Equals("FairyGod"))
        {
            if (GameManager.instance.TrySpendCoin(100))
            {
                GameObject obstacle = Instantiate(character[4], GetRandomPosition(), Quaternion.identity);
            }
        }
    }
        private Vector2 GetRandomPosition()
    {
        return new Vector2(-9.5f, Random.Range(-1f, -2f));
    }
}