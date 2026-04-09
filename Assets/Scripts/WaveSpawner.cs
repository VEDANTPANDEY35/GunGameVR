using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public float spawnDelay = 3f;
    public int enemiesPerWave = 3;

    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    System.Collections.IEnumerator SpawnWave()
    {
        while (true)
        {
            // wait until all enemies are dead
            while (enemiesAlive > 0)
                yield return null;

            // spawn new wave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);

        enemiesAlive++;

        // track death
        enemy.GetComponent<Target>().OnDeath += OnEnemyDeath;
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;
    }
}