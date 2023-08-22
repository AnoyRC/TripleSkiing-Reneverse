using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameManager _gameManager;
    public Text scoreText;
    public Text scoreOver;
    public Text bestScore;
    public GameObject tableOver;
    public GameObject tablePause;
    public GameObject tutorial;

	// Use this for initialization
	void Start () {
        StartCoroutine(hideTutorial());
	}

    IEnumerator hideTutorial()
    {
        yield return new WaitForSeconds(1);
        tutorial.SetActive(false);
    }
	
	public void reset () {
        Time.timeScale = 1;
        SoundManager.instance.soundBackgroundPlay();
        Application.LoadLevel(GameManager.GAMEPLAY_SCENE);
	}

    public void updateScore(int score)
    {
        scoreText.text = "" + score;
    }

    public void skion()
    {
        Time.timeScale = 1;
        _gameManager.skion();
        tablePause.SetActive(false);
    }

    public void pauseGame()
    {
        _gameManager.pauseGame();
        Time.timeScale = 0;
        tablePause.SetActive(true);
    }

    public void gameOver()
    {
        Camera.main.GetComponent<ShakeCamera>().DoShake();
        StartCoroutine(delayOver());
    }

    IEnumerator delayOver()
    {
        yield return new WaitForSeconds(1);
        SoundManager.instance.playSoundGameOver();
        scoreText.transform.parent.gameObject.SetActive(false);
        tableOver.SetActive(true);
        scoreOver.text = scoreText.text;
        bestScore.text = PlayerPrefs.GetInt(GameManager.BESTSCORE_KEY) + "";
        Time.timeScale = 1;
    }

    public void mainMenu()
    {
        Time.timeScale = 1;
        SoundManager.instance.soundBackgroundMenu();
        Application.LoadLevel(GameManager.MAINMENU_SCENE);
    }
}
