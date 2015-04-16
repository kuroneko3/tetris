using UnityEngine;
using System.Collections;

class cTetrisManager {

	public bool CheckGameOver ()
	{
		return ( m_gameState == GameState.GAME_OVER ) ;
	}

	public cTetrisManager ( GameObject rendererObject )
	{
		m_blockManager = new ControlBlockManager ();
		
		m_field = new FieldBlockType[cTetrisParam.MAP_HEIGHT, cTetrisParam.MAP_WIDTH];

		m_blockRenderer = rendererObject.GetComponent<BlockRenderer> ();
		m_blockRenderer.Initialize( cTetrisParam.MAP_WIDTH, cTetrisParam.MAP_HEIGHT );

		m_blockMoveModeChangeTime_s = 0.0f;
		m_blockMoveDeltaTime_s = 0.0f;

		BLOCK_FALL_TIME_MAX_SEC = cTetrisParam.BLOCK_FALL_TIME_MAX_SEC_DEFAULT;
		
		m_blockFallDeltaTime = 0.0f;

		m_waitUpDownArrowKey = false;

		resetFieldState ();
		
		setEnableGameObject ();

		resetControlBlockParameter ();

		m_blockManager.ResetForGameStart ();

		m_blockRenderer.SetRenderNextBlock (
			m_blockManager.GetNextBlock(), m_blockManager.GetNextBlockType() );
	}


	private int m_controlBlockLeft, m_controlBlockTop;

	private ControlBlockManager m_blockManager;

	private FieldBlockType[,] m_field;
	
	private BlockRenderer m_blockRenderer = null;

	private int m_nowLevel = 0;

	//	ブロックの落下時間間隔[s].
	float m_blockFallDeltaTime;
	float BLOCK_FALL_TIME_MAX_SEC;
	
	//	ブロックの左右高速移動の開始までの時間間隔[s].
	float m_blockMoveModeChangeTime_s;

	//	ブロックの左右移動における時間間隔[s].
	float m_blockMoveDeltaTime_s;
	
	//	ブロックを↓キーで落下させた場合,
	//	次のブロックを落下させないように,
	//	↓キーを上げるまで落下させないようにする.
	bool m_waitUpDownArrowKey;
	
	//	ゲームのステートを保持する.
	GameState m_gameState = GameState.PLAY_GAME;
	
	bool m_enableVanishEffect;

	//	フィールドの全要素を空白にする.
	private void clearFieldState()
	{
		for ( int i = 0;i < cTetrisParam.MAP_WIDTH; ++i )
		{
			for ( int j = 0;j < cTetrisParam.MAP_HEIGHT; ++j )
			{
				m_field[ j, i ] = FieldBlockType.NONE;
			}
		}
	}
	
	//	フィールドを初期状態にリセットする.
	private void resetFieldState()
	{
		clearFieldState ();
		
		//	壁を設定する.
		for ( int i = 0;i <  cTetrisParam.SPARE_DISTANCE; ++i )
		{
			for ( int j = 0;j < cTetrisParam.MAP_HEIGHT; ++j )
			{
				m_field[ j, i ] = FieldBlockType.WALL;
				m_field[ j, i +  cTetrisParam.FIELD_WIDTH + cTetrisParam.SPARE_DISTANCE ] = FieldBlockType.WALL;
			}
		}
		for ( int i =  cTetrisParam.SPARE_DISTANCE;i <  cTetrisParam.FIELD_WIDTH + cTetrisParam.SPARE_DISTANCE; ++i )
		{
			for ( int j = cTetrisParam.FIELD_HEIGHT;j < cTetrisParam.MAP_HEIGHT; ++j )
			{
				m_field[ j, i ] = FieldBlockType.WALL;
			}
		}
	}




	private bool checkCollision(int blockLeft, int blockTop, Block block)
	{
		
		for( int i = 0;i < block.m_blockMapSize; ++i )
		{
			for( int j = 0;j < block.m_blockMapSize; ++j )
			{
				if( !block.m_blockMap[ j, i ] )
				{
					continue;
				}
				
				if( m_field[ blockTop + j, blockLeft + i ] != FieldBlockType.NONE )
				{
					return true;
				}
			}
		}
		
		return false;
	}
	
