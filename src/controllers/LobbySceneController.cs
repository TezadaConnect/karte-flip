using Godot;
using System;

public partial class LobbySceneController : Node2D{
	private RouteManager mRoute;
	private Button mPlayButton;

	public override void _Ready(){
		mRoute = RouteManager.GetIntance();
		mPlayButton = GetNode<Button>("PlayGameButton");
		mPlayButton.Connect("pressed",new Callable(this, "OnPressedPlayAIButton"));
	}

	private void OnPressedPlayAIButton(){
		GameTurnManager SEAN =  GameTurnManager.GetInstance();
		SEAN.SetGamePlayType(GamePlayTypeEnum.VS_COMPUTER);
		mRoute.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

}
