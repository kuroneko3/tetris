using UnityEngine;
using System.Collections;

class cSceneChanger
{

	public static void ChangeScene ( eScene nextScene )
	{

		string sceneName = selectSceneName ( nextScene );

		if ( sceneName == "" )
		{
			return;
		}

		Application.LoadLevel ( sceneName );

	}

	static string selectSceneName ( eScene nextScene )
	{
		switch ( nextScene )
		{
		case eScene.TITLE:
			return "titleScene";

		case eScene.GAME:
			return "gameScene";

		case eScene.RESULT:
			return "resultScene";

		case eScene.OLD_GAME:
			return "oldgame";
		}

		return "";
	}
};
