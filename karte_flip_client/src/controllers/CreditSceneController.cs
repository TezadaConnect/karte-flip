using Godot;
using System;
using System.Threading.Tasks;

public partial class CreditSceneController : Node2D{
	RouteManager _routeManager;

	public override void _Ready(){
		GetNode<AudioableButton>("PressToLobby").Connect(
			"pressed", 
			new Callable(this, "OnPressedGoBackToLobby")
		);
		GetNode<AudioStreamPlayer>("HoorayAudioStreamPlayer").Play();
		_routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
	}

	public async void OnPressedGoBackToLobby(){
		await Task.Delay(500);
		_routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE);
	}

}
