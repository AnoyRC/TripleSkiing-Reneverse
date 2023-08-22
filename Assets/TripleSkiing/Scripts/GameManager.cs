using UnityEngine;
using System.Collections;

public enum GameState
{
play,
pause,
gameOver}
;

public class GameManager : MonoBehaviour
{

	public const string BESTSCORE_KEY = "bestscore";
	public const string GAMEPLAY_SCENE = "GamePlay";
	public const string MAINMENU_SCENE = "MainMenu";

	public PlayerController _playerCtroller;
	public UIManager _uiManager;
	public GameState gameState;
	public int score;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;
		setGameState (GameState.play);
	}

	public void setGameState (GameState state)
	{
		gameState = state;
	}

	public void bonus ()
	{
		score++;
		_uiManager.updateScore (score);
	}

	public void gameOver ()
	{
		_uiManager.gameOver ();
		setGameState (GameState.gameOver);
		if (score > PlayerPrefs.GetInt (BESTSCORE_KEY)) {
			PlayerPrefs.SetInt (BESTSCORE_KEY, score);
		}
	}

	public void pauseGame ()
	{
		setGameState (GameState.pause);
		_playerCtroller.onEffect (false);
	}

	public void skion ()
	{
		setGameState (GameState.play);
		_playerCtroller.onEffect (true);
	}
}
