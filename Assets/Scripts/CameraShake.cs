using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin _noise;
    private float shakeTime;

    void Awake()
    {
        _noise = GetComponent<CinemachineVirtualCamera>().
            GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(_noise == null)
        {
            Debug.LogError("CameraShake is Null");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime <= 0f)
            {
                _noise.m_AmplitudeGain = 0f;
            }
        }
    }
    public void ShakeCamera(float intensity, float time)
    {
        _noise.m_AmplitudeGain = intensity;
        shakeTime = time;
    }
}
