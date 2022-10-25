using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class EnemyWave
{
    // 0 - soilderEnemy, 1 - kamazUnit, 2 - tigerUnit, 3 - btrUnit, 4 - tankUnit,

    // list of lists of different enemys on the start wave
    public static List<List<int>> EnemyListStart = new List<List<int>>
    {
        new List<int> { 0, 3, 0, 2, 1 },
        new List<int> { 0, 2, 0, 2, 2 },
        new List<int> { 0, 1, 0, 1, 3 },
    };

    ////////////////////////////////////////////////////////////////////////////////////

    // list of lists of different enemys on the first stage
    public static List<List<int>> EnemyListFirstStage = new List<List<int>>
    {
        new List<int> { 0, 2, 3, 0, 1 },
        new List<int> { 0, 1, 1, 1, 2 },
        new List<int> { 0, 0, 3, 2, 1 },
        new List<int> { 0, 1, 2, 2, 1 },
    };

    ////////////////////////////////////////////////////////////////////////////////////

    // list of lists of different enemys on the second stage part 1
    public static List<List<int>> EnemyListSecondStagePart1 = new List<List<int>>
    {
        new List<int> { 0, 3, 1, 1, 2 },
        new List<int> { 0, 2, 2, 2, 1 },
        new List<int> { 0, 1, 3, 2, 1 },
        new List<int> { 0, 0, 2, 2, 3 },
    };

    // list of lists of different enemys on the second stage part 2
    public static List<List<int>> EnemyListSecondStagePart2 = new List<List<int>>
    {
        new List<int> { 0, 4, 1, 2, 1 },
        new List<int> { 0, 3, 1, 1, 3 },
        new List<int> { 0, 2, 3, 2, 2 },
        new List<int> { 0, 1, 2, 2, 2 },
    };

    // list of lists of different enemys on the second stage part 3
    public static List<List<int>> EnemyListSecondStagePart3 = new List<List<int>>
    {
        new List<int> { 0, 2, 3, 3, 3 },
        new List<int> { 0, 3, 1, 2, 4 },
        new List<int> { 0, 3, 2, 2, 2 },
        new List<int> { 0, 2, 1, 3, 3 },
    };

    ////////////////////////////////////////////////////////////////////////////////////

    // list of lists of different enemys on the third stage part 1
    public static List<List<int>> EnemyListThirdStagePart1 = new List<List<int>>
    {
        new List<int> { 3, 0, 2, 3, 4 },
        new List<int> { 4, 0, 3, 4, 2 },
        new List<int> { 4, 0, 2, 3, 3 },
        new List<int> { 3, 0, 4, 2, 3 },
    };

    // list of lists of different enemys on the third stage part 2
    public static List<List<int>> EnemyListThirdStagePart2 = new List<List<int>>
    {
        new List<int> { 4, 0, 3, 3, 4 },
        new List<int> { 5, 0, 4, 4, 2 },
        new List<int> { 5, 0, 3, 3, 3 },
        new List<int> { 4, 0, 5, 2, 3 },
    };

    // list of lists of different enemys on the third stage part 3
    public static List<List<int>> EnemyListThirdStagePart3 = new List<List<int>>
    {
        new List<int> { 4, 0, 2, 4, 4 },
        new List<int> { 6, 0, 3, 5, 2 },
        new List<int> { 6, 0, 2, 4, 3 },
        new List<int> { 5, 0, 4, 3, 3 },
    };

    ////////////////////////////////////////////////////////////////////////////////////

    // list of lists of different enemys on the forth stage
    public static List<List<int>> EnemyListForthStage = new List<List<int>>
    {
        new List<int> { 5, 0, 2, 2, 1 },
        new List<int> { 4, 0, 1, 3, 2 },
        new List<int> { 5, 0, 1, 2, 2 },
        new List<int> { 6, 0, 1, 1, 1 },
    };
}*/



public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    [SerializeField] int TotalEnemyCount;
    //Towercount for each type
    public List<int> EnemyCounts;
    public List<int> CurrentEnemyCounts;
    //Enemy prefabs
    public List<GameObject> prefabs;
    //Enemy spawn root point
    public List<Transform> spawnPoints;
    //Enemy spawn interval
    public float spawnInterval = 2f;

    public void StartSpawning()
    {
        CurrentEnemyCounts = new List<int>();
        for (int c = 0; c < EnemyCounts.Count; c++)
        {
            TotalEnemyCount += EnemyCounts[c];
            CurrentEnemyCounts.Add(0);
        }
        //Call the spawn coroutine
        StartCoroutine(SpawnDelay());
    }

    IEnumerator SpawnDelay()
    {
        int t = 0;
        for (int c = 0; c < CurrentEnemyCounts.Count; c++)
            t += CurrentEnemyCounts[c];
        while (t < TotalEnemyCount)
        {
            //Call the spawn method
            t += SpawnEnemy();
            Debug.Log(t);
            //Wait spawn interval
            yield return new WaitForSeconds(spawnInterval);
            //Recall the same coroutine
            //StartCoroutine(SpawnDelay());
        }
    }

    int SpawnEnemy()
    {
        //Randomize the enemy spawned
        int randomPrefabID = Random.Range(0,prefabs.Count);
        if (CurrentEnemyCounts[randomPrefabID] < EnemyCounts[randomPrefabID])
        {
            CurrentEnemyCounts[randomPrefabID]++;
            //Randomize the spawn point
            int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
            //Instantiate the enemy prefab
            GameObject spawnedEnemy = Instantiate(prefabs[randomPrefabID], spawnPoints[randomSpawnPointID]);
        }
        else if(CurrentEnemyCounts[randomPrefabID] > EnemyCounts[randomPrefabID] * 0.6)
        {
            for(int c = 0; c < CurrentEnemyCounts.Count; c++)
            {
                if(EnemyCounts[c] - CurrentEnemyCounts[c] > 0)
                {
                    int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
                    //Instantiate the enemy prefab
                    GameObject spawnedEnemy = Instantiate(prefabs[c], spawnPoints[randomSpawnPointID]);
                }

            }
        }
        else
            SpawnEnemy();
        return 1;
    }
}
