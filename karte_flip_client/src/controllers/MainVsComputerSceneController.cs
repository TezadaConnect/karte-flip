using System.Threading.Tasks;
using Godot;

public partial class MainVsComputerSceneController: MainSceneController {

    TurnComputerManager _computerTurnManager;

     public override void _Ready(){
		InitAutoLoads();
		InitUiBindings();
		InitializeListeners();
		DisplayRandomCard();
		_computerTurnManager.ComputerFirstTurn();
	}

    private void InitAutoLoads(){
		_routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
		_computerTurnManager = GetNode<TurnComputerManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.TURN_COMPUTER_MANAGER)
		);
	}

    public override void _Input(InputEvent @event){
		if(_tilemap.IsMaxTiles()){
			return;
		}

		if(!_displayDialog.IsDialogHidden()){
			return;
		}

		if(@event is InputEventMouseButton){
			if(_computerTurnManager.IsMyTurn()){
				OnTapGroundTile(@event);
			}
		}
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

		_computerTurnManager.AddTokenToTilemap(tilePostion, _randomCard);
		_tilemap.PlayTileDropAudio();
		DisplayRandomCard();
        _computerTurnManager.ComputerTapGroundTile();
	}

	protected override async void OnPressedDialogConfirmButton(){
		if(Dialogs.RESTART_DIALOGUE == _currentDialogue){
            _computerTurnManager.ResettingTurn();
            _randomCard = TileHelper.GetRandomCard();
			_computerTurnManager.ComputerFirstTurn();
			_displayDialog.CloseDialog();
		}

		if(Dialogs.QUIT_DIALOGUE == _currentDialogue){
			Multiplayer.MultiplayerPeer = null;
            _computerTurnManager.ResettingTurn();
			await Task.Delay(500);
			_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE, "Leaving game, please wait.");
		}
	}
}