	private void moveControlBlockToLeft()
	{
		//	ブロックの端が左端に達している場合.
		if ( m_controlBlockLeft - 1 < 0 )
		{
			return;
		}
		
		//	ブロックを動かすと他のブロックと衝突する場合.
		if( checkCollision( m_controlBlockLeft - 1, m_controlBlockTop,
		                   m_blockManager.GetControlBlock() ) )
		{
			return;
		}
		
		--m_controlBlockLeft;
	}
	
	private void moveControlBlockToRight()
	{
		//	ブロックの端がフィールドの右端に達している場合.
		if ( m_controlBlockLeft + 1 >=  cTetrisParam.FIELD_WIDTH +  cTetrisParam.SPARE_DISTANCE )
		{
			return;
		}
		
		//	ブロックを動かすと他のブロックと衝突する場合.
		if( checkCollision ( m_controlBlockLeft + 1, m_controlBlockTop, 
		                    m_blockManager.GetControlBlock() ) )
		{
			return;
		}
		
		++m_controlBlockLeft;
	}
	
	private void setUpBlock()
	{
		//	現在操作中のブロックを取得.
		Block block = m_blockManager.GetControlBlock ();
		
		for( int i = 0;i < block.m_blockMapSize; ++i )
		{
			for (int j = 0;j < block.m_blockMapSize; ++j )
			{
				//	ブロックが存在する場合.
				if( block.m_blockMap[ j, i ] )
				{
					m_field[ j + m_controlBlockTop, i + m_controlBlockLeft ] = m_blockManager.GetBlockType();
				}
			}
		}
	}
	
	private void resetControlBlockParameter()
	{
		m_controlBlockLeft	= cTetrisParam.MAP_WIDTH / 2;
		m_controlBlockTop	= 0;
		m_blockManager.ResetControlBlockParameter ();
	}
	
	//	列が揃っているか確認する.
	private bool checkRow(int row)
	{
		for ( int i =  cTetrisParam.SPARE_DISTANCE;i < cTetrisParam.MAP_WIDTH -  cTetrisParam.SPARE_DISTANCE; ++i )
		{
			if( m_field[ row, i ] == FieldBlockType.NONE )
			{
				return false;
			}
		}
		
		return true;
	}
	
	//	列を削除し上のブロックを1段落とす.
	private void downUpperBlock( int row )
	{
		for ( int i = row - 1;i >= 0; --i )
		{
			for( int j = 0;j < cTetrisParam.MAP_WIDTH; ++j )
			{
				m_field[ i + 1, j ] = m_field[ i, j ];
				
				if( m_field[ i, j ] != FieldBlockType.WALL )
				{
					m_field[ i, j ] = FieldBlockType.NONE;
				}
			}
		}
	}
	
	//	列が揃っている場合は削除し上のブロックを落とす.
	private void deleteRow()
	{
		
		int deleteRowCounter = 0;
		
		for ( int i = 0;i < cTetrisParam.FIELD_HEIGHT; ++i )
		{
			if( !checkRow( i ) )
			{
				continue;
			}
			
			downUpperBlock( i );
			
			++deleteRowCounter;
		}
		
		if ( deleteRowCounter == 0 )
		{
			return;
		}
		
		//	スコアを加算する.
		cScore.AddScore ( deleteRowCounter );

		int nextLevel = cScore.GetLevel();
		if ( nextLevel <= cTetrisParam.MAX_LEVEL_UPPER_DIFFICULTY && nextLevel > m_nowLevel )
		{
			BLOCK_FALL_TIME_MAX_SEC *= cTetrisParam.BLOCK_FALL_LEVEL_ATTENUATION;
			m_nowLevel = nextLevel;
		}
	}
	
	private bool checkVanishRows()
	{
		
		int vanishRowCounter = 0;
		
		for ( int i = 0;i < cTetrisParam.FIELD_HEIGHT; ++i )
		{
			if( !checkRow( i ) )
			{
				continue;
			}
			
			for ( int j =  cTetrisParam.SPARE_DISTANCE;j <  cTetrisParam.FIELD_WIDTH +  cTetrisParam.SPARE_DISTANCE; ++j )
			{
				m_field[i, j] = FieldBlockType.VANISH;
			}
			
			++vanishRowCounter;
		}
		
		if ( vanishRowCounter == 0)
		{
			return false;
		}

		m_blockRenderer.EnableVanishEffect ();
		
		m_enableVanishEffect = true;
		
		return true;
	}
	
