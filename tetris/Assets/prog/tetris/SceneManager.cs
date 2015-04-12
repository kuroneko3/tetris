using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
	/*
	[SerializeField]
	private TitleScene m_TitleScene;
	[SerializeField]
	private GameScene m_GameScene;
	[SerializeField]
	private WaitPlayScene m_WaitPlayScene;
	[SerializeField]
	private GameOverScene m_GameOverScene;
	*/

	public GameObject m_gameScene;
	public GameObject m_waitPlayScene;
	public GameObject m_titleScene;
	public GameObject m_gameOverScene;

	public GameObject m_scoreRenderer;

	GameState m_gameState;

	// Use this for initialization
	void Start () {
		m_gameState = GameState.WAIT_START_KEY_INPUT;
	}

	private GameState updateScene()
	{
		switch ( m_gameState )
		{
			case GameState.WAIT_START_KEY_INPUT:
				return m_titleScene.GetComponent<TitleScene>().UpdateScene();

			case GameState.PLAY_GAME:
				return m_gameScene.GetComponent<GameScene>().UpdateScene();

			case GameState.WAIT_PLAY:
				return m_waitPlayScene.GetComponent<WaitPlayScene>().UpdateScene();

			case GameState.GAME_OVER:
				return m_gameOverScene.GetComponent<GameOverScene>().UpdateScene();

			default:
				return GameState.WAIT_START_KEY_INPUT;
		}
	}

	private bool changeGameState( GameState before )
	{

		if ( before == m_gameState )
		{
			return true;
		}

		switch ( m_gameState )
		{
		case GameState.WAIT_START_KEY_INPUT:
			return m_titleScene.GetComponent<TitleScene>().ChangeGameState( before );
			
		case GameState.PLAY_GAME:
			return m_gameScene.GetComponent<GameScene>().ChangeGameState( before );
			
		case GameState.WAIT_PLAY:
			return m_waitPlayScene.GetComponent<WaitPlayScene>().ChangeGameState( before );
			
		case GameState.GAME_OVER:
			return m_gameOverScene.GetComponent<GameOverScene>().ChangeGameState( before );
			
		default:
			return false;
		}
	}

	private bool selectScoreActive()
	{
		if ( m_gameState == GameState.WAIT_START_KEY_INPUT )
		{
			return false;
		}

		return true;
	}
	
	// Update is called once per frame
	void Update () {

		GameState beforeGameState = m_gameState;

		m_gameState = updateScene ();

		changeGameState( beforeGameState );

		m_scoreRenderer.SetActive ( selectScoreActive() );
	}
}
