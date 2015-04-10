using UnityEngine;
using System.Collections;

public class BlockRenderer : MonoBehaviour {

	public GameObject m_blockPrefab;
	GameObject[,] m_spriteObjects;
	public float m_blockDistance;

	int MAP_WIDTH, MAP_HEIGHT;

	GameObject[,] m_nextControlBlockObject;
	public float NEXT_BLOCK_POS_LEFT;
	public float NEXT_BLOCK_POS_TOP;
	public float NEXT_BLOCK_POS_Z;

	public float COLOR_ALPHA_FALL_ESTIMATE;

	public int NEXT_BLOCK_SIZE_MAX;

	public GUIStyle m_drawStyle;
	public Rect m_drawRect;
	public string m_drawString;

	private bool m_drawNext;

	private float m_vanishEffectTime_s;
	public float EFFECT_VANISH_TIME_MAX_SEC;

	//	次に操作対象になるブロックを表示するためのGameObjectを初期化する.
	private void initializeNextControlObject( int blockScale )
	{

		m_nextControlBlockObject = new GameObject[blockScale, blockScale];

		Vector3 position = new Vector3 ( NEXT_BLOCK_POS_LEFT, NEXT_BLOCK_POS_TOP, NEXT_BLOCK_POS_Z );

		for ( int j = 0;j < blockScale; ++j )
		{
			for ( int i = 0;i < blockScale; ++i )
			{

				position.x = NEXT_BLOCK_POS_LEFT + m_blockDistance * i;
				position.y = NEXT_BLOCK_POS_TOP - m_blockDistance * j;

				m_nextControlBlockObject[j, i] = 
					Instantiate (m_blockPrefab, position, Quaternion.identity ) as GameObject;

				m_nextControlBlockObject[j, i].SetActive ( false );
			}
		}

		m_drawNext = false;
	}
	
	public void Initialize( int MAP_WIDTH, int MAP_HEIGHT )
	{

		this.MAP_WIDTH = MAP_WIDTH;
		this.MAP_HEIGHT = MAP_HEIGHT;

		m_spriteObjects = new GameObject[MAP_HEIGHT, MAP_WIDTH];
		
		Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);
		
		for ( int j = 0;j < MAP_HEIGHT; ++j )
		{
			for ( int i = 0;i < MAP_WIDTH; ++i )
			{
				pos.x = ( i - MAP_WIDTH / 2 ) * m_blockDistance;
				pos.y = ( MAP_HEIGHT / 2 - j ) * m_blockDistance;
				m_spriteObjects[j, i] = Instantiate (m_blockPrefab, pos, Quaternion.identity) as GameObject;
			}
		}

		initializeNextControlObject ( NEXT_BLOCK_SIZE_MAX );

