using System.Threading.Tasks;
using Godot;

public enum DialogType { ONE_BUTTON, TWO_BUTTON, TIMER_ONE_BUTTON }

public partial class DisplayDialog : Node2D{
	private TextureRect _dialogTextureRect;
	private Button _confirmButton;
	private Button _cancelButton;

	public override void _Ready(){
		_dialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
		SetDialogType(DialogType.ONE_BUTTON);
	}

	public void SetDialogType(DialogType dialogType){
		if(dialogType == DialogType.ONE_BUTTON){
 			_dialogTextureRect.GetNode<Button>("Confirm2Button").Hide();
			_dialogTextureRect.GetNode<Button>("CancelButton").Hide();
			_confirmButton = _dialogTextureRect.GetNode<Button>("Confirm1Button");
			_confirmButton.Text = "Ok";
			_confirmButton.Show();
			return;
		}
		
		_dialogTextureRect.GetNode<Button>("Confirm1Button").Hide();
		_cancelButton = _dialogTextureRect.GetNode<Button>("CancelButton");
		_confirmButton = _dialogTextureRect.GetNode<Button>("Confirm2Button");
		_cancelButton.Text = "No";
		_confirmButton.Text = "Yes";
		_cancelButton.Show();
		_confirmButton.Show();
	}

	public async void SetDialogType(DialogType dialogType, int miliseconds){
		if(dialogType == DialogType.TIMER_ONE_BUTTON){
 			_dialogTextureRect.GetNode<Button>("Confirm2Button").Hide();
			_dialogTextureRect.GetNode<Button>("CancelButton").Hide();
			_dialogTextureRect.GetNode<Button>("Confirm1Button").Hide();
			_confirmButton = _dialogTextureRect.GetNode<Button>("Confirm1Button");
			_confirmButton.Text = "Ok";
			int timeDelay = miliseconds == 0 ? 2000 : miliseconds;
			await Task.Delay(timeDelay);
			_confirmButton.Show();
			return;
		}
		SetDialogType(dialogType);
	}

	public void ShowDialog(string message){
		_dialogTextureRect.GetNode<Label>("MessageLabel").Text = message;
		_dialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").Play("Intro");
	}

	public void CloseDialog(){
		_dialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation").PlayBackwards("Intro");
	}
	
	public Button GetConfirmButton(){
		return _confirmButton;
	}

	public Button GetCancelButton(){
		return _cancelButton;
	}

	public bool IsDialogHidden(){
		return _dialogTextureRect.Scale.X == 0 && _dialogTextureRect.Scale.Y == 0;
	}
}
