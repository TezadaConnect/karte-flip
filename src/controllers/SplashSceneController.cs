using Godot;
using System;
using System.Threading.Tasks;

public partial class SplashSceneController : Node2D{
	public async override void _Ready(){
		GetNode<AnimationPlayer>("AnimationPlayer").Play("sm_popup");
		await Task.Delay(2500);
		RouteManager.GetIntance().MoveToScene(SceneFileNameEnum.LOBBY_SCENE, GetTree());
	}
}
