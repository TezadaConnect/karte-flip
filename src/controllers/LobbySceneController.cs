using Godot;
using System;

public partial class LobbySceneController : Node2D{
	private RouteManager mRoute;
	private AudioStreamPlayer mBackgroundMusic;

	public override void _Ready(){
		mRoute = RouteManager.GetIntance();
		GetNode<Button>("PlayGameButton").Connect(
			"pressed",
			new Callable(this, "OnPressedPlayAIButton")
		);
		mBackgroundMusic = GetNode<AudioStreamPlayer>("BackgroundAudioStreamPlayer");
		mBackgroundMusic.Play();
	}

    public override void _Process(double delta){
        if(mBackgroundMusic.Playing == false){
			mBackgroundMusic.Play();
		}
    }

    private void OnPressedPlayAIButton(){
		GameTurnManager.GetInstance().SetGamePlayType(GamePlayTypeEnum.VS_COMPUTER);
		mRoute.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

}
