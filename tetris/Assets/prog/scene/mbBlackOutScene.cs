using UnityEngine;
using System.Collections;

class mbBlackOutScene : MonoBehaviour
{

	public bool m_Reverse = false;
	public bool m_FinishBlackOut = false;

	private static SpriteRenderer m_sprite;

	[SerializeField]
	private static GameObject s_SpritePrefab = null;

	static public GameObject Create ( bool Reverse )
	{

		if ( s_SpritePrefab == null )
		{
			s_SpritePrefab = Resources.Load ( "prefab/block" ) as GameObject;

			if ( s_SpritePrefab == null )
			{
				return null;
			}
		}

		GameObject obj = GameObject.Instantiate ( s_SpritePrefab ) as GameObject;
		obj.transform.localScale *= 100.0f;

		obj.AddComponent<mbBlackOutScene>().m_Reverse = Reverse;

		return obj;
	}

	void Start ()
	{
		m_sprite = this.gameObject.GetComponent<SpriteRenderer>();

		StartCoroutine ( BlackOut () );
	}


	private float BLACK_OUT_TIME_MAX = 0.5f;
	IEnumerator BlackOut ()
	{
		float timer = BLACK_OUT_TIME_MAX;

		Color cr = new Color ( 0.0f, 0.0f, 0.0f, ( m_Reverse )? 1.0f : 0.0f );
		
		while ( timer > 0.0f )
		{
			timer -= Time.deltaTime;

			cr.a = ( ( m_Reverse )? timer : ( 1.0f - timer ) ) / BLACK_OUT_TIME_MAX;
			m_sprite.color = cr;
			
			yield return 0;
		}

		m_FinishBlackOut = true;
	}

};
