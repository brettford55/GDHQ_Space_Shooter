using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _EnemyContainer;

    [SerializeField]
    private GameObject[] _powerUps;

    [SerializeField]
    private float _enemySpawnTimer;

    [SerializeField]
    private bool _stopSpawning = false;


    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine(_enemySpawnTimer));
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnRoutine(float waitTime)
    {
        yield return new WaitForSeconds(2);//spawn delay
        
        while (_stopSpawning == false)
        {
            float randx = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randx, 11, 0), Quaternion.identity);
            newEnemy.transform.parent = _EnemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(5);//spawn delay

        while (_stopSpawning == false)
        {

            float randx = Random.Range(-8f, 8f);
            int randomPowerUp = Random.Range(0,4);
            Instantiate(_powerUps[randomPowerUp], new Vector3(randx, 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
     
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        Debug.Log("Switch to true");
    }
    

}

