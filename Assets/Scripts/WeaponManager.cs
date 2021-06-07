using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _weaponPrefabs; // 0 = laser, 1 = tripleshot, 2 = droneshot

    [SerializeField]
    private int  _ammo = 15, _weaponID = 0;

    [SerializeField]
    private float _fireRate = 0.5f, _canFire;

    [SerializeField]
    private float  _tripleShotCoolDown, _droneShotCoolDown;

    [SerializeField]
    private AudioSource _laserSFX;

    private int _ammoUsed = 1;



    private UIManager _UIManager;
    // Start is called before the first frame update
    void Start()
    {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UIManager is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            Shoot();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _weaponID == 2) //droneshot should be able to
        {                                                                                   // shoot when ammo = 0
            Shoot();
        }
    }

    void Shoot()
    {
        _canFire = Time.time + _fireRate;
        //Shoots proper weapon and takes away proper amount of ammo
        Instantiate(_weaponPrefabs[_weaponID], transform.position + new Vector3(0, 0.9f, 0), Quaternion.identity);
        _ammo -= _ammoUsed;
           
        _UIManager.UpdateAmmo(_ammo);
        _laserSFX.Play();   
    }

    public void AddAmmo()
    {
        _ammo += 3;
        _UIManager.UpdateAmmo(_ammo);
    }

    public void TripleShotActive()
    {
        _weaponID = 1; //used to be _isTripleShotActive = True
        _ammoUsed = 3;
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    public void DroneShotActive()
    {
        _weaponID = 2;
        _ammoUsed = 0; 
        StartCoroutine(DroneShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_tripleShotCoolDown);
        _weaponID = 0;
        _ammoUsed = 1;
    }

    IEnumerator DroneShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_droneShotCoolDown);
        _weaponID = 0;
        _ammoUsed = 1;
    }
}
