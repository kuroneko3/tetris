using UnityEngine;
using System.Collections;

class mbBlinkText : MonoBehaviour
{

	public float m_ColorChangeTime = 3.0f;
	public Color m_Color1 = new Color ( 1.0f, 1.0f, 1.0f, 1.0f );
	public Color m_Color2 = new Color ( 1.0f, 1.0f, 0.3f, 1.0f );

	GUIText m_text;

	float m_timer = 0.0f;

	void Start ()
	{
		m_text = this.gameObject.GetComponent<GUIText>();
	}

	void Update ()
	{
		m_timer += Time.deltaTime;

		float t = Mathf.Abs ( Mathf.Sin( m_timer / m_ColorChangeTime * Mathf.PI ) );

		m_text.color = Color.Lerp ( m_Color1, m_Color2, 1.0f - t );
	}
};