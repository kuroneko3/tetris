using UnityEngine;
using System.Collections;

public class mbScene : MonoBehaviour {

	virtual protected void start ()
	{
	}

	virtual protected void update ()
	{
	}

	// Use this for initialization
	void Start ()
	{
		wakeUpScene ();

		start ();
	}

	// Update is called once per frame
	void Update ()
	{
		if ( !m_finishWakeUpCoroutine )
		{
			return;
		}

		update ();
	}

	protected void changeScene ( eScene nextScene )
	{
		setFadeSprireRenderer ( false );

		m_nextScene = nextScene;
		StartCoroutine ( changeSceneCoroutine() );
	}

	protected void wakeUpScene ()
	{
		setFadeSprireRenderer ( true );
		StartCoroutine ( wakeUpSceneCoroutine () );
	}

	void setFadeSprireRenderer ( bool enable )
	{
		GUIText[] texts = transform.GetComponentsInChildren<GUIText> ();
		
		foreach ( GUIText text in texts )
		{
			text.gameObject.AddComponent<mbFadeText>().m_Reverse = enable;
		}
	}

	bool m_finishWakeUpCoroutine = false;
	IEnumerator wakeUpSceneCoroutine ()
	{
		GameObject obj = mbBlackOutScene.Create ( true );

		if ( obj == null )
		{
			m_finishWakeUpCoroutine = true;
			yield break;
		}

		mbBlackOutScene script = obj.GetComponent<mbBlackOutScene> ();
		
		while ( !script.m_FinishBlackOut )
		{
			yield return 0;
		}

		m_finishWakeUpCoroutine = true;
	}

	private eScene m_nextScene = eScene.NONE;

	IEnumerator changeSceneCoroutine ()
	{

		GameObject obj = mbBlackOutScene.Create ( false );

		if ( obj == null )
		{
			yield break;
		}

		mbBlackOutScene script = obj.GetComponent<mbBlackOutScene> ();

		while ( !script.m_FinishBlackOut )
		{
			yield return 0;
		}

		cSceneChanger.ChangeScene ( m_nextScene );
	}
}
