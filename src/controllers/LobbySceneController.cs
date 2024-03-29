using Godot;
using System;
using System.Threading.Tasks;

public partial class LobbySceneController : Node2D{
	private RouteManager mRoute;
	private AudioStreamPlayer mBackgroundMusic;
	private NetworkingService _networkingService;

	public override void _Ready(){
		mRoute = RouteManager.GetIntance();
		_networkingService = GetNode<NetworkingService>(
			mRoute.GetSingletonAutoLoad(SingletonAutoLoadEnum.NETWORKING_SERVICE)
		);
		GetNode<AudioableButton>("PlayGameButton").Pressed += OnPressedPlayAIButton;
		GetNode<AudioableButton>("CreditsButton").Pressed += OnPressedCreditsButton;
		GetNode<AudioableButton>("CreateServerButton").Pressed += OnPressedCreateServerButton;
		GetNode<AudioableButton>("JoinServerButton").Pressed += OnPressedJoinServerButton;
		// Playe background music
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

	private async void OnPressedCreateServerButton(){
		await Task.Delay(500);
		// mRoute.MoveToScene(SceneFileNameEnum.CREDIT_SCENE, GetTree());
		_networkingService.CreateAServer();
	}

	private async void OnPressedJoinServerButton(){
		await Task.Delay(500);
		// mRoute.MoveToScene(SceneFileNameEnum.CREDIT_SCENE, GetTree());
		_networkingService.JoinAServer();
	}
}