		m_vanishEffectTime_s = -0.1f;
	}

	//	ブロックのタイプから色を選択する.
	private Color selectColor( FieldBlockType blockType )
	{
		switch ( blockType )
		{

		case FieldBlockType.VANISH:

			if( m_vanishEffectTime_s < 0.0f )
			{
				return new Color( 1.0f, 1.0f, 1.0f, 0.0f );
			}
		//	return new Color( 1.0f, 1.0f, 1.0f, m_vanishEffectTime_s / EFFECT_VANISH_TIME_MAX_SEC );
			return Color.white * m_vanishEffectTime_s / EFFECT_VANISH_TIME_MAX_SEC;

		case FieldBlockType.WALL:
			return Color.gray;
			
		case FieldBlockType.LINE:
			return Color.red;
			
		case FieldBlockType.BUMP:
			return new Color( 0.3f, 0.5f, 0.8f );
			
		case FieldBlockType.L_REVERSE:
			return Color.blue;
			
		case FieldBlockType.L:
			return new Color( 0.8f, 0.5f, 0.3f );
			
		case FieldBlockType.S_REVERSE:
			return Color.green;
			
		case FieldBlockType.S:
			return new Color( 0.8f, 0.3f, 0.5f );
			
		case FieldBlockType.SQUARE:
			return Color.yellow;
			
		default:
			return Color.white;
		}
	}

	public void SetRenderBlock( FieldBlockType[,] field )
	{

		//	配置済みのブロックを設定する.
		for ( int i = 0;i < MAP_WIDTH; ++i )
		{
			for ( int j = 0;j < MAP_HEIGHT; ++j )
			{
				//	ブロックが存在しない場合.
				if( field[ j, i ] == FieldBlockType.NONE )
				{
					m_spriteObjects[ j, i ].SetActive ( false );
				}
				//	ブロックが存在する場合.
				else
				{
					m_spriteObjects[ j, i ].SetActive ( true );
					
					m_spriteObjects[ j, i ].renderer.material.color = selectColor( field[ j, i ] );
					
				//	Vector3 pos = m_spriteObjects[ j, i ].transform.position;
				//	pos.z = (float)( (int)field[ j, i ] - (int)FieldBlockType.WALL );
				//	m_spriteObjects[ j, i ].transform.position = pos;
				}
			}
		}
	}

	private void SetRenderBlock( int left, int top, Block block, Color color )
	{
		for ( int i = 0;i < block.m_blockMapSize; ++i )
		{
			for ( int j = 0;j < block.m_blockMapSize; ++j )
			{
				//	ブロックが存在する場合.
				if( block.m_blockMap[ j, i ] )
				{
					GameObject controlObject = m_spriteObjects[ top + j, left + i ];
					
					controlObject.SetActive ( true );
					
					controlObject.renderer.material.color = color;
				}
			}
		}
	}

	public void SetRenderBlock( int blockLeft, int blockTop, Block block, FieldBlockType blockType )
	{
		SetRenderBlock( blockLeft, blockTop, block, selectColor ( blockType ) );
	}
	
	public void SetRenderBlockFallPosition( int blockLeft, int blockTop, Block block, FieldBlockType blockType )
	{
		Color cr = selectColor ( blockType );
		cr.a = COLOR_ALPHA_FALL_ESTIMATE;

		SetRenderBlock( blockLeft, blockTop, block, cr );
	}

	public void SetRenderNextBlock( Block block, FieldBlockType blockType )
	{

		ResetRenderNextBlock ();

		Color cr = selectColor ( blockType );

		for ( int i = 0;i < block.m_blockMapSize; ++i )
		{
			for ( int j = 0;j < block.m_blockMapSize; ++j )
			{

				GameObject obj = m_nextControlBlockObject[j, i];

				if( block.m_blockMap[j, i] )
				{
					obj.SetActive ( true );

					obj.renderer.material.color = cr;
				}
			}
		}

		m_drawNext = true;
	}

	public void ResetRenderNextBlock()
	{
		for( int i = 0;i < NEXT_BLOCK_SIZE_MAX; ++i )
		{
			for( int j = 0;j < NEXT_BLOCK_SIZE_MAX; ++j )
			{
				m_nextControlBlockObject[i, j].SetActive( false );
			}
		}

		m_drawNext = false;
	}

	void Update()
	{
		if ( m_vanishEffectTime_s >= 0.0f )
		{
			m_vanishEffectTime_s -= Time.deltaTime;
		}
	}

	public void EnableVanishEffect()
	{
		m_vanishEffectTime_s = EFFECT_VANISH_TIME_MAX_SEC;
	}

	public bool FinishVanishEffect()
	{
		if ( m_vanishEffectTime_s <= 0.0f )
		{
			return true;
		}

		return false;
	}

	public void SetAllActive ( bool enable )
	{
		for ( int i = 0;i < MAP_HEIGHT; ++i )
		{
			for ( int j = 0;j < MAP_WIDTH; ++j )
			{
				m_spriteObjects[i, j].SetActive ( enable );
			}
		}

		for ( int i = 0;i < NEXT_BLOCK_SIZE_MAX; ++i )
		{
			for ( int j = 0;j < NEXT_BLOCK_SIZE_MAX; ++j )
			{
				m_nextControlBlockObject[i, j].SetActive ( enable );
			}
		}

		m_drawNext = enable;
	}

	void OnGUI()
	{

		if ( !m_drawNext )
		{
			return;
		}

		GUI.Label ( m_drawRect, m_drawString, m_drawStyle );
	}
}
