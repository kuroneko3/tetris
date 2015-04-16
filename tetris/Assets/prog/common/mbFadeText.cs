using UnityEngine;
using System.Collections;

class mbFadeText : MonoBehaviour
{

	GUIText m_text;

	public bool m_Reverse = false;

	void Awake ()
	{
		m_text = this.gameObject.guiText;
		
		StartCoroutine ( fadeText() );
	}

	IEnumerator fadeText ()
	{

		if ( m_text == null )
		{
			yield break;
		}

		float TIME_MAX = 0.5f;
		float timer = TIME_MAX;

		while ( timer >= 0.0f )
		{
			timer -= Time.deltaTime;

			Color cr = m_text.color;
			cr.a = timer / TIME_MAX;
			cr.a = ( m_Reverse )? 1.0f - cr.a : cr.a;

			m_text.color = cr;

			yield return 0;
		}
	}
};