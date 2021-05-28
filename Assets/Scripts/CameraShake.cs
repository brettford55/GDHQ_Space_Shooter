using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    private CinemachineVirtualCamera cinemachineCam1;
    private CinemachineBasicMultiChannelPerlin channelPerlinCam1;

    private float shakeTime;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        cinemachineCam1 = GetComponent<CinemachineVirtualCamera>();
        channelPerlinCam1 = cinemachineCam1.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(cinemachineCam1 == null || channelPerlinCam1 == null)
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
                channelPerlinCam1.m_AmplitudeGain = 0f;
            }
        }
       

    }


    public void ShakeCamera(float intensity, float time)
    {
        channelPerlinCam1.m_AmplitudeGain = intensity;
        shakeTime = time;
    }
}
