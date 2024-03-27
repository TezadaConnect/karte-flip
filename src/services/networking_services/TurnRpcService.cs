using Godot;

public partial class TurnRpcService : NetworkingService{
    private GameTurnManager _gameTurnManager;
    private PlayerManager _playerManager;
    private ScoringManager _scoringManager;
    private MainGameInterface _mainGameInterFace;

    public override void _Ready(){
        base._Ready();
        _gameTurnManager = GameTurnManager.GetInstance();
        _playerManager = PlayerManager.GetInstance();
        _scoringManager = ScoringManager.GetInstance();
    }

    public void SetInterfaceItem(MainGameInterface mainGameInterface){
        _mainGameInterFace = mainGameInterface;
    }

    public void OnTapTile(
        InputEvent @event, 
        GridGroundTilemap tilemap, 
        bool isOpenDialog,
        bool isMaxTiles
    ){
        Rpc(nameof(OnTapGroundTile), @event, tilemap, isOpenDialog, isMaxTiles);
    }

    private void OnTapGroundTile(
        InputEvent @event, 
        GridGroundTilemap tilemap, 
        bool isOpenDialog,
        bool isMaxTiles
    ){

		InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;

		if(!(mouseEvent.ButtonIndex == MouseButton.Left && @event.IsReleased())){
			return;
		}

		if(isOpenDialog){ // Halt click event
			return;
		}

		Vector2I tilePostion = tilemap.LocalToMap(tilemap.GetLocalMousePosition());
		TileData groundTileData = tilemap.GetCellTileData(tilemap.GROUND_LAYER, tilePostion);
		
		if(groundTileData == null){
			return;
		} 
			
		TileData tokenPlacementTilemap = tilemap.GetCellTileData(
			tilemap.TOKEN_PLACEMENT_LAYER, 
			tilePostion
		);

		if(tokenPlacementTilemap != null){
			return;
		}

		TileHelper.AddAtlasFromGameTurnManagerToTilemap(tilePostion, tilemap);

		CardModel cardDisplay = _gameTurnManager.GetCurrentCard();

		TokenFlipService.FlipTokens( // Flip nearby tokens
			tilePostion, 
			tilemap, 
			cardDisplay.GetCardListFlipDirections()
		);

		tilemap.PlayTileDropAudio();

		// if(isMaxTiles){
		// 	_mainGameInterFace.EndGameResult();
		// 	return;
		// }

		// _mainGameInterFace.SetNextTurnPLayer();
		// _mainGameInterFace.DisplayScore();	
		
		// _gameTurnManager.SetPlayerTurn(_playerManager.GetPlayerTwo());
	}
}