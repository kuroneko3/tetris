using UnityEngine;
using System.Collections;

public class ControlBlockManager{

	private BlockMaps[] m_blocks;

	//	操作中のブロック番号.
	private FieldBlockType m_blockID;
	private int m_blockSpinID;

	private FieldBlockType m_nextBlockID;

	private FieldBlockType[] m_notUsedBlockID;
	private int m_notUsedBlockMax;

	public ControlBlockManager()
	{
		m_blocks = new BlockMaps[(int)FieldBlockType.ENUM_FIELD_BLOCK_TYPE_END];

		m_blocks [ (int)FieldBlockType.LINE ] = 
			new BlockMaps (2, 4, "00001111000000000010001000100010");

		m_blocks [ (int)FieldBlockType.BUMP ] = 
			new BlockMaps (4, 3, "000111010010011010000010111010110010");

		m_blocks [ (int)FieldBlockType.L_REVERSE ] = 
			new BlockMaps (4, 3, "000111001011010010000100111010010110");

		m_blocks [ (int)FieldBlockType.L ] = 
			new BlockMaps (4, 3, "000111100010010011000001111110010010");

		m_blocks [ (int)FieldBlockType.S_REVERSE ] = 
			new BlockMaps (2, 3, "000110011001011010");

		m_blocks [ (int)FieldBlockType.S ] = 
			new BlockMaps (2, 3, "000011110100110010");

		m_blocks [ (int)FieldBlockType.SQUARE ] = 
			new BlockMaps (1, 2, "1111");

		m_notUsedBlockMax = 0;
		m_notUsedBlockID = new FieldBlockType[(int)FieldBlockType.ENUM_FIELD_BLOCK_TYPE_END];

		ResetForGameStart ();
	}

	//	現在操作中のブロックを取得する.
	public Block GetControlBlock()
	{
		return m_blocks[ (int)m_blockID ].GetSpinBlockMap( m_blockSpinID );
	}

	//	現在捜査中のブロックが回転した状態を取得する
	public Block GetControlBlockSpin()
	{
		return m_blocks[ (int)m_blockID ].GetSpinBlockMap( m_blockSpinID + 1 );
	}

	public Block GetControlBlockSpinReverse()
	{
		return m_blocks[ (int)m_blockID ].GetSpinBlockMap( m_blockSpinID - 1 );
	}

	//	ブロックを回転させる.
	public void SpinBlock()
	{
		++m_blockSpinID;

		if( m_blockSpinID >= m_blocks[ (int)m_blockID ].GetSpinTypeMax() )
		{
			m_blockSpinID -= m_blocks[ (int)m_blockID ].GetSpinTypeMax();
		}
	}

	public void SpinBLockReverse()
	{
		--m_blockSpinID;

		if( m_blockSpinID < 0 )
		{
			m_blockSpinID = m_blocks[ (int)m_blockID ].GetSpinTypeMax() - 1;
		}
	}

	public FieldBlockType GetBlockType()
	{
		return m_blockID;
	}

	public Block GetNextBlock()
	{
		return m_blocks[ (int)m_nextBlockID ].GetSpinBlockMap( 0 );
	}

	public FieldBlockType GetNextBlockType()
	{
		return m_nextBlockID;
	}

	private void resetNotUseBlockType()
	{
		for( int i = 0;i < (int)FieldBlockType.ENUM_FIELD_BLOCK_TYPE_END; ++i )
		{
			m_notUsedBlockID[ i ] = (FieldBlockType) i;
		}

		m_notUsedBlockMax = (int)FieldBlockType.ENUM_FIELD_BLOCK_TYPE_END;
	}

	private FieldBlockType SelectNextBlockType()
	{

		if ( m_notUsedBlockMax == 0 )
		{
			resetNotUseBlockType();
		}

		int randValue = Random.Range( 0, m_notUsedBlockMax );

		FieldBlockType retValue = m_notUsedBlockID [randValue];

		--m_notUsedBlockMax;

		for ( int i = randValue;i < m_notUsedBlockMax; ++i )
		{
			m_notUsedBlockID[i] = m_notUsedBlockID[i + 1];
		}

		return retValue;

	//	return (FieldBlockType)Random.Range( 0, (int)FieldBlockType.ENUM_FIELD_BLOCK_TYPE_END );
	}

	public void ResetControlBlockParameter()
	{
		m_blockID = m_nextBlockID;
		m_nextBlockID = SelectNextBlockType ();
		m_blockSpinID = 0;
	}

	public void ResetForGameStart()
	{

		resetNotUseBlockType ();

		ResetControlBlockParameter ();
		m_blockID = SelectNextBlockType ();
	}
}
