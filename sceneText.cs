using UnityEngine;
using System.Collections;

public class sceneText : MonoBehaviour {

	//	タイトル描画用のパラメータ.
	public GUIStyle m_titleStyle;
	public Rect m_titleRect;
	public string m_titleString;

	public GUIStyle m_textStyle;
	public Rect m_textRect;
	public string m_textString;

	void OnGUI()
	{
		GUI.Label ( m_titleRect, m_titleString, m_titleStyle );

		GUI.Label ( m_textRect, m_textString, m_textStyle );
	}
}
