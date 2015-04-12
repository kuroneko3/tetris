using UnityEngine;
using System.Collections;

class mbResultScene : mbScene
{

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
			yield return 0;
		}

		//	リザルトを徐々に表示.
		timer = 2.0f;
		while ( timer > 0.0f )
		{
			timer -= Time.deltaTime;

			if ( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Return ) )
			{
				timer = -1.0f;
			}

		}

		//	リザルトを完全表示.
		yield return 0;

		//	Space, Enterキーが押されるまで待機.
		while ( ! ( Input.GetKeyDown ( KeyCode.Space ) & Input.GetKeyDown ( KeyCode.Return ) ) )
		{
			yield return 0;
		}

		m_finishResultCoroutine = true;
	}
};
