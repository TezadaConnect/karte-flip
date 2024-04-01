using Godot;
using System.Threading.Tasks;

public partial class LobbySceneController : Node2D{
	private RouteManager _routeManager;
	private AudioStreamPlayer mBackgroundMusic;
	private NetworkingService _networkingService;

	public override void _Ready(){
		_routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
		_networkingService = GetNode<NetworkingService>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.NETWORKING_SERVICE)
		);
		GetNode<AudioableButton>("PlayGameButton").Pressed += OnPressedPlayAIButton;
		GetNode<AudioableButton>("CreditsButton").Pressed += OnPressedCreditsButton;
		GetNode<AudioableButton>("FindMatchButton").Pressed += OnPressedFindMatchButton;
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
		_routeManager.MoveToScene(SceneFilenameEnum.MAIN_SCENE);
	}

	private async void OnPressedCreditsButton(){
		await Task.Delay(500);
		_routeManager.MoveToScene(SceneFilenameEnum.CREDIT_SCENE);
	}

	private async void OnPressedFindMatchButton(){
		await Task.Delay(500);
		_networkingService.JoinAServer();
	}
}
