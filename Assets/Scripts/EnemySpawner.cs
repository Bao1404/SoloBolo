using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obj;   // Prefab index 0,1,2,3...

    private float timer = 0f;

    // Cho 0→50s
    private int spawnEventCount0to50 = 0;
    private const int maxSpawnEvents0to50 = 10;
    private float nextSpawnTime0to50 = 5f;  // lần đầu spawn lúc 5s

    // Cho 50s trở đi
    private float nextSpawnTime50plus = 60f; // lần đầu spawn lúc 60s (50 + 10)

    void Update()
    {
        timer += Time.deltaTime;

        // 0→50s: spawn 10 lần, mỗi 5s, mỗi lần random 1–2 champ (loại 0 hoặc 1)
        if (timer < 50f && spawnEventCount0to50 < maxSpawnEvents0to50)
        {
            if (timer >= nextSpawnTime0to50)
            {
                int count = Random.Range(1, 3); // 1 hoặc 2
                for (int i = 0; i < count; i++)
                {
                    // mỗi lần spawn 1 đối tượng loại 0 hoặc 1
                    int type = Random.Range(0, 2);
                    SpawnEnemy(type);
                }

                spawnEventCount0to50++;
                nextSpawnTime0to50 += 5f;
            }
        }
        // Từ 50s trở đi: mỗi 10s spawn random 1–3 champ (loại 0..3)
        else if (timer >= 50f)
        {
            if (timer >= nextSpawnTime50plus)
            {
                int count = Random.Range(1, 4); // 1,2 hoặc 3
                for (int i = 0; i < count; i++)
                {
                    int type = Random.Range(0, Mathf.Min(obj.Length, 4)); // đảm bảo không vượt prefab[]
                    SpawnEnemy(type);
                }

                nextSpawnTime50plus += 10f;
            }
        }
    }

    private void SpawnEnemy(int index)
    {
        Vector2 pos = new Vector2(9.5f, Random.Range(-2f, -1f));
        Instantiate(obj[index], pos, Quaternion.identity);
    }
}
