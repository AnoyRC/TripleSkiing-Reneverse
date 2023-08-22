using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

    public GameManager _gameManager;
	public GameObject[] map;
	public GameObject A_Zone;
	public GameObject B_Zone;
	
	public float Speed = 1f;
	
	void Update () {
	
			MOVE();
	}
	
	public void MAKE(){
		
		B_Zone=A_Zone;
		int a = Random.Range(0,map.Length);
        A_Zone = Instantiate(map[a], new Vector3(0,-10,0), transform.rotation) as GameObject;
		
	}
	
	public void MOVE(){
        if (_gameManager.gameState == GameState.play)
        {
            A_Zone.transform.Translate(Vector3.up * Speed * Time.deltaTime, Space.World);
            B_Zone.transform.Translate(Vector3.up * Speed * Time.deltaTime, Space.World);
        }
			
		if(A_Zone.transform.position.y>=0){
				DEATH();
		}
	}
	
	public void DEATH(){
		Destroy(B_Zone);
		MAKE();	
	}
}
