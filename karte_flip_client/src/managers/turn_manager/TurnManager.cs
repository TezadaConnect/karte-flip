using System;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;
using KarteFlipClient;

public partial class TurnManager: Node {
    protected PlayerManager _playerManager;
    protected ScoringManager _scoringManager;
    protected GridGroundTilemap _mainTilemap;
    protected TextureRect _turnDisplayTextureRect;
    protected CardModel _aiRandomCard;
    protected DisplayDialog _resultDialog;

    protected void ShiftPlayer(){
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

    protected void EndGameResult(){
        // GridGroundTilemap _mainTilemap = GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap"); ensure this is not empty
		if(!_mainTilemap.IsMaxTiles()){
			return;
		}
			
		bool isblackWin = _scoringManager.BlackScore > _scoringManager.WhiteScore;

		if(isblackWin){
			MakeResultDialog(TokenColorEnum.DARK_TOKEN);
			return;
		}
			
		MakeResultDialog(TokenColorEnum.LIGHT_TOKEN); 
	}

    protected void InstantiateResultDialog(Node mainScene){
        mainScene.AddChild(_resultDialog);
        _resultDialog.SetDialogType(DialogType.ONE_BUTTON);
        _resultDialog.GetConfirmButton().Text = "Ok";
        _resultDialog.GetConfirmButton().Pressed += async () => {
            _resultDialog.CloseDialog();
            await Task.Delay(2000);
            _resultDialog.Free();
            _resultDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
        };
    }

    protected void ShowDialogBaseOnResult(bool isWinner){
        if(isWinner){
            _resultDialog.ShowDialog("You Win!");
            _resultDialog.SetDialogType(DialogType.ONE_BUTTON);
            _resultDialog.PlayWinSoundEffect();
            return;
        }
					
		_resultDialog.ShowDialog("You Lose!"); 
		_resultDialog.PlayLoseSoundEffect();
    }

    protected void DrawTokens(Vector2I position, Array<DirectionEnum> directions){
        _mainTilemap ??= GetNode<GridGroundTilemap>("/root/MainScene/GridGroundTilemap");
        TileHelper.AddAtlasToken(
            position, 
            _mainTilemap, 
            _playerManager.CurrentPlayerTurn.TokenColor
        );
        TokenFlipService.FlipTokens(
            position, 
            _mainTilemap, 
            directions, 
            _playerManager.CurrentPlayerTurn.TokenColor
        );
    }

    /*
    * ********************************************************
    *	Overrides
    * ********************************************************
    */
    public virtual bool IsMyTurn(){
		return false; // default
	}

    protected virtual void MakeResultDialog(TokenColorEnum winningColor){}
}