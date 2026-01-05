using UnityEngine;
using System.Collections.Generic;

public class Program : MonoBehaviour
{
    public Camera cam;
    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    public Bound bound;

    public float edgeOffset = 1f;
    public float minDisToPlayer = 4f;
    public int maxTry = 20;
    public float spawnTime = 2f;
    //public float radius = 5f;

    Player player;

    List<Monster> monsters = new();

    // ui
    [SerializeField] HealthBarUI healthBar;
    [SerializeField] ExpManager expBar;

    void Awake()
    {
        cam = Camera.main;
        // sinh ra player
        GameObject playerObj = Instantiate(playerPrefab, new Vector2(0, 1), Quaternion.identity);
        player = playerObj.GetComponent<Player>();
        player.Init(bound, monsters, healthBar, expBar); // TODO

        // sinh ra monster
        //InvokeRepeating(nameof(SpawnMonster), 0f, spawnTime);
        //Instantiate(monsterPrefab, new Vector2(5, 0), Quaternion.identity);

        // init ui
        healthBar.Init(player);
        expBar.Init(player);
    }
    void Start()
    {
        InvokeRepeating(nameof(SpawnMonster), 0.2f, spawnTime);
    }
    public void SpawnMonster()
    {
        /*while (true)
        {
            float x = Random.Range(bound.Left, bound.Right);
            float y = Random.Range(bound.Bot, bound.Top);
            Vector3 position = new Vector3(x, y);

            // kiem tra position co nam trong pham vi radius hay khong
            if ((player.transform.position - position).magnitude < radius)
            {
                var monster = Instantiate(monsterPrefab, position, Quaternion.identity);
                monster.name = $"Monster-{monsters.Count}";
                monsters.Add(monster.GetComponent<Monster>());
                break;
            }
        }
        */

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        Vector2 camPos = cam.transform.position;

        for (int i = 0; i < maxTry; i++)
        {
            int side = Random.Range(0, 4);
            Vector2 spawnPos = camPos;

            switch (side)
            {
                case 0: //tren
                    spawnPos.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                    spawnPos.y = camPos.y + camHeight - edgeOffset;
                    break;
                case 1: //duoi
                    spawnPos.x = Random.Range(camPos.x - camWidth, camPos.x + camWidth);
                    spawnPos.y = camPos.y - camHeight + edgeOffset;
                    break;
                case 2: //trai
                    spawnPos.x = camPos.x - camWidth + edgeOffset;
                    spawnPos.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                    break;
                case 3: //phai
                    spawnPos.x = camPos.x + camWidth - edgeOffset;
                    spawnPos.y = Random.Range(camPos.y - camHeight, camPos.y + camHeight);
                    break;
            }
            spawnPos.x = Mathf.Clamp(spawnPos.x, bound.Left, bound.Right);
            spawnPos.y = Mathf.Clamp(spawnPos.y, bound.Bot, bound.Top);

            if (Vector2.Distance(spawnPos, player.transform.position) < minDisToPlayer)
                continue;
            var monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
            monster.name = $"Monster-{monsters.Count}";
            monsters.Add(monster.GetComponent<Monster>());
            return;
        }
        Debug.LogWarning("Khong spawn duoc vi tri cua monster");
    }
}

[System.Serializable]
public class Bound
{
    public Transform boundLeft;
    public Transform boundRight;
    public Transform boundTop;
    public Transform boundBot;

    public float Left => boundLeft.position.x;
    public float Right => boundRight.position.x;
    public float Top => boundTop.position.y;
    public float Bot => boundBot.position.y;
}
