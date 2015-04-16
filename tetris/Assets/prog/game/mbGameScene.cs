using UnityEngine;
using System.Collections;

class mbGameScene : mbScene
{
	[SerializeField]
	private GameObject m_PauseObject;

	[SerializeField]
	private GameObject m_BlockRendererObject;

	cTetrisManager m_tetrisManager = null;

	void Awake ()
	{
		cScore.Reset ();

		m_tetrisManager = new cTetrisManager ( m_BlockRendererObject );
	}

	override protected void start ()
	{
	}


	bool m_disableUpdate = false;
	override protected void update ()
	{

		if ( m_disableUpdate )
		{
			return;
		}
		
		if ( Input.GetKeyDown ( KeyCode.Escape ) )
		{
			StartCoroutine ( updatePauseCoroutine () );
			return;
		}

		if ( checkGameOver() )
		{
			StartCoroutine ( updateGameOverCoroutine () );
			return;
		}
	
		updateGame ();
	}

	void updateGame ()
	{
		m_tetrisManager.UpdateGame();
	}

	bool checkGameOver ()
	{
		return m_tetrisManager.CheckGameOver();
	}

	IEnumerator updateGameOverCoroutine ()
	{
		yield return 0;

		changeScene ( eScene.RESULT );
	}

	IEnumerator updatePauseCoroutine ()
	{

		eScene nextScene = eScene.NONE;

		m_disableUpdate = true;

		m_PauseObject.SetActive ( true );

		while ( true )
		{
			//	エスケープの場合はゲームに戻る.
			if ( Input.GetKeyDown ( KeyCode.Escape ) )
			{
				break;
			}

			//	決定の場合は指定された処理を行う.
			if ( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Return ) )
			{
				nextScene = ( m_PauseObject.GetComponent<mbPauseManager>().m_SelectReturnGame )? eScene.NONE : eScene.TITLE;
				break;
			}
		}

		//	ゲームに戻る場合.
		if ( nextScene == eScene.NONE )
		{
			m_PauseObject.SetActive ( false );
			m_disableUpdate = false;
			yield break;
		}

		changeScene ( nextScene );
	}
};

