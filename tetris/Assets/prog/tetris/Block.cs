using UnityEngine;
using System.Collections;

public class Block{

	public bool[,] m_blockMap;
	public int m_blockMapSize;

	public Block( int blockSize, string blockMapData )
	{

		m_blockMap = new bool[blockSize, blockSize];
		m_blockMapSize = blockSize;

		for( int i = 0;i < blockSize; ++i )
		{
			for (int j = 0;j < blockSize; ++j )
			{
				if( blockMapData[i * blockSize + j] == '1' )
				{
					m_blockMap[i, j] = true;
				}
				else
				{
					m_blockMap[i, j] = false;
				}
			}
		}
	}
}