	//	下に動かせなかった場合,falseを返す.
	//	下に動かせた場合,trueを返す.
	private bool downControlBlock()
	{
		//	下に動かした場合に衝突しなかった場合.
		if ( !checkCollision ( m_controlBlockLeft, m_controlBlockTop + 1,
		                      m_blockManager.GetControlBlock() ) )
		{
			++m_controlBlockTop;
			return true;
		}
		
		setUpBlock ();
		
		if ( checkVanishRows() )
		{
			return false;
		}
		
		deleteRows ();
		
		return false;
	}
	
	private void deleteRows()
	{
		
		resetControlBlockParameter ();
		
		deleteRow ();
		
		m_blockRenderer.SetRenderNextBlock(
			m_blockManager.GetNextBlock(), m_blockManager.GetNextBlockType() );
		
		m_waitUpDownArrowKey = true;
		
		//	新たに生成したブロックが出現時点で設置済みのブロックと衝突している場合.
		if( checkCollision( m_controlBlockLeft, m_controlBlockTop, m_blockManager.GetControlBlock() ) )
		{
			
			//	ゲームオーバーに遷移する.
			m_gameState = GameState.GAME_OVER;
			
			clearFieldState();
			setEnableGameObject();
			
			m_blockRenderer.ResetRenderNextBlock();
		}
	}
	
	private bool checkCanSpinControlBlock( Block spinBlock )
	{
		
		//	回転させても衝突するブロックがない場合.
		if( !checkCollision ( m_controlBlockLeft, m_controlBlockTop,
		                     spinBlock ) )
		{
			return true;
		}
		
		if ( m_controlBlockLeft <  cTetrisParam.SPARE_DISTANCE )
		{
			for ( int i = 1;i <  cTetrisParam.SPARE_DISTANCE + 1; ++i )
			{
				
				if( !checkCollision ( m_controlBlockLeft + i, m_controlBlockTop,
				                     spinBlock ) )
				{
					m_controlBlockLeft += i;		
					return true;
				}
			}
		}
		else
		{
			for ( int i = 1;i <  cTetrisParam.SPARE_DISTANCE + 1; ++i )
			{
				if( !checkCollision ( m_controlBlockLeft - i, m_controlBlockTop,
				                     spinBlock ) )
				{
					m_controlBlockLeft -= i;
					return true;
				}
			}
		}
		
		return false;
	}
	
	private void spinControlBlock()
	{
		if ( !checkCanSpinControlBlock( m_blockManager.GetControlBlockSpin() ) )
		{
			return;
		}
		
		m_blockManager.SpinBlock ();
	}
	
	private void spinReverseControlBlock()
	{
		if ( !checkCanSpinControlBlock( m_blockManager.GetControlBlockSpinReverse() ) )
		{
			return;
		}
		
		m_blockManager.SpinBLockReverse ();
	}
	
	//	画面に表示するブロックを設定する.
	private void setEnableGameObject()
	{
		m_blockRenderer.SetRenderBlock ( m_field );
	}
	
	//	操作中のブロックを元に画面に表示するブロックを設定する.
	private void setEnableGameObjectByControlBlock()
	{
		
		//	落下地点を予測したブロックを配置する.
		
		int downDist = 0;
		while( !checkCollision( m_controlBlockLeft, m_controlBlockTop + downDist,
		                       m_blockManager.GetControlBlock() ) )
		{
			++downDist;
		}
		
		m_blockRenderer.SetRenderBlockFallPosition(
			m_controlBlockLeft, m_controlBlockTop + downDist - 1, m_blockManager.GetControlBlock(),
			m_blockManager.GetBlockType() );
		
		
		m_blockRenderer.SetRenderBlock (
			m_controlBlockLeft, m_controlBlockTop, 
			m_blockManager.GetControlBlock(), m_blockManager.GetBlockType() );
	}
	
