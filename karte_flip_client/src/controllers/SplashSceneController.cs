using Godot;
using System;
using System.Threading.Tasks;

public partial class SplashSceneController : Node2D{
	public async override void _Ready(){
		GetNode<AnimationPlayer>("AnimationPlayer").Play("sm_popup");
		RouteManager routeManager = GetNode<RouteManager>(
			RouteManager.GetSingletonAutoLoad(SingletonAutoLoadEnum.ROUTE_MANAGER)
		);
		await Task.Delay(2500);
		routeManager.MoveToScene(SceneFilenameEnum.LOBBY_SCENE);
	}
}
