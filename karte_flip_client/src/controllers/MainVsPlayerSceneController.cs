using Godot;
using KarteFlipClient;
using System.Threading.Tasks;

public partial class MainVsPlayerSceneController: MainSceneController{
    private TurnRpcService _turnRpcService;

    public override void _Ready(){
		InitAutoLoads();
		InitUiBindings();
		InitializeListeners();
		DisplayRandomCard();
	}

    public override void _Input(InputEvent @event){
		if(_tilemap.IsMaxTiles()){
			return;
		}

		if(!_displayDialog.IsDialogHidden()){
			return;
		}

		if(@event is InputEventMouseButton){
			if(_turnRpcService.IsMyTurn()){
				OnTapGroundTile(@event);
			}
		}
    }

	private void InitAutoLoads(){
		_routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
		_turnRpcService = GetNode<TurnRpcService>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.TURN_RPC_SERVICE)
		);
	}

	private void OnTapGroundTile(InputEvent @event){

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsReleased())){
			return;
		}

		Vector2I tilePostion = _tilemap.LocalToMap(_tilemap.GetLocalMousePosition());
		TileData groundTileData = _tilemap.GetCellTileData(_tilemap.GROUND_LAYER, tilePostion);
		
		if(groundTileData == null){
			return;
		} 
			
		TileData tokenPlacementTilemap = _tilemap.GetCellTileData(
			_tilemap.TOKEN_PLACEMENT_LAYER, 
			tilePostion
		);

		if(tokenPlacementTilemap != null){
			return;
		}

		_turnRpcService.ExecuteAddTokenToTilemap(tilePostion, _randomCard);
		DisplayRandomCard();
	}

	protected override void OnPressedRestartButton(){
		_displayDialog.ShowDialog("You cant restart a P2P match.");
		_displayDialog.GetCancelButton().Text = "Close";
		_displayDialog.GetConfirmButton().Text = "Ok";
		_currentDialogue = Dialogs.RESTART_DIALOGUE;
	}

	protected override async void OnPressedDialogConfirmButton(){
		if(Dialogs.RESTART_DIALOGUE == _currentDialogue){
			_displayDialog.CloseDialog();	
		}

		if(Dialogs.QUIT_DIALOGUE == _currentDialogue){
			_turnRpcService.ExecuteQuitMatch();
			await Task.Delay(500);
			_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE, "Leaving game, please wait.");
		}
	}
}