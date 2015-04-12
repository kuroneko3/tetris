using UnityEngine;
using System.Collections;

class mbGameScene : mbScene
{
	[SerializeField]
	private GameObject m_PauseObject;

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
		}

		if ( checkGameOver() )
		{
			StartCoroutine ( updateGameOverCoroutine () );
		}

		updateGame ();
	}

	void updateGame ()
	{



	}

	bool checkGameOver ()
	{
		return false;
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

