using Godot;
using System;
using System.Threading.Tasks;

public partial class CreditSceneController : Node2D{

	public override void _Ready(){
		GetNode<AudioableButton>("PressToLobby").Connect(
			"pressed", 
			new Callable(this, "OnPressedGoBackToLobby")
		);
		GetNode<AudioStreamPlayer>("HoorayAudioStreamPlayer").Play();
	}

	public async void OnPressedGoBackToLobby(){
		await Task.Delay(500);
		RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}

}
