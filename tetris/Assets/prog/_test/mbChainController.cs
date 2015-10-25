using UnityEngine;
using System.Collections;

class mbChainController : MonoBehaviour {

	[SerializeField]
	GameObject m_chainPrefab;
	[SerializeField]
	GameObject m_rootObj;
	[SerializeField]
	GameObject m_tipObj;

	[SerializeField]
	int m_chainDiv = 17;

	[SerializeField]
	float m_maxLength;

	[SerializeField]
	float m_objDist;

	GameObject[] m_chainObject = null;
	[SerializeField]
	Vector3[] m_chainPos = null;

	Vector3[] m_chainSpd = null;

	Vector3 m_rootSpd;
	Vector3 m_tipSpd;

	void Awake () {

		if ( m_chainPrefab == null ) return;

		int chainDiv = m_chainDiv;

		m_chainObject = new GameObject[chainDiv-1];
		for ( int i = 0;i < m_chainObject.Length; ++i ) {
			m_chainObject[i] = Object.Instantiate ( m_chainPrefab ) as GameObject;
			m_chainObject[i].transform.parent = this.transform;
		}

		m_chainPos = new Vector3[chainDiv];
		m_chainSpd = new Vector3[chainDiv];

		m_chainPos[0] = m_rootObj.transform.position;
		m_chainPos[m_chainPos.Length-1] = m_tipObj.transform.position;
		for ( int i = 1;i < m_chainPos.Length-1; ++i ) {
			float rate = (float)i / ( m_chainPos.Length - 1 );
			m_chainPos[i] = Vector3.Lerp ( m_chainPos[0], m_chainPos[m_chainPos.Length-1], rate );
		}
	}

	void LateUpdate () {

		if ( m_chainObject == null || m_rootObj == null || m_tipObj == null ) return;

		//	両端の位置を最新の位置に補正.
		m_rootSpd = ( m_rootObj.transform.position - m_chainPos[0] ) / Time.deltaTime;
		m_tipSpd = ( m_tipObj.transform.position - m_chainPos[m_chainPos.Length-1] ) / Time.deltaTime;
		m_chainPos[0] = m_rootObj.transform.position;
		m_chainPos[m_chainPos.Length-1] = m_tipObj.transform.position;


		//	全体の大体の長さを計算.
	//	m_maxLength = 0.235f * m_chainObject.Length;
		m_objDist = ( m_rootObj.transform.position - m_tipObj.transform.position ).magnitude;
		float chainLength = Mathf.Max ( m_maxLength, m_objDist );


		//	計算用にコピー.
		var tmp = new Vector3[m_chainPos.Length];
		for ( int i = 0;i < tmp.Length; ++i ) {
			tmp[i] = m_chainPos[i];
		}

		//	位置を計算.
		calculatePysicalPos ( ref tmp );
	//	calculateChainPos ( ref tmp, 0, m_chainPos.Length / 2, m_chainPos.Length-1, chainLength );

		//	元に戻す.
		for ( int i = 0;i < tmp.Length; ++i ) {
			m_chainPos[i] = tmp[i];
		}

		//	位置を元にオブジェクトを移動.
		setChainObjPos ();

	}

	void calculateChainPos ( ref Vector3[] pos, int left, int center, int right, float length ) {

		if ( left == center || right == center || left == right ) return;

		pos[center] = ( pos[left] + pos[right] ) * 0.5f;

		float dist = ( pos[left] - pos[right] ).magnitude;

		//	地面に接する際に必要な距離を計算する.
		float d = ( length * length ) * 0.25f;
		float groundLength = Mathf.Sqrt ( d + pos[left].y * pos[left].y ) + Mathf.Sqrt ( d + pos[right].y * pos[right].y );

		float maxHeight = ( pos[left].y + pos[right].y ) * 0.5f;

		float a = ( -maxHeight / ( groundLength - dist ) );
		float b = -a * groundLength;

		if ( length <= dist ) 				pos[center].y = maxHeight;
		else if ( length < groundLength )	pos[center].y = b + a * length;
		else 								pos[center].y = 0.0f;

		calculateChainPos ( ref pos, left, ( left + center ) /2, center, length * 0.5f );
		calculateChainPos ( ref pos, center, ( center + right ) /2, right, length * 0.5f );
	}

