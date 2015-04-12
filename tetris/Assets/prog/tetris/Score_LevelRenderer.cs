using UnityEngine;
using System.Collections;

public class Score_LevelRenderer : MonoBehaviour {

	//	タイトル描画用のパラメータ.
	public GUIStyle m_drawStyle;
	public Rect m_drawRect;
	private string m_drawString;

	private long m_score;

	private int m_level;

	private int m_deletedRows;

	public int NEW_LINE_TO_PRINT_DELETE_ROW;
	int LEVEL_UP_ROW_COUNT;

	void Start()
	{
		Reset ();
	}

	public void AddScore( long score )
	{
		//	オーバーフローする場合.
		if(m_score + score < m_score)
		{
			return;
		}

		m_score += score;
	}

	public void Reset()
	{
		m_score = 0;
		m_level = 1;
		m_deletedRows = 0;
	}

	public long GetScore()
	{
		return m_score;
	}

	public void AddLevel()
	{
		++m_level;
	}

	public int GetLevel()
	{
		return m_level;
	}

	public void SetDeletedRows( int deletedRows )
	{
		m_deletedRows = deletedRows;
	}

	public void SetLevelUpRowCountMax(int rowMax)
	{
		LEVEL_UP_ROW_COUNT = rowMax;
	}

	void OnGUI()
	{

		string text = "Score : " + m_score.ToString () + "\n" + "Level : " + m_level.ToString();

		int i = 0;
		for ( ;i < m_deletedRows; ++i )
		{
			if( i % NEW_LINE_TO_PRINT_DELETE_ROW == 0 )
			{
				text += "\n";
			}
			text += "■";
		}

		for( ;i < LEVEL_UP_ROW_COUNT; ++i )
		{
			if( i % NEW_LINE_TO_PRINT_DELETE_ROW == 0 )
			{
				text += "\n";
			}
			text += "□";
		}


		GUI.Label ( m_drawRect, text, m_drawStyle );
	}
}
