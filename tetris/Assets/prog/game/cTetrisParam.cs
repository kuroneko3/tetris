
class cTetrisParam
{

//	フィールドサイズに関するパラメータ.

	public const int FIELD_WIDTH	= 10;
	public const int FIELD_HEIGHT	= 20;

	public const int SPARE_DISTANCE = 2;

	public const int MAP_WIDTH		= FIELD_WIDTH + SPARE_DISTANCE * 2;
	public const int MAP_HEIGHT	= FIELD_HEIGHT + SPARE_DISTANCE;


//	ゲームバランスに関するパラメータ.
	public const float BLOCK_FALL_TIME_MAX_SEC_DEFAULT = 1.0f;
	public const float BLOCK_FALL_LEVEL_ATTENUATION = 0.9f;
	public const float BLOCK_MOVE_MODE_CHANGE_TIME_MAX_SEC = 0.5f;
	public const float BLOCK_MOVE_DELTA_TIME_MAX_SEC = 0.05f;

//	スコアに関するパラメータ.

	public const int LEVEL_UP_ROW_COUNT = 10;
	public const int MAX_LEVEL_UPPER_DIFFICULTY = 20;

};
