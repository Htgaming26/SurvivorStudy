using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public float spawnTime = 2f;
    public float radius = 5f;

    Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(SpawnMonster), 0f, spawnTime);
    }

    void SpawnMonster()
    {
        Vector2 center = player.position;
        Vector2 spawnPos = center + Random.insideUnitCircle * radius;
        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
    }
}
