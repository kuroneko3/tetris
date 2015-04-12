
class cScore
{

	static int s_score = 0;
	static int s_level = 1;

	static public void Reset ()
	{
		s_score = 0;
		s_level = 1;
	}

	static public void AddScore ( int deletedRowNumber )
	{

	}

	static public int GetLevel ()
	{
		return s_level;
	}

	static public int GetScore ()
	{
		return s_score;
	}

};
