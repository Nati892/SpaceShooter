using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject SpawnedEnemy = null;

    [SerializeField]
    private GameObject[] PowerUpsPrefabs;

    [SerializeField]
    private float EnemySpawnInterval = 1;

    [SerializeField]
    private Vector2 PowerUpSpawnInterval;

    [SerializeField]
    private Vector2 X_axis_range = new Vector2(-5.0f, 5.0f);

    [SerializeField]
    private float Y_axis_pos = 8f;

    [SerializeField]
    private GameObject EnemyContainer;

    [SerializeField]
    private GameObject PowerUpsContainer;

    private bool _Spawn_Enemies = true;
    // Start is called before the first frame update
    void Start()
    {
        if (PowerUpsPrefabs != null)
            StartCoroutine(SpawnPowerUp_TripleShot());
        if (SpawnedEnemy != null)
            StartCoroutine(SpawnEnemies());
    }



    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemies()
    {
        while (_Spawn_Enemies)
        {
            float x_pos = Random.Range(X_axis_range.x, X_axis_range.y);
            var new_enemy = Instantiate(SpawnedEnemy, new Vector3(x_pos, Y_axis_pos, 0), Quaternion.identity);
            if (EnemyContainer != null)
            {
                new_enemy.transform.parent = EnemyContainer.transform;
            }
            yield return new WaitForSeconds(EnemySpawnInterval);
        }
    }

    IEnumerator SpawnPowerUp_TripleShot()
    {
        while (_Spawn_Enemies)
        {
            float x_pos = Random.Range(X_axis_range.x, X_axis_range.y);
            int PowerUpIndexToSpawn = Random.Range(0, PowerUpsPrefabs.Length);
            var new_tmp = Instantiate(PowerUpsPrefabs[PowerUpIndexToSpawn], new Vector3(x_pos, Y_axis_pos, 0), Quaternion.identity);
            if (PowerUpsContainer != null)
            {
                new_tmp.transform.parent = PowerUpsContainer.transform;
            }
            float tmp_time = EnemySpawnInterval;
            if (PowerUpSpawnInterval != null)
            {
                tmp_time = Random.Range(PowerUpSpawnInterval.x, PowerUpSpawnInterval.y);
            }
            yield return new WaitForSeconds(tmp_time);
        }
    }

    public void ReportPlayerDeath()
    {
        _Spawn_Enemies = false;
        CleanUpEnemies();
        CleanUpPowerUps();
    }

    public void ReportPlayerSpawned()
    {
        _Spawn_Enemies = true;
    }

    private void CleanUpEnemies()
    {
        //clean up enemies from scene
        if (EnemyContainer != null)
        {
            foreach (Transform t in EnemyContainer.transform)
            {
                Destroy(t.gameObject);
            }
        }
        else
        {
            Debug.LogError("SpawnManager::CleanUpEnemies Cant clean enemies, EnemyContainer is null");
        }

    }

    private void CleanUpPowerUps()
    {
        //clean up enemies from scene
        if (PowerUpsContainer != null)
        {
            foreach (Transform t in PowerUpsContainer.transform)
            {
                Destroy(t.gameObject);
            }
        }
        else
        {
            Debug.LogError("SpawnManager::CleanUpEnemies Cant clean enemies, PowerUpsContainer is null");
        }

    }
}
