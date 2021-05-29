using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private int _speed = 3;

    [SerializeField] // 0 is triple shot, 1 is speed boost, 2 is shield
    private int powerUpID;

    private Player _player;
    private AudioSource __powerUpSFX;





    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        __powerUpSFX = GameObject.Find("Power_Up_SFX").GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        if (__powerUpSFX == null)
        {
            Debug.LogError("Power Up SFX is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
       transform.Translate(Vector3.down * _speed * Time.deltaTime);
       if(transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    //Ontriggercolliso
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (powerUpID)
            {
                case 0:
                    _player.TripleShotActive();
                    break;
                case 1:
                     _player.SpeedBoostActive();
                    break;
                case 2:
                     _player.ShieldActiveTrue();
                    break;
                case 3:
                    _player.AddAmmo();
                    break;
                case 4:
                    _player.AddLife();
                    break;
                default:
                    Debug.Log("Invalid powerUpID");
                    break;
            }
            
            __powerUpSFX.Play();
             Destroy(this.gameObject);
        }

    }
   
}