	Vector3 calculateAcceleration ( int number, int compareDir ) {

		Vector3 dir1 = ( m_chainPos[number-compareDir] - m_chainPos[number] ).normalized;

		return m_chainSpd[number-compareDir] * Vector3.Dot ( m_chainSpd[number-compareDir].normalized, dir1 );
	}

	void calculatePysicalPos ( ref Vector3[] pos ) {

		Vector3 g = new Vector3 ( 0.0f, -1.0f, 0.0f );

		//	重力と張力を元に位置を計算.
		m_chainSpd[0] = m_rootSpd;
		m_chainSpd[m_chainSpd.Length-1] = m_tipSpd;

		for ( int i = 1;i < m_chainSpd.Length-1; ++i ) {
			m_chainSpd[i].x = 0.0f;
			m_chainSpd[i].z = 0.0f;
		}

		int center = pos.Length / 2;
		for ( int i = 1;i < m_chainSpd.Length-1; ++i ) {
			//	Root側.
			int number = i;
			Vector3 a = calculateAcceleration ( number, -1 );
			a.y = 0.0f;
			m_chainSpd[i] += a * Time.deltaTime;

			number = m_chainSpd.Length-i-1;
			a = calculateAcceleration ( number, 1 );
			a.y = 0.0f;
			m_chainSpd[i] += a * Time.deltaTime;
		}

		for ( int i = 1;i < m_chainSpd.Length-1; ++i ) {
			m_chainSpd[i] += g * Time.deltaTime;
			if ( m_chainSpd[i].magnitude > 1.0f ) pos[i] += m_chainSpd[i] * Time.deltaTime;
			if ( pos[i].y < 0.0f ) {
				pos[i].y = 0.0f;
				m_chainSpd[i] = Vector3.zero;
			}
		}

		//	長さを元に位置を補正する.
		float allDist = 0.0f;
		for ( int i = 0;i < pos.Length-1; ++i ) {
			allDist += ( pos[i+1] - pos[i] ).magnitude;
		}

		allDist = Mathf.Min ( 0.5f * ( pos.Length - 1 ), allDist );
		float dist = allDist / ( pos.Length - 1 );
		//	両端から補正する.
		for ( int i = 0;i < ( pos.Length ) / 2; ++i ) {

			pos[i+1] = pos[i] + ( pos[i+1] - pos[i] ).normalized * dist;
			pos[pos.Length-i-2] = pos[pos.Length-i-1] + ( pos[pos.Length-i-2] - pos[pos.Length-i-1] ).normalized * dist;
		}

		pos[center] = ( pos[center+1] + pos[center-1] ) * 0.5f;

		//	中央から補正する.
		center = pos.Length/2;
		for ( int i = 0;i < center-1; ++i ) {

			pos[center-i-1] = pos[center-i] + ( pos[center-i-1] - pos[center-i] ).normalized * dist;
			pos[center+i+1] = pos[center+i] + ( pos[center+i+1] - pos[center+i] ).normalized * dist;

		}

	}

	void setChainObjPos () {

		for ( int i = 0;i < m_chainObject.Length; ++i ) {

			Vector3 dist = ( m_chainPos[i+1] - m_chainPos[i] );

			var scale = m_chainObject[i].transform.localScale;
			scale.x = dist.magnitude;
			m_chainObject[i].transform.localScale = scale;

			m_chainObject[i].transform.rotation = 
				Quaternion.FromToRotation ( new Vector3 ( 1.0f, 0.0f, 0.0f ) , dist.normalized );

			m_chainObject[i].transform.position = ( m_chainPos[i] + m_chainPos[i+1] ) * 0.5f;
		}

	}

};