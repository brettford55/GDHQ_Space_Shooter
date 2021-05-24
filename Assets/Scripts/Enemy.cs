using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private GameObject _laser;

    [SerializeField]
    private bool _isShooting = true;
    
    private AudioSource _explosionSFX;

    private Player _player;

    private Animator _destroyedAnim;

    private Collider2D _enemyCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _destroyedAnim = GetComponent<Animator>();
        _enemyCollider = GetComponent<Collider2D>();
        _explosionSFX = GetComponent<AudioSource>();

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

    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        
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

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        float randx = Random.Range(-9, 10);
        if (transform.position.y < -6)
        {
            transform.position = new Vector3(randx, 7, 0);
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

