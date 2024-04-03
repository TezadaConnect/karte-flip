using System.Threading.Tasks;
using Godot;
using KarteFlipClient;

public partial class TurnComputerManager : Node{
    PlayerManager _playerManager;
    private ScoringManager _scoringManager;
    private GridGroundTilemap _mainTilemap;
    private TextureRect _turnDisplayTextureRect;
    private CardModel _aiRandomCard;
    private DisplayDialog _resultDialog;

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

    public void ComputerFirstTurn(){
        if(_playerManager.CurrentPlayerTurn.PlayerType == PlayerTypeEnum.COMPUTER){
            ComputerTapGroundTile();
        }
    }

    private void EndGameResult(){
        // GridGroundTilemap _mainTilemap = GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap"); ensurr this is not empty
        if(!_mainTilemap.IsMaxTiles()){
            return;
        }
        
        bool isblackWin = _scoringManager.BlackScore > _scoringManager.WhiteScore;

        if(isblackWin){
            ShowResultModal(TokenColorEnum.DARK_TOKEN);
            return;
        }
        
        ShowResultModal(TokenColorEnum.LIGHT_TOKEN); 
    }

    private void ShowResultModal(TokenColorEnum winningColor){ // Todo make this more faster
        MainVsComputerSceneController mainScene = GetNode<MainVsComputerSceneController>("/root/MainScene");
        mainScene.AddChild(_resultDialog);
        _resultDialog.SetDialogType(DialogType.ONE_BUTTON);
        _resultDialog.GetConfirmButton().Text = "Ok";
        _resultDialog.GetConfirmButton().Pressed += async () => {
            _resultDialog.CloseDialog();
            await Task.Delay(2000);
            _resultDialog.Free();
            _resultDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
        };

        bool isMeWithColorWinning = _playerManager.PlayerOne.TokenColor == winningColor;

        if(isMeWithColorWinning){
            _resultDialog.ShowDialog("You Win!");
            _resultDialog.SetDialogType(DialogType.ONE_BUTTON);
            _resultDialog.PlayWinSoundEffect();
            return;
        }
                
        _resultDialog.ShowDialog("You Lose!"); 
        _resultDialog.PlayLoseSoundEffect();
    }
}