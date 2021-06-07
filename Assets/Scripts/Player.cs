using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _minSpeed = 3, _currentSpeed,_speedBoostMultiplyer, _acceleration, _maxSpeed;
    //[SerializeField]
   // private float _fireRate = 0.5f, _canFire;
    [SerializeField]
    private bool _tripleShotActive = false, _speedBoostActive = false, _shieldActive = false;
    [SerializeField]
    private GameObject /*_laserPrefab, _tripleShotPrefab,*/ _shieldVisualizer;
    [SerializeField]
    private GameObject _rightTailDmg, _leftTailDmg;
    [SerializeField]
    private AudioSource _laserSFX;
    [SerializeField]
    private int _score, _lives, _shieldLives, _ammo;

    private SpawnManager _spawnManager;
    private UIManager _UIManager;
    private SpriteRenderer _shieldRenderer;
    private CameraShake _cameraShake;

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

        _cameraShake = GameObject.Find("CM vcam1").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.LogError("Camera Shake is null");
        }
        

        _laserSFX = _laserSFX.GetComponent<AudioSource>();
        if (_laserSFX == null)
        {
            Debug.LogError("Laser SFX is Null");
        }
      
        _shieldRenderer = _shieldVisualizer.GetComponent<SpriteRenderer>();
        if (_shieldRenderer == null)
        {
            Debug.LogError("Shield Color is Null");
        }

        _currentSpeed = _minSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        //ShootLaser(); //When Space is Pressed 
    }
    private void DamageVisualizer()
    {
        switch (_lives)
        {
            case 3:
                _rightTailDmg.SetActive(false);
                break;
            case 2:
                _rightTailDmg.SetActive(true);
                _leftTailDmg.SetActive(false);
                break;
            case 1:
                _leftTailDmg.SetActive(true);
                break;
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
    public void AddLife()
    {
        if(_lives < 3)
        {
             _lives += 1;
            DamageVisualizer();
            _UIManager.UpdateLives(_lives);
        }
    }
    public void LoseLife()
    {
       if(_shieldActive == true)
        {
            _cameraShake.ShakeCamera( 1f, .2f);
            ShieldVisualizer();
            return;
        }

        _lives -= 1;
        _cameraShake.ShakeCamera(2f, .4f);
        DamageVisualizer();
        _UIManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _cameraShake.ShakeCamera(3f, .6f);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    public void ShieldVisualizer()
    {
        switch (_shieldLives)
        {
            case 3:
                _shieldLives -= 1;
                _shieldRenderer.color = new Color(255,255,255,.5f);
                break;
            case 2:
                _shieldLives -= 1;
                _shieldRenderer.color = new Color(255, 255, 255, .2f);
                break;
            case 1:
                _shieldLives -= 1;
                _shieldActive = false;
                _shieldVisualizer.SetActive(false);
                break;
        }
    }
    public void AddToScore(int points)
    {
        _score += points;
        _UIManager.UpdateScore(_score);
    }
    public void ShieldActiveTrue()
    {
        _shieldLives = 3;
        _shieldRenderer.color = new Color(255, 255, 255, 1f);
        _shieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        Debug.Log("Speed Boost is true");
        StartCoroutine(SpeedBoostPowerDownRoutine());
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
