
class cScore
{

	const int LEVEL_UP_ROW_COUNT = 20;

	static int s_score = 0;
	static int s_level = 1;
	static int s_deleteRowCount = 0;

	static public void Reset ()
	{
		s_score = 0;
		s_level = 1;
		s_deleteRowCount = 0;
	}

	static public void AddScore ( int deletedRowNumber )
	{
		s_deleteRowCount += deletedRowNumber;

		if ( s_deleteRowCount >= LEVEL_UP_ROW_COUNT )
		{
			++s_level;
		}

		s_score += deletedRowNumber * deletedRowNumber * s_level * cTetrisParam.FIELD_WIDTH;
	}

	static public int GetLevel ()
	{
		return s_level;
	}

	static public int GetScore ()
	{
		return s_score;
	}

	static public int GetDeleteRowCount ()
	{
		return s_deleteRowCount;
	}

};
