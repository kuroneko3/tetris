using UnityEngine;
using System.Collections;

public class GameScene : MonoBehaviour {

	public GameObject m_fieldManager;

	private bool m_enableScene;

	public GUIStyle m_explanationStyle;
	public Rect m_explanationRect;

	// Use this for initialization
	void Start () {
	}

	public bool ChangeGameState( GameState before )
	{

		if ( before == GameState.WAIT_PLAY )
		{

			m_fieldManager.GetComponent<FieldManager>().RestartRenderField();

			return true;
		}

		m_fieldManager.GetComponent<FieldManager>().ResetToStartGame();

		return true;
	}

	public GameState UpdateScene()
	{

		m_enableScene = true;
		
		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{		
			m_fieldManager.GetComponent<FieldManager>().StopRenderField();

			m_enableScene = false;
			
			return GameState.WAIT_PLAY;
		}

		m_fieldManager.GetComponent<FieldManager> ().UpdateGame ();

		GameState retGameState = m_fieldManager.GetComponent<FieldManager>().GetGameState();

		m_enableScene = ( retGameState == GameState.PLAY_GAME );

		return retGameState;
	}

	void OnGUI()
	{
		if ( !m_enableScene )
		{
			return;
		}

		string text = "LeftArrow/\n    RightArrow : Move Block\n" +
			"DownArrow : Fall Block\n" + 
			"UpArrow : Land Block\n" + 
			"Z / SPACE : Spin Block\n" + 
			"X : ReverseSpin Block\n" + 
			"Escape : Pause game";

		GUI.Label( m_explanationRect, text, m_explanationStyle );
	}
}
