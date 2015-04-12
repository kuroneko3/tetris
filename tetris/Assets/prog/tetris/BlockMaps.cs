using UnityEngine;
using System.Collections;

public class BlockMaps{

	private Block[] m_spinBlockMaps;
	private int m_spinBlockTypeMax;

	public BlockMaps( int spinTypeMax, int blockSize, string blockMapData )
	{
		m_spinBlockTypeMax = spinTypeMax;

		m_spinBlockMaps = new Block[spinTypeMax];

		for( int i = 0;; ++i )
		{

			m_spinBlockMaps[i] = new Block( blockSize, blockMapData );

			if( i + 1 == spinTypeMax )
			{
				break;
			}

			blockMapData = blockMapData.Substring( blockSize * blockSize );
		}
	}

	public Block GetSpinBlockMap( int blockSpinID )
	{

		if( blockSpinID >= m_spinBlockTypeMax )
		{
			blockSpinID = blockSpinID % m_spinBlockTypeMax;
		}

		if( blockSpinID < 0 )
		{
			blockSpinID += m_spinBlockTypeMax;
		}

		return m_spinBlockMaps[ blockSpinID ];
	}

	public int GetSpinTypeMax()
	{
		return m_spinBlockTypeMax;
	}
}
