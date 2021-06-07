using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_Shot : MonoBehaviour
{

    [SerializeField] private float _speed, _rotateSpeed;

    private Rigidbody2D _rb;
    private GameObject _nearestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GoToEnemy();
    }
    private void GoToEnemy()
    {
        StartCoroutine(FindEnemyRoutine());

        if (_nearestEnemy != null)
        {
            Transform target = _nearestEnemy.transform;
            Vector2 direction = (Vector2)target.position - _rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rb.angularVelocity = -rotateAmount * _rotateSpeed;

            _rb.velocity = transform.up * _speed;
        }
        else
        {
            transform.position = transform.position;  //stays in same place, eventually will hover next to player
        }
       

    } 
    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;
        Vector3 ourPosition = transform.position;

        foreach(GameObject enemy in enemies) //goes through each enemy
        {
            Vector3 difference = enemy.transform.position - ourPosition;
            float currentEnemyDistance = difference.sqrMagnitude;

            if(currentEnemyDistance < closestEnemyDistance)
            {
                closestEnemy = enemy;
                closestEnemyDistance = currentEnemyDistance;
            }
        }

        return closestEnemy;
    }
    IEnumerator FindEnemyRoutine()
    {
        _nearestEnemy = FindNearestEnemy();
        yield return new WaitForSeconds(2f);
    }
}