	private void updateMoveQuick()
	{
		//	ブロックを移動する入力が行われていた場合.
		if( m_blockMoveDeltaTime_s > 0.0f )
		{
			m_blockMoveDeltaTime_s -= Time.deltaTime;
		}
		
		if ( Input.GetKey ( KeyCode.LeftArrow ) )
		{
			if( m_blockMoveDeltaTime_s <= 0.0f )
			{
				moveControlBlockToLeft();
				
				m_blockMoveDeltaTime_s = cTetrisParam.BLOCK_MOVE_DELTA_TIME_MAX_SEC;
			}
		}
		else if ( Input.GetKey ( KeyCode.RightArrow ) )
		{
			if( m_blockMoveDeltaTime_s <= 0.0f )
			{
				moveControlBlockToRight();
				
				m_blockMoveDeltaTime_s = cTetrisParam.BLOCK_MOVE_DELTA_TIME_MAX_SEC;
			}
		}
		else
		{
			m_blockMoveDeltaTime_s = -0.1f;
			m_blockMoveModeChangeTime_s = 0.0f;
		}
	}
	
	private void updateMoveBlock()
	{
		
		if( m_blockMoveModeChangeTime_s >= cTetrisParam.BLOCK_MOVE_MODE_CHANGE_TIME_MAX_SEC )
		{
			updateMoveQuick();
			return;
		}
		
		if ( Input.GetKey ( KeyCode.LeftArrow ) || Input.GetKey ( KeyCode.RightArrow ) )
		{
			m_blockMoveModeChangeTime_s += Time.deltaTime;
		}
		else
		{
			m_blockMoveModeChangeTime_s = 0.0f;
		}
		
		if ( Input.GetKeyDown ( KeyCode.LeftArrow ) )
		{
			moveControlBlockToLeft();
		}
		if ( Input.GetKeyDown ( KeyCode.RightArrow ) )
		{
			moveControlBlockToRight();
		}
	}
	
	private void updateFallBlock()
	{
		
		//	瞬時に落下させる.
		if ( Input.GetKeyDown( KeyCode.UpArrow ) )
		{
			while( downControlBlock() )
			{
			}
			
			return;
		}
		
		if( m_waitUpDownArrowKey )
		{
			if ( Input.GetKeyUp( KeyCode.DownArrow ) )
			{
				m_waitUpDownArrowKey = false;
			}
		}
		else if( Input.GetKey( KeyCode.DownArrow ) )
		{
			downControlBlock();
			
			m_blockFallDeltaTime = 0.0f;
		}
		
		//	前回落下したときからの時間経過を計算し,
		//	一定時間以上経過している場合に落下処理を行う.
		m_blockFallDeltaTime += Time.deltaTime;
		
		if( m_blockFallDeltaTime >= BLOCK_FALL_TIME_MAX_SEC )
		{
			downControlBlock ();
			m_blockFallDeltaTime = 0.0f;
		}
		
	}
	
	public void UpdateGame()
	{

		if ( m_enableVanishEffect )
		{
			if ( m_blockRenderer.FinishVanishEffect() )
			{
				deleteRows ();
				
				m_enableVanishEffect = false;
			}
			
			//	ブロックの状態をGameObjectに反映させる.
			setEnableGameObject ();
			//	setEnableGameObjectByControlBlock ();
			
			return;
		}
		
		//	ブロックの左右移動.
		updateMoveBlock ();
		
		//	ブロックの回転.
		if( Input.GetKeyDown ( KeyCode.Space ) | Input.GetKeyDown ( KeyCode.Z) )
		{
			spinControlBlock();
		}
		
		if( Input.GetKeyDown ( KeyCode.X ) )
		{
			spinReverseControlBlock();
		}
		
		//	ブロックの落下.
		updateFallBlock ();
		
		if( m_gameState != GameState.PLAY_GAME )
		{
			return;
		}
		
		//	ブロックの状態をGameObjectに反映させる.
		setEnableGameObject ();
		setEnableGameObjectByControlBlock ();
	}
	
	public GameState GetGameState()
	{
		return m_gameState;
	}
	
	public void StopRenderField()
	{
		m_blockRenderer.SetAllActive ( false ) ;
	}
	
	public void RestartRenderField()
	{
		m_blockRenderer.SetRenderNextBlock(
			m_blockManager.GetNextBlock(), m_blockManager.GetNextBlockType() );
	}

};
