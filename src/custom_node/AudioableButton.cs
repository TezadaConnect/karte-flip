using Godot;

public partial class AudioableButton : Button{
	private AudioStreamPlayer mButtonAudioPlayer;
	public override void _Ready(){
        mButtonAudioPlayer = new AudioStreamPlayer{
            Stream = RouteManager.GetIntance().GetLocalAssetInAudioStream(LocalAssetFileNameEnum.BUTTON_TAP_AUDIO)
        };

        AddChild(mButtonAudioPlayer);
	}

	public void PlayButtonTapAudio(){
		mButtonAudioPlayer.Play();
	}

	public void Destroy(){
		mButtonAudioPlayer.QueueFree();
	}
}
