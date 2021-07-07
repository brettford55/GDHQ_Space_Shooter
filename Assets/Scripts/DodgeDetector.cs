using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeDetector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _dodgeRecover = 2f, 
        _nextDodge = 0.0f;

    Enemy _enemy;
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
        if(_enemy == null)
        {
            Debug.LogError("Enemy is Null");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser" )
        {
            _enemy.Dodge();
            Debug.Log("Dodge activated");
        }
    }
}
