using Godot.Collections;
using System.Threading.Tasks;
using Godot;

public enum DialogType { ONE_BUTTON, TWO_BUTTON, TIMER_ONE_BUTTON }

public partial class DisplayDialog : Node2D{
	private TextureRect _dialogTextureRect;
	private Button _confirmButton;
	private Button _cancelButton;
	private AnimationPlayer _popAnumation;
	private Label _messageLabel;
	private AudioStreamPlayer _loseAudio;
	private AudioStreamPlayer _winAudio;

	public override void _Ready(){
		_dialogTextureRect = GetNode<TextureRect>("DialogueBackgroundTextureRect");
		_popAnumation = _dialogTextureRect.GetNode<AnimationPlayer>("PopupAnimation");
		_messageLabel = _dialogTextureRect.GetNode<Label>("MessageLabel");
		_loseAudio = _dialogTextureRect.GetNode<AudioStreamPlayer>("LoseAudioStreamPlayer");
		_winAudio = _dialogTextureRect.GetNode<AudioStreamPlayer>("WinAudioStreamPlayer");
		_dialogTextureRect.GetNode<Button>("Confirm1Button").Hide();
		_dialogTextureRect.GetNode<Button>("Confirm2Button").Hide();
		_dialogTextureRect.GetNode<Button>("CancelButton").Hide();
	}

	public void SetDialogType(DialogType dialogType){
		if(dialogType == DialogType.ONE_BUTTON){
			_confirmButton = _dialogTextureRect.GetNode<Button>("Confirm1Button");
			_confirmButton.Text = "Ok";
			_confirmButton.Show();
			return;
		}
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
		_messageLabel.Text = message;
		_popAnumation.Play("Intro");
	}

	public async void CloseDialog(){
		_popAnumation.PlayBackwards("Intro");
		await Task.Delay(1000);
		if(_cancelButton != null){
			GetCancelButton().Text = "No";
		}
		GetConfirmButton().Text = "Yes";	
	}
	
	public Button GetConfirmButton(){
		return _confirmButton;
	}

	public Button GetCancelButton(){
		return _cancelButton;
	}

	public bool IsDialogHidden(){
		return _dialogTextureRect.Scale.X != 1 && _dialogTextureRect.Scale.Y != 1;
	}

	public void PlayLoseSoundEffect(){
		_loseAudio.Play();
	}

	public void PlayWinSoundEffect(){
		_winAudio.Play();
	}
}
