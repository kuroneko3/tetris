using UnityEngine;
using System.Collections;

public class GameOverScene : MonoBehaviour {

	private bool m_enableScene;
	
	public GameObject m_gameOverText;

	// Use this for initialization
	void Start () {
	
		m_enableScene = false;

		m_gameOverText.SetActive ( m_enableScene );
	}

	public bool ChangeGameState( GameState before )
	{
		return true;
	}
	
	public GameState UpdateScene()
	{
		m_enableScene = true;
		
		if ( Input.GetKeyDown ( KeyCode.Space ) )
		{
			
			m_enableScene = false;
			
			m_gameOverText.SetActive ( m_enableScene );
			
			return GameState.WAIT_START_KEY_INPUT;
		}
		
		m_gameOverText.SetActive ( m_enableScene );
		
		return GameState.GAME_OVER;
	}
}
