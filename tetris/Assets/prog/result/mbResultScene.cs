using UnityEngine;
using System.Collections;

class mbResultScene : mbScene
{

	[SerializeField]
	GUIText m_LevelText;

	[SerializeField]
	GUIText m_ScoreText;

	override protected void start ()
	{
		StartCoroutine ( updateCoroutine () );
	}
	
	override protected void update ()
	{
		//	コルーチン終了時にタイトル画面に遷移.
		if ( m_finishResultCoroutine )
		{
			changeScene ( eScene.TITLE );
		}
	}


	bool m_finishResultCoroutine = false;
	IEnumerator updateCoroutine ()
	{

		float timer = 1.0f;

		//	暗転解除待ち.
		while (timer > 0.0f )
		{
			timer -= Time.deltaTime;
			yield return 0;
		}

		Color cr = new Color ( 1.0f, 1.0f, 1.0f, 1.0f );

		//	リザルトを徐々に表示.
		float TEXT_FADE_TIME = 1.0f;

		//	レベル.
		timer = TEXT_FADE_TIME;
		m_LevelText.text = cScore.GetLevel().ToString();

		while ( timer > 0.0f )
		{
			timer -= Time.deltaTime;

			cr.a = ( TEXT_FADE_TIME - timer ) / TEXT_FADE_TIME;
			m_LevelText.color = cr;

			if ( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Return ) )
			{
				timer = -1.0f;
			}

			yield return 0;
		}

		m_LevelText.color = new Color ( 1.0f, 1.0f, 1.0f, 1.0f );

		yield return 0;
		
		timer = TEXT_FADE_TIME;
		m_ScoreText.text = cScore.GetScore().ToString();

		while ( timer > 0.0f )
		{
			timer -= Time.deltaTime;
			
			cr.a = ( TEXT_FADE_TIME - timer ) / TEXT_FADE_TIME;
			m_ScoreText.color = cr;
			
			if ( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Return ) )
			{
				timer = -1.0f;
			}
			
			yield return 0;
		}
		
		m_ScoreText.color = new Color ( 1.0f, 1.0f, 1.0f, 1.0f );

		//	リザルトを完全表示.
		yield return 0;

		//	Space, Enterキーが押されるまで待機.
		while ( ! ( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Return ) ) )
		{
			yield return 0;
		}

		m_finishResultCoroutine = true;
	}
};
