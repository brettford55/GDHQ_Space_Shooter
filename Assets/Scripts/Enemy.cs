using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed, _beamerSpeed, _zigzagSpeed, _zigzagDelay, r, _movementType;

    [SerializeField]
    private GameObject _laser, _smartLaser;

    [SerializeField]
    private bool _isShooting = true, _canDodge = false, _isSmart = false , _shootingBackward = false;
    [SerializeField]
    private int _enemyID; // 0

    private AudioSource _explosionSFX;

    private Player _player;

    private Animator _destroyedAnim;


    private Collider2D _enemyCollider;

    private Rigidbody2D _rb;

    private SpawnManager _spawnManager;
    bool dirRight = true;
    Vector3 direction = Vector3.down;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyedAnim = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        _explosionSFX = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
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
        if(_rb == null)
        {
            Debug.LogError("RB is Null");
        }

        _movementType = Random.Range(1f, 100f);
        if (_enemyID == 0 || _enemyID == 2) StartCoroutine(ShootLaserRoutine());
        else if (_enemyID == 1)
        {
            StartCoroutine(ShootBeamRoutine());
            StartCoroutine(ZigzagRoutine());
        }
            

        if (_movementType >= 51 && _enemyID == 0) StartCoroutine(ZigzagRoutine()); // zigzag enemy  
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementType >= 51 && _enemyID == 0) ZigzagEnemy(r);
        else if (_enemyID == 0) NormalEnemy();
        else if (_enemyID == 1) BeamerEnemy();
        else if (_enemyID == 2) SmartEnemy();
    }

    IEnumerator ShootLaserRoutine()
    {
        while (_isShooting)
        {
            float waitTime = Random.Range(3, 8);
            if(_shootingBackward == false) Instantiate(_laser, transform.position + new Vector3(0, -1.3f, 0), Quaternion.identity);
            else Instantiate(_smartLaser, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator ShootBeamRoutine()
    {
        while (_isShooting)
        {
            if (r < 50) Instantiate(_laser, transform.position + new Vector3(0, -1.3f, 0), Quaternion.identity);
            yield return new WaitForSeconds(.15f);
        }
    }
    void NormalEnemy()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void SmartEnemy()
    {
        float playerYPos = _player.GetPlayerYPos();
        if (playerYPos >= transform.position.y) 
        {
            _shootingBackward = true;
        }
        NormalEnemy();
    }
    void BeamerEnemy()
    {
        
        if (dirRight)
            transform.Translate(Vector3.right * _beamerSpeed * Time.deltaTime);
        else
            transform.Translate(Vector3.left * _beamerSpeed * Time.deltaTime);

        if (transform.position.x >= 11f)
        {
            dirRight = false;
        }

        if (transform.position.x <= -11f)
        {
            dirRight = true;
        }
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

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void Dodge()
    {
        _rb.AddForce(transform.right * 8 * Random.Range(-1,2), ForceMode2D.Impulse);
    }
   
   
    IEnumerator ZigzagRoutine()
    {
        while (true)
        {
            r = Random.Range(0, 100f);
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

