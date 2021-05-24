using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 5;

    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;

    private AudioSource _explosionSFX;


    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _explosionSFX = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is Null");
        }
        if (_explosionSFX == null)
        {
            Debug.LogError("Explosion SFX is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -1) * _rotateSpeed * Time.deltaTime);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_explosionSFX.clip, transform.position + new Vector3(0,0,-10));
            _spawnManager.StartSpawning();
            Destroy(this.gameObject);
            Destroy(collision.gameObject,.2f);
        }
    }
}
