using Godot;
using System;
using System.Threading.Tasks;

public partial class LobbySceneController : Node2D{
	private RouteManager mRoute;
	private AudioStreamPlayer mBackgroundMusic;

	public override void _Ready(){
		mRoute = RouteManager.GetIntance();
		GetNode<AudioableButton>("PlayGameButton").Connect(
			"pressed",
			new Callable(this, "OnPressedPlayAIButton")
		);
		GetNode<AudioableButton>("CreditsButton").Connect(
			"pressed",
			new Callable(this, "OnPressedCreditsButton")
		);
		mBackgroundMusic = GetNode<AudioStreamPlayer>("BackgroundAudioStreamPlayer");
		mBackgroundMusic.Play();
	}

    public override void _Process(double delta){
        if(mBackgroundMusic.Playing == false){
			mBackgroundMusic.Play();
		}
    }

    private async void OnPressedPlayAIButton(){
		await Task.Delay(500);
		GameTurnManager.GetInstance().SetGamePlayType(GamePlayTypeEnum.VS_COMPUTER);
		mRoute.MoveToScene(SceneFileNameEnum.MAIN_SCENE, GetTree());
	}

	private async void OnPressedCreditsButton(){
		await Task.Delay(500);
		mRoute.MoveToScene(SceneFileNameEnum.CREDIT_SCENE, GetTree());
	}

}
