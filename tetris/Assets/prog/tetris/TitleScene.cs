using UnityEngine;
using System.Collections;

public class TitleScene : MonoBehaviour {

	private bool m_enableScene;

	public GameObject m_titleText;

	// Use this for initialization
	void Start () {

		m_enableScene = false;
	
		m_titleText.SetActive ( m_enableScene );
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

			m_titleText.SetActive ( m_enableScene );

			return GameState.PLAY_GAME;
		}

		m_titleText.SetActive ( m_enableScene );

		return GameState.WAIT_START_KEY_INPUT;
	}
}
