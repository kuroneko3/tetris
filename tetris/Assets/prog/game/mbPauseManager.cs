using UnityEngine;
using System.Collections;

class mbPauseManager : MonoBehaviour
{

	[SerializeField]
	private GUIText m_returnGame;

	[SerializeField]
	private GUIText m_returnTitle;

	public bool m_SelectReturnGame = true;

	void Start ()
	{
	}

	void Update ()
	{

		if ( !gameObject.activeSelf )
		{
			return;
		}

		//	入力に応じて切り替え.
		if ( Input.GetKeyDown ( KeyCode.UpArrow ) | Input.GetKeyDown ( KeyCode.DownArrow ) )
		{
			m_SelectReturnGame ^= true;
		}

		//	文字色の設定.
		m_returnGame.color = getColor ( m_SelectReturnGame );
		m_returnTitle.color = getColor ( !m_SelectReturnGame );
	}

	float m_colorTimer = 0.0f;
	float COLOR_TIME_MAX = 1.0f;
	Color getColor ( bool enable )
	{
		if ( enable )
		{
			m_colorTimer += Time.deltaTime;

			if ( m_colorTimer > COLOR_TIME_MAX )
			{
				m_colorTimer -= COLOR_TIME_MAX;
			}

			return new Color ( 0.8f, 0.8f, 0.3f, 0.5f + 0.5f * Mathf.Sin ( m_colorTimer / COLOR_TIME_MAX * Mathf.PI * 2 ) );
		}

		return Color.gray;
	}
};
