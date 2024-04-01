using System;
using System.Threading.Tasks;
using Godot;

public partial class RouteManager: Node{

    public async void MoveToScene(SceneFilenameEnum filenameEnum, string message){
        GetTree().CurrentScene.Free();
        // Changing to loading scene
        string loadingScenePath = "res://src/resources/screen_scenes/loading_scene.tscn";
        PackedScene loadingScenePack = ResourceLoader.Load<PackedScene>(loadingScenePath);
        LoadingSceneController loadingSceneController = loadingScenePack.Instantiate<LoadingSceneController>();
        loadingSceneController.SetLoadingMessage(message);
        GetTree().UnloadCurrentScene();
        GetTree().Root.AddChild(loadingSceneController);
        GetTree().CurrentScene = loadingSceneController;
        // Next Scene
        string sceneName = Enum.GetName(filenameEnum);
        string NextScenePath = "res://src/resources/screen_scenes/" + sceneName.ToLower() + ".tscn";
        PackedScene nextScene = ResourceLoader.Load<PackedScene>(NextScenePath);
        await Task.Delay(2000);
        GetTree().ChangeSceneToPacked(nextScene);
    }

    public static Texture2D GetLocalAssetInTexture2D(LocalAssetFileNameEnum route){
        string assetName = Enum.GetName(route);
        Texture2D texture2D = GD.Load<Texture2D>("res://src/resources/assets/imgs/cards_and_tokens/"+ assetName.ToLower() +".png");
        return texture2D;
    }

    public static AudioStream GetLocalAssetInAudioStream(LocalAssetFileNameEnum route){
        string assetName = Enum.GetName(route);
        AudioStream stream = GD.Load<AudioStream>("res://src/resources/assets/audios/"+ assetName.ToLower() +".mp3");
        return stream;
    }

    public static string GetSingletonAutoLoad(SingletonAutoLoadEnum singletonAutoLoadEnum){
        string singletonName = Enum.GetName(singletonAutoLoadEnum);
        return "/root/" + singletonName;
    }

    public static PackedScene GetDrawables(SceneFilenameEnum filenameEnum){
        string sceneName = Enum.GetName(filenameEnum);
        string packedScenePatch = "res://src/resources/drawables/" + sceneName.ToLower() + ".tscn";
        PackedScene packedScene = ResourceLoader.Load<PackedScene>(packedScenePatch);
        return packedScene;
    }
}