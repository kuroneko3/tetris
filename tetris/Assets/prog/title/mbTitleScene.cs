using UnityEngine;
using System.Collections;

class mbTitleScene : mbScene
{
	override protected void start ()
	{
	}

	override protected void update ()
	{
		if ( Input.GetKeyDown ( KeyCode.Space ) )
		{
			cScore.Reset ();
			changeScene ( eScene.GAME );
		}

		if ( Input.GetKeyDown ( KeyCode.F1 ) )
		{
			cScore.Reset ();

			changeScene ( eScene.OLD_GAME );
		}
	}
};
