using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed, _zigzagSpeed, _zigzagDelay, r;

    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private bool _isShooting = true;

    string _enemyType;

    private AudioSource _explosionSFX;

    private Player _player;

    private Animator _destroyedAnim;

    private Collider2D _enemyCollider;

    private SpawnManager _spawnManager;
    Vector3 direction = Vector3.down;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyedAnim = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        _explosionSFX = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        StartCoroutine(ShootLaserRoutine());

        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        if (_destroyedAnim == null)
        {
            Debug.LogError("Destroy ANIM is null");
        }
        if (_enemyCollider == null)
        {
            Debug.LogError("Collider is null");
        }
        if (_explosionSFX == null)
        {
            Debug.LogError("Explosion SFX is null");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn mnager is Null");
        }

        //Call assign enemy from SpawnManager
        _enemyType = _spawnManager.AssignEnemyType();
        if(_enemyType == "Zigzag") StartCoroutine(ZigzagRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyType == "Zigzag") ZigzagEnemy(r);
        else NormalEnemy();

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
  

    IEnumerator ShootLaserRoutine()
    {
        while (_isShooting)
        {
            float waitTime = Random.Range(3, 8);
            Instantiate(_laser, transform.position + new Vector3(0, -1.3f, 0), Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void NormalEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void ZigzagEnemy(float r)
    {
        _zigzagDelay -= Time.deltaTime;
        if (_zigzagDelay <= 0)
        {
            
            if (r <= 30)
            {
                direction = Vector3.left;
            }
            else if (r >= 60)
            {
                direction = Vector3.down;
            }
            else
            {
                direction = Vector3.right;
            }
            _zigzagDelay += 1;
        }
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    IEnumerator ZigzagRoutine()
    {
        while (true)
        {
            r = Random.Range(0, 90f);
            yield return new WaitForSeconds(1f);
        }
       
    }

  

   


    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.tag == "Player")
        {
            _player.LoseLife();
            StartCoroutine(EnemyDestroyRoutine());
            
        }
        else if (col.tag == "Laser")
        {
            _player.AddToScore(10);
            Destroy(col.gameObject);
            StartCoroutine(EnemyDestroyRoutine());
        }


    }

    IEnumerator EnemyDestroyRoutine()
    {
        _destroyedAnim.SetTrigger("OnEnemyDeath");
        _explosionSFX.Play();
        _isShooting = false;
        _enemyCollider.enabled = false;
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}

