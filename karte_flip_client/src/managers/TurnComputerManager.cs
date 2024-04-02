using System.Threading.Tasks;
using Godot;
using KarteFlipClient;

public partial class TurnComputerManager : Node{
    PlayerManager _playerManager;
    private ScoringManager _scoringManager;
    private GridGroundTilemap _mainTilemap;
    private TextureRect _turnDisplayTextureRect;
    private CardModel _aiRandomCard;

    public override void _Ready(){
        _aiRandomCard = TileHelper.GetRandomCard();
        _playerManager = GetNode<PlayerManager>(
            RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.PLAYER_MANAGER)
        );
        _scoringManager = GetNode<ScoringManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.SCORING_MANAGER)
		);
    }

    public bool IsMyTurn(){
        if(_playerManager.CurrentPlayerTurn.PlayerType == PlayerTypeEnum.PERSON){
            return true;
        }
        return false;
	}

    public void AddTokenToTilemap(Vector2I position, CardModel card){
        _mainTilemap ??= GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
        
        TileHelper.AddAtlasToken(
            position, 
            _mainTilemap, 
            _playerManager.CurrentPlayerTurn.TokenColor
        );
        TokenFlipService.FlipTokens(
            position, 
            _mainTilemap, 
            card.CardListFlipDirections, 
            _playerManager.CurrentPlayerTurn.TokenColor
        );

        ShiftPlayer();
        _scoringManager.DisplayScore(_mainTilemap);
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
    }

    private void ShiftPlayer(){
        _turnDisplayTextureRect ??= GetNode<TextureRect>("/root/MainScene/HUDTextureRect/TurnTextureRect");
        _playerManager.CurrentPlayerTurn = _playerManager.CurrentPlayerTurn.Equals(
            _playerManager.PlayerOne
        ) ? _playerManager.PlayerTwo : _playerManager.PlayerOne;

        if(_playerManager.CurrentPlayerTurn.TokenColor == TokenColorEnum.DARK_TOKEN){
            _turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.BLACK_TOKEN);
        } else {
            _turnDisplayTextureRect.Texture = RouteManager.GetLocalAssetInTexture2D(LocalAssetFileNameEnum.WHITE_TOKEN);
        }
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

    // public void 
}