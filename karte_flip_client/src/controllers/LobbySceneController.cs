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
		GameTurnManager.GetInstance().SetGamePlayType(GamePlayTypeEnum.VS_COMPUTER);
		_routeManager.MoveToScene(SceneFilenameEnum.MAIN_SCENE, null);
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
		Multiplayer.MultiplayerPeer = null;
		_displayDialog.CloseDialog();
	}
}
