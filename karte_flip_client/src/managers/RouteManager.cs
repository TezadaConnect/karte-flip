using System;
using System.Threading.Tasks;
using Godot;

public partial class RouteManager: Node{

    public async void MoveToScene(SceneFilenameEnum filenameEnum){
        string sceneName = Enum.GetName(filenameEnum);
        string loadingScenePath = "res://src/resources/screen_scenes/loading_scene.tscn";
        GetTree().ChangeSceneToFile(loadingScenePath);
        string NextScenePath = "res://src/resources/screen_scenes/" + sceneName.ToLower() + ".tscn";
        PackedScene nextScene = ResourceLoader.Load<PackedScene>(NextScenePath);
        await Task.Delay(1500);
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
}