using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoreTXT, _ammoTXT;
    [SerializeField]
    private GameObject _gameOverTXT, _restartTXT;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImage;
    private Slider _slider;
    private GameManager _gameManager;
    void Awake()
    {
        _scoreTXT.text = "Score: 0";
        _ammoTXT.text = "Ammo: 15";

        _slider = GameObject.Find("Thruster_UI").GetComponent<Slider>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
        if(_slider == null)
        {
            Debug.LogError("Slider is Null");
        }  
    }

    public void UpdateThrusterBar(float thrusterUsed)
    {
        _slider.value = thrusterUsed;
    }
    public void SetThrusterSlider(int min, int max)
    {
        _slider.minValue = min;
        _slider.maxValue = max;
    }

    public void UpdateScore(int score)
    {
        _scoreTXT.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives < 1)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmo(int ammo)
    {
        if(ammo < 0)
        {
            ammo = 0;
        }
        _ammoTXT.text = "Ammo: " + ammo.ToString();
    }

    private void GameOverSequence()
    {
        StartCoroutine(FlickerRoutine());
        _restartTXT.SetActive(true);
        _gameManager.GameOver();
    }

    IEnumerator FlickerRoutine()
    {
        while(true)
        {
            _gameOverTXT.SetActive(true);

            yield return new WaitForSeconds(.5f);

            _gameOverTXT.SetActive(false);

            yield return new WaitForSeconds(.5f);

        }

    }
}
