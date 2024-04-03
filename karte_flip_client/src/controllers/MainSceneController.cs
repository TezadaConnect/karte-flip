using Godot;

public enum Dialogs { RESTART_DIALOGUE, QUIT_DIALOGUE, DISABLE_DIALOGUE }

public partial class MainSceneController : Node {
	// Managers
	protected RouteManager _routeManager;
	
	// UI NODE
	protected GridGroundTilemap _tilemap;
	protected TextureRect _hudTextureRect;
	
	// Dialogues
	protected Dialogs _currentDialogue;
	protected DisplayDialog _displayDialog;

	// Data Models
	protected CardModel _randomCard;

	protected void InitializeListeners(){
		//Button Listeners
		GetNode<Button>("QuitButton").Pressed += OnPressedQuitButton;
		GetNode<Button>("RestartButton").Pressed += OnPressedRestartButton;
	}

	protected void InitUiBindings(){
		// Nodes
		_tilemap = GetNode<GridGroundTilemap>("GridGroundTilemap");
		_hudTextureRect = GetNode<TextureRect>("HUDTextureRect");
		// Dialogues
		_displayDialog = RouteManager.GetDrawables(SceneFilenameEnum.DISPLAY_DIALOG).Instantiate<DisplayDialog>();
		AddChild(_displayDialog);
		_displayDialog.SetDialogType(DialogType.TWO_BUTTON);
		_displayDialog.GetConfirmButton().Pressed += OnPressedDialogConfirmButton;
		_displayDialog.GetCancelButton().Pressed += OnPressedDialogCancelButton;
	}

	protected virtual void OnPressedDialogConfirmButton(){}

	protected virtual void OnPressedRestartButton(){
		_displayDialog.ShowDialog("Are you sure about restarting the game?");
		_currentDialogue = Dialogs.RESTART_DIALOGUE;
	}

	protected void OnPressedQuitButton(){
		_displayDialog.ShowDialog("Are you sure about quiting game?");
		_currentDialogue = Dialogs.QUIT_DIALOGUE;
	}

	protected void OnPressedDialogCancelButton(){
		_displayDialog.CloseDialog();
	}

	protected void SetRandomCard(){
		_randomCard = TileHelper.GetRandomCard();
	}

	protected void DisplayRandomCard(){
		SetRandomCard();
		Label randomCardDescription = _hudTextureRect.GetNode<Label>("RandomCardDescription");
		Label randomCardName = _hudTextureRect.GetNode<Label>("RandomCardName");
		TextureRect randomCardTexture = GetNode<TextureRect>("HUDTextureRect").GetNode<TextureRect>("RandomCardTextureRect");
		randomCardTexture.Texture = RouteManager.GetLocalAssetInTexture2D(_randomCard.CardFilename);
		randomCardName.Text = _randomCard.CardName;
		randomCardDescription.Text = _randomCard.CardDescription;
	}

}