using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _minSpeed = 3, _currentSpeed,_speedBoostMultiplyer, _acceleration, _maxSpeed;


    [SerializeField]
    private float _fireRate = 0.5f, _canFire;

    [SerializeField]
    private bool _tripleShotActive = false, _speedBoostActive = false, _shieldActive = false;

    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _shieldVisualizer;


    [SerializeField]
    private GameObject _rightTailDmg, _leftTailDmg;

    [SerializeField]
    private AudioSource _laserSFX;


    [SerializeField]
    private int _score, _lives;

    private SpawnManager _spawnManager;
    private UIManager _UIManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is Null");
        }

        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UIManager is Null");
        }

        _laserSFX = _laserSFX.GetComponent<AudioSource>();
        if (_laserSFX == null)
        {
            Debug.LogError("Laser SFX is Null");
        }

        _currentSpeed = _minSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        ShootLaser(); //When Space is Pressed 
    }

    private void DamageVisualizer()
    {
        switch (_lives)
        {
            case 2:
                _rightTailDmg.SetActive(true);
                break;
            case 1:
                _leftTailDmg.SetActive(true);
                break;
        }
    }


    void ShootLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            
            if(_tripleShotActive == false)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
            }

            _laserSFX.Play();
        }
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        CalculateCurrentSpeed();
        
        if(_speedBoostActive == true)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * (_currentSpeed *_speedBoostMultiplyer)  );
        }
        else
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * _currentSpeed);
        }
        
        //y bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    private void CalculateCurrentSpeed()
    {
        StartCoroutine(AccelerationDelayRoutine());
        if (Input.GetKey(KeyCode.LeftShift) && _currentSpeed < _maxSpeed)
        {
            float newSpeed = _currentSpeed + _acceleration * Time.deltaTime;
            _currentSpeed = newSpeed;
        }
        else if (_currentSpeed > _minSpeed && Input.GetKey(KeyCode.LeftShift) == false)
        {
            _currentSpeed -= _acceleration * Time.deltaTime;
        }
        
        //set min and max because update method make _current speed slightly more or less than given min and max
        else if(Input.GetKey(KeyCode.LeftShift) && _currentSpeed > _maxSpeed) 
        {
            _currentSpeed = _maxSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) == false && _currentSpeed < _minSpeed)
        {
            _currentSpeed = _minSpeed;
        }
    }

    IEnumerator AccelerationDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
    }

    


    public void LoseLife()
    {
       if(_shieldActive == true)
        {
            _shieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives -= 1;
        DamageVisualizer();
        _UIManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void AddToScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
    }


    public void TripleShotActive()
    {
        _tripleShotActive = true;
        Debug.Log("Triple Shot is true");
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void ShieldActiveTrue()
    {
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        Debug.Log("Speed Boost is true");
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotActive = false;
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
       _speedBoostActive = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy Laser")
        {
            LoseLife();
            Destroy(collision.gameObject);
        }
    }


}
