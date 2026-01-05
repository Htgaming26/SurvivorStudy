using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // ===== TIME =====
    float gameTime;
    float spawnTimer;
    public float spawmTimer = 1.5f;

    // ===== WAVE =====
    public int wave = 1;
    float waveTimer;
    public float waveDuration = 60f;

    // ===== SPAWN COUNT =====
    int spawnCount = 1;

    // ===== SPAWN SPEED =====
    public float baseDelay = 2f;
    public float minDelay = 0.3f;
    public float timeFactor = 0.015f;
    public float waveFactor = 0.1f;

    
    [SerializeField] Program program;

    void Start()
    {
       
    }

    void Update()
    {
        UpdateTime();
        UpdateWave();
        UpdateSpawnCount();
        HandleSpawn();
    }

    // ⏱ Tổng thời gian chơi
    void UpdateTime()
    {
        gameTime += Time.deltaTime;
    }

    // 🌊 Mỗi 1phut tăng 1 wave
    void UpdateWave()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveDuration)
        {
            wave++;
            waveTimer = 0;
        }
    }

    // 👾 Mỗi t giay tăng thêm 1 quái / lần spawn
    void UpdateSpawnCount()
    {
        spawnCount = 1 + (int)(gameTime / 20f);
    }

    // 🔥 Spawn logic chính
    void HandleSpawn()
    {
        float spawnDelay = baseDelay
                         - gameTime * timeFactor
                         - wave * waveFactor;

        spawnDelay = Mathf.Max(minDelay, spawnDelay);

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnDelay)
        {
            SpawnWavePack();
            //Invoke(nameof(SpawnWavePack), 5f);
            spawnTimer = 0;
        }
    }

    // 📦 Mỗi lần spawn = spawnCount con
    void SpawnWavePack()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            program.SpawnMonster();
            Debug.Log($"Spawn monster | Wave {wave} | Count {spawnCount}");
        }
    }
}
