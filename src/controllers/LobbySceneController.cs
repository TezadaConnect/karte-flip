using Godot;
using System;

public partial class LobbySceneController : Node2D{
	private RouteManager mRoute;
	private Button mPlayButton;

	public override void _Ready(){
		mRoute = RouteManager.GetIntance();
		mPlayButton = GetNode<Button>("PlayGameButton");
		mPlayButton.Connect("pressed",new Callable(this, "OnPressedPlayButton"));
	}

	private void OnPressedPlayButton(){
		GetTree().ChangeSceneToFile(mRoute.GetSceneFilePath(SceneFileNameEnum.MAIN_SCENE));
	}

}
