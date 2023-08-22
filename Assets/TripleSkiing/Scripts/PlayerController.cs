using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

    public GameManager _gameManager;
    public Sprite spriteDie;
    public float speedMove;
    private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float offset = Time.deltaTime * speedMove;
        if (_gameManager.gameState == GameState.play)
        {

#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8
            foreach (Touch touch in Input.touches)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary && touch.position.x > Screen.width / 2)
                        transform.position = new Vector2(transform.position.x + offset, transform.position.y);
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary && touch.position.x < Screen.width / 2)
                        transform.position = new Vector2(transform.position.x - offset, transform.position.y);
                }
            }
#endif

            if (Input.GetKey(KeyCode.D))
                transform.position = new Vector2(transform.position.x + offset, transform.position.y);
            if (Input.GetKey(KeyCode.A))
                transform.position = new Vector2(transform.position.x - offset, transform.position.y);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_gameManager.gameState == GameState.play)
        {
            if (col.tag.Equals("enemy"))
            {
                die();
            }

            if (col.tag.Equals("bonus"))
            {
                Destroy(col.gameObject);
                takeBonus();
            }
        }
    }

    void takeBonus()
    {
        SoundManager.instance.playSoundBonus();
        _gameManager.bonus();
    }

    void die()
    {
        SoundManager.instance.playSoundDie();
        onEffect(false);
        renderer.sprite = spriteDie;
        _gameManager.gameOver();
    }

    public void onEffect(bool check)
    {
        foreach (Transform tr in transform)
        {
            tr.gameObject.SetActive(check);
        }
    }
}
