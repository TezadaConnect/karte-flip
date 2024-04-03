using System.Threading.Tasks;
using Godot;
using KarteFlipClient;

public partial class ComputerTurnManager : TurnManager{

    public override void _Ready(){
        _aiRandomCard = TileHelper.GetRandomCard();
        _playerManager = GetNode<PlayerManager>(
            RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.PLAYER_MANAGER)
        );
        _scoringManager = GetNode<ScoringManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.SCORING_MANAGER)
		);
        _resultDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
    }

    public override bool IsMyTurn(){
        if(_playerManager.CurrentPlayerTurn.PlayerType == PlayerTypeEnum.PERSON){
            return true;
        }
        return false;
	}

    public void AddTokenToTilemap(Vector2I position, CardModel card){
        DrawTokens(position, card.CardListFlipDirections);
        ShiftPlayer();
        _scoringManager.DisplayScore(_mainTilemap);
        EndGameResult();
    }

    public async void ComputerTapGroundTile(){
        _mainTilemap ??= GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
        if(_mainTilemap.IsMaxTiles()){
            return;
        }
        
        await Task.Delay(500);

        ComputerOpponentService.GetComputerTileMove(
            _mainTilemap, 
            _aiRandomCard, 
            _playerManager.CurrentPlayerTurn.TokenColor
        );

        _mainTilemap.PlayTileDropAudio();

        ShiftPlayer();
        _scoringManager.DisplayScore(_mainTilemap);
        _aiRandomCard = TileHelper.GetRandomCard();
        EndGameResult();
    }

    public void ResettingTurn(){
        _scoringManager.ResetScore();
        _playerManager.InitComputerVsPlayer();
        if(_mainTilemap != null){
            _mainTilemap?.ClearLayer(_mainTilemap.TOKEN_PLACEMENT_LAYER);
            _scoringManager.DisplayScore(_mainTilemap);
        }
        _turnDisplayTextureRect = null;
        _mainTilemap = null;
    }

    public void ComputerFirstTurn(){
        if(_playerManager.CurrentPlayerTurn.PlayerType == PlayerTypeEnum.COMPUTER){
            ComputerTapGroundTile();
        }
    }

    protected override void MakeResultDialog(TokenColorEnum winningColor){ // Todo make this more faster
        MainVsComputerSceneController mainScene = GetNode<MainVsComputerSceneController>("/root/MainScene");
        InstantiateResultDialog(mainScene);
        bool isMeWithColorWinning = _playerManager.PlayerOne.TokenColor == winningColor;
        ShowDialogBaseOnResult(isMeWithColorWinning);
    }
}