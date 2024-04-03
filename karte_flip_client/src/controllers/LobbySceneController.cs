using Godot;
using System.Threading.Tasks;

public partial class LobbySceneController : Node2D{
	private RouteManager _routeManager;
	private AudioStreamPlayer mBackgroundMusic;
	private NetworkingService _networkingService;
	private DisplayDialog _displayDialog;

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

		_displayDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
		AddChild(_displayDialog);
		_displayDialog.SetDialogType(DialogType.ONE_BUTTON);
		_displayDialog.GetConfirmButton().Pressed += CancelFindingMatch;
		_displayDialog.GetConfirmButton().Text = "Cancel";
	}

    public override void _Process(double delta){
        if(mBackgroundMusic.Playing == false){
			mBackgroundMusic.Play();
		}
    }

    private async void OnPressedPlayAIButton(){
		await Task.Delay(500);
		GetNode<PlayerManager>(
			RouteManager.GetSingletonAutoLoad(
				SingletonAutoLoadEnum.PLAYER_MANAGER
			)
		).InitComputerVsPlayer();
		_routeManager.MoveToScene(SceneFilenameEnum.VS_COMPUTER_MATCH_SCENE, "Starting match, please wait.");
	}

	private async void OnPressedCreditsButton(){
		await Task.Delay(500);
		_routeManager.MoveToScene(SceneFilenameEnum.CREDIT_SCENE, "Loading credits, please wait.");
	}

	private async void OnPressedFindMatchButton(){
		await Task.Delay(500);
		_displayDialog.ShowDialog("Finding Match");
		_networkingService.JoinAServer();
	}

	private void CancelFindingMatch(){
		_networkingService.CloseConnection();
		_displayDialog.CloseDialog();
	}
}
