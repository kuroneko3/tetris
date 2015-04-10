using UnityEngine;
using System.Collections;

public class WaitPlayScene : MonoBehaviour {

	bool m_enableScene;

	public GUIStyle m_textStyle;
	public Rect m_textRect;
	public string m_textString;

	// Use this for initialization
	void Start () {
		m_enableScene = false;
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
			
			return GameState.PLAY_GAME;
		}

		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{
			m_enableScene = false;

			return GameState.WAIT_START_KEY_INPUT;
		}
		
		return GameState.WAIT_PLAY;
	}

	void OnGUI()
	{

		if ( !m_enableScene )
		{
			return;
		}

		GUI.Label ( m_textRect, m_textString, m_textStyle );
	}
}
