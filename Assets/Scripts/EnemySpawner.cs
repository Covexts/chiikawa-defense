/*
 * file:        EnemySpawner.cs
 * project:     Chiikawa Defense
 * description: spawns enemies on a timer. spawns get faster over time and
 *              stronger enemies unlock as the round goes on. when the enemy
 *              tower gets low, the chimera boss spawns once.
 */

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    public Unit[] enemyPrefabs;          // weakest first, strongest last
    public Unit bossPrefab;              // the chimera
    public Transform spawnPoint;         // where enemies appear
    public Tower enemyTower;             // used to know when to spawn the boss

    [Header("Timing")]
    public float spawnDelay = 4f;        // seconds between spawns at the start
    public float minDelay = 1.5f;        // fastest spawns can get
    public float speedUp = 0.1f;         // delay shrinks by this each spawn
    public int spawnsPerUnlock = 5;      // spawns before a stronger enemy unlocks

    [Header("Boss")]
    public float bossThreshold = 0.5f;   // tower health percent that spawns the boss (0.5 = 50%)

    private float timer;                 // counts up to the next spawn
    private int spawnCount;              // enemies spawned so far
    private bool bossSpawned;            // so the boss only spawns once

    // runs every frame. checks for the boss and spawns enemies on a timer.
    void Update()
    {
        // stop spawning once the enemy tower is destroyed
        if (enemyTower.IsDead() == true) {
            return;
        }

        CheckBoss();

        timer = timer + Time.deltaTime;

        // not time to spawn yet
        if (timer < spawnDelay) {
            return;
        }

        timer = 0f;
        SpawnEnemy();
    }

    // spawns one random enemy from the ones unlocked so far.
    void SpawnEnemy()
    {
        // one more enemy type unlocks every few spawns
        int unlocked = spawnCount / spawnsPerUnlock + 1;
        if (unlocked > enemyPrefabs.Length) {
            unlocked = enemyPrefabs.Length;
        }

        // pick a random enemy from the unlocked ones
        int pick = Random.Range(0, unlocked);
        Instantiate(enemyPrefabs[pick], spawnPoint.position, Quaternion.identity);

        spawnCount = spawnCount + 1;

        // spawn a little faster next time
        spawnDelay = spawnDelay - speedUp;
        if (spawnDelay < minDelay) {
            spawnDelay = minDelay;
        }
    }

    // spawns the chimera once the enemy tower drops below the threshold.
    void CheckBoss()
    {
        if (bossSpawned == true) {
            return;
        }

        if (bossPrefab == null) {
            return;
        }

        if (enemyTower.GetHealthPercent() > bossThreshold) {
            return;
        }

        Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        bossSpawned = true;
    }
}
