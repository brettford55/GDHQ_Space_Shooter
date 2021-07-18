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
    private GameObject[] _enemyType; // 0 = Normal  1 == Beamer 2 Dodge, 3 Smart


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
    public int AssignEnemyType()
    {
        int r = (int)Random.Range(0, _enemyType.Length);
        return r;
    }

    IEnumerator SpawnRoutine(float waitTime)
    {
        yield return new WaitForSeconds(2);//spawn delay
        
        while (_stopSpawning == false)
        {
            float randx = Random.Range(-8f, 8f);
            GameObject newEnemy = null;
            int i = AssignEnemyType();
            switch (i)
            {
                case 0: 
                    newEnemy = Instantiate(_enemyType[i], new Vector3(randx, 11, 0), Quaternion.identity);
                    break;
                case 1:
                    if(randx < 0) newEnemy = Instantiate(_enemyType[i], new Vector3(11.3f, 2, 0), Quaternion.identity);
                    else newEnemy = Instantiate(_enemyType[i], new Vector3(-11.3f, 2, 0), Quaternion.identity);
                    break;
                case 2:
                    newEnemy = Instantiate(_enemyType[i], new Vector3(randx, 11, 0), Quaternion.identity);
                    break;
                case 3:
                    Debug.Log("Smart enemy spawned");
                    newEnemy = Instantiate(_enemyType[i], new Vector3(randx, 11, 0), Quaternion.identity);
                    break;
                case 4:
                    newEnemy = Instantiate(_enemyType[i], new Vector3(randx, 11, 0), Quaternion.identity);
                    break;
                default:
                    Debug.LogError("No Enemy spawned");
                    break;
            }
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

