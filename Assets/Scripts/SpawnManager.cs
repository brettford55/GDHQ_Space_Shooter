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

    [SerializeField]
    private string[] _enemyType;


    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine(_enemySpawnTimer));
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private bool DoesPowerUpSpawn(int powerUp)
    {
        float randx = Random.Range(0, 100f);
        if (powerUp < 2)
        {
            return true;
        }
        else if(powerUp < 4)
        {
            if(randx <= 80)
            {
                return true;
            }
            else
            {
                Debug.Log(" Medium tier item did not get spawned");
                return false;
            }
        }
        else
        {
            if (randx <= 50)
            {
                return true;
            }
            else
            {
                Debug.Log("Rare item did not get spawned");
                return false;
            }
        }

    }

    public string AssignEnemyType()
    {
        float r = Random.Range(1f, 100f);
        int i = 0; // normal enemy

        if(r >= 51) // zigzag enemy
        {
            i = 1; // zigzag
        }

        return _enemyType[i];
    }

    IEnumerator SpawnRoutine(float waitTime)
    {
        yield return new WaitForSeconds(2);//spawn delay
        
        while (_stopSpawning == false)
        {
            float randx = Random.Range(-8f, 8f);
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(randx, 11, 0), Quaternion.identity);
            AssignEnemyType();
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
            int randomPowerUp = Random.Range(0,_powerUps.Length);
           
            bool _isSpawning = DoesPowerUpSpawn(randomPowerUp);
            if(_isSpawning == true)
            {
                Instantiate(_powerUps[randomPowerUp], new Vector3(randx, 7, 0), Quaternion.identity);
            }
          
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
     
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        Debug.Log("Switch to true");
    }
    

}

