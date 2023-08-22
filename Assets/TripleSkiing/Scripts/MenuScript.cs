using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void playGame () {
        SoundManager.instance.soundBackgroundPlay();
        Application.LoadLevel(GameManager.GAMEPLAY_SCENE);
	}
	
}
