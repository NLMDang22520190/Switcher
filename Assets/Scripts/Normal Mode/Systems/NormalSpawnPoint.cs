using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalSpawnPoint : MonoBehaviour
{
    public static NormalSpawnPoint Instance { get; private set; }

    [SerializeField] private bool isTopLane;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float spawnPointsDistance;
    [SerializeField] private int startDelay;
    [SerializeField] private float spawnRate;

    [Header("Enemy and Coin")]
    [SerializeField] private NormalEnemy enemyPrefab;
    [SerializeField] private ParticleSystem enemyParticlePrefab;
    [SerializeField] private NormalCoin coinPrefab;
    [SerializeField] private ParticleSystem coinParticlePrefab;

    private int[,] spawnPattern;
    private bool canSpawn = true;
    private List<int[,]> spawnPatterns = new List<int[,]>();
    private Vector3[,] spawnPoints = new Vector3[2, 5];

    private Queue<NormalEnemy> enemies = new Queue<NormalEnemy>();
    private Queue<ParticleSystem> enemyParticles = new Queue<ParticleSystem>();
    private Queue<NormalCoin> coins = new Queue<NormalCoin>();
    private Queue<ParticleSystem> coinParticles = new Queue<ParticleSystem>();

    private float spawnTimer;
    private float currentSpawnPointsDistance;
    private float currentSpawnRate;

    public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
    public bool CanSpawn { get => canSpawn; set => canSpawn = value; }
    public float CurrentSpawnPointsDistance { get => currentSpawnPointsDistance; set => currentSpawnPointsDistance = value; }
    public float SpawnPointsDistance { get => spawnPointsDistance; set => spawnPointsDistance = value; }
    public float SpawnRate { get => spawnRate; set => spawnRate = value; }
    public float CurrentSpawnRate { get => currentSpawnRate; set => currentSpawnRate = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreateSpawnPatterns();
        GeneratePool();

        CurrentSpawnPointsDistance = SpawnPointsDistance;
        CurrentSpawnRate = SpawnRate;
    }

    private void Start()
    {
        InvokeRepeating("SpawnAtInterval", startDelay, SpawnRate);
    }

    private void Update()
    {
        if (canSpawn)
        {
            spawnTimer += Time.deltaTime;
        }

        MoveSpawnPoint();
    }

    private void GeneratePool()
    {
        for (int i = 0; i < 10; i++)
        {
            NormalEnemy enemy = Instantiate(enemyPrefab);
            enemy.gameObject.SetActive(false);
            enemies.Enqueue(enemy);

            ParticleSystem enemyParticle = Instantiate(enemyParticlePrefab);
            enemyParticle.gameObject.SetActive(false);
            enemyParticles.Enqueue(enemyParticle);

            NormalCoin coin = Instantiate(coinPrefab);
            coin.gameObject.SetActive(false);
            coins.Enqueue(coin);

            ParticleSystem coinParticle = Instantiate(coinParticlePrefab);
            coinParticle.gameObject.SetActive(false);
            coinParticles.Enqueue(coinParticle);
        }
    }

    private void CreateSpawnPatterns()
    {
        // Adding predefined spawn patterns
        spawnPatterns.Add(new int[,] { { 0, 1, 0, 1, 0 }, { 0, 0, 0, 0, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 0, 0, 0, 0 }, { 0, 1, 0, 1, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 2, 0, 2, 0 }, { 0, 0, 0, 0, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 0, 0, 0, 0 }, { 0, 2, 0, 2, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 0, 0, 1, 0 }, { 0, 1, 0, 0, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 1, 0, 0, 0 }, { 0, 0, 0, 1, 0 } });
        spawnPatterns.Add(new int[,] { { 2, 0, 2, 0, 2 }, { 0, 2, 0, 2, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 2, 0, 2, 0 }, { 2, 0, 2, 0, 2 } });
        spawnPatterns.Add(new int[,] { { 1, 0, 1, 0, 1 }, { 0, 0, 0, 0, 0 } });
        spawnPatterns.Add(new int[,] { { 0, 0, 0, 0, 0 }, { 1, 0, 1, 0, 1 } });
    }

    private void SelectSpawnPattern()
    {
        int randomIndex = Random.Range(0, spawnPatterns.Count);
        spawnPattern = spawnPatterns[randomIndex];
    }

    private void FindSpawnPoints()
    {
        Vector3 castDirection = isTopLane ? Vector3.down : Vector3.up;
        float castHeight = 10f;
        Vector3 startPosition_1 = transform.position + (isTopLane ? Vector3.up : Vector3.down) * castHeight;
        Vector3 startPosition_2 = transform.position + (!isTopLane ? Vector3.up : Vector3.down) * castHeight;

        for (int i = 0; i < 5; i++)
        {
            if (Physics.Raycast(startPosition_1 + i * CurrentSpawnPointsDistance * Vector3.right, castDirection, out RaycastHit hit1, 999f, GroundLayer))
            {
                spawnPoints[0, i] = hit1.point;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (Physics.Raycast(startPosition_2 + i * CurrentSpawnPointsDistance * Vector3.right, -castDirection, out RaycastHit hit2, 999f, GroundLayer))
            {
                spawnPoints[1, i] = hit2.point;
            }
        }
    }

    private void DefineSpawnPoints()
    {
        SelectSpawnPattern();

        for (int i = 0; i < spawnPattern.GetLength(1); i++)
        {
            for (int j = 0; j < spawnPattern.GetLength(0); j++)
            {
                if (spawnPattern[j, i] == 1)
                {
                    NormalEnemy enemy = enemies.Dequeue();
                    enemy.transform.position = spawnPoints[j, i];
                    enemy.transform.rotation = (j == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(180, 0, 0);
                    enemy.gameObject.SetActive(true);
                    enemies.Enqueue(enemy);
                }
                else if (spawnPattern[j, i] == 2)
                {
                    NormalCoin coin = coins.Dequeue();
                    coin.transform.position = spawnPoints[j, i];
                    coin.transform.rotation = (j == 0) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180);
                    coin.gameObject.SetActive(true);
                    coins.Enqueue(coin);
                }
            }
        }
    }

    private void SpawnAtInterval()
    {
        if (spawnTimer >= CurrentSpawnRate)
        {
            Debug.Log("Spawning");

            FindSpawnPoints();
            DefineSpawnPoints();

            spawnTimer = 0f;
        }
    }

    public void ReturnCoin(NormalCoin coin)
    {
        coin.gameObject.SetActive(false);
        coins.Enqueue(coin);
    }

    public void ReturnEnemy(NormalEnemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemies.Enqueue(enemy);
    }

    public void GetCoinParticle(Vector3 position)
    {
        ParticleSystem coinParticle;

        if (coinParticles.Count == 0)
        {
            coinParticle = Instantiate(coinParticlePrefab);
            coinParticle.gameObject.SetActive(false);
            coinParticles.Enqueue(coinParticle);
        }

        coinParticle = coinParticles.Dequeue();
        coinParticle.transform.position = position;
        coinParticle.gameObject.SetActive(true);
        coinParticle.Play();

        StartCoroutine(ReturnParticle(coinParticle, coinParticle.main.duration));
    }

    public void GetEnemyPartical(Vector3 position)
    {
        ParticleSystem enemyParticle;

        if (enemyParticles.Count == 0)
        {
            enemyParticle = Instantiate(enemyParticlePrefab);
            enemyParticle.gameObject.SetActive(false);
            enemyParticles.Enqueue(enemyParticle);
        }

        enemyParticle = enemyParticles.Dequeue();
        enemyParticle.transform.position = position;
        enemyParticle.gameObject.SetActive(true);
        enemyParticle.Play();

        StartCoroutine(ReturnParticle(enemyParticle, enemyParticle.main.duration));
    }

    private IEnumerator ReturnParticle(ParticleSystem coinParticle, float delay)
    {
        yield return new WaitForSeconds(delay);
        coinParticle.Stop();
        coinParticle.gameObject.SetActive(false);
        coinParticles.Enqueue(coinParticle);
    }

    private void MoveSpawnPoint()
    {
        Player player = Player.Instance;
        int spawnDistance = 20;
        transform.position = new Vector3(player.transform.position.x + spawnDistance, player.transform.position.y, transform.position.z);
    }
}
