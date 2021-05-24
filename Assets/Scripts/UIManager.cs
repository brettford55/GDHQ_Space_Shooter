using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text _scoreTXT;

    [SerializeField]
    private GameObject _gameOverTXT, _restartTXT;
    
    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Image _livesImage;

    private GameManager _gameManager;

    
    
    
    void Start()
    {
        _scoreTXT.text = "Score: 0";

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }
    }

    // Update is called once per frame
   

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
