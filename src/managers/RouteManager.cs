using System;
using Godot;

class RouteManager{
    private static RouteManager mRouteManagerInstance;

    public static RouteManager GetIntance(){
        mRouteManagerInstance ??= new RouteManager();
        return mRouteManagerInstance;
    }

    public void MoveToScene(SceneFileNameEnum fileNameEnum, SceneTree getTree){
        string sceneName = Enum.GetName(fileNameEnum);
        getTree.ChangeSceneToFile("res://src/resources/screen_scenes/" + sceneName.ToLower() + ".tscn");
    }

    public Texture2D GetLocalAssetInTexture2D(LocalAssetFileNameEnum route){
        string assetName = Enum.GetName(route);
        Texture2D texture2D = GD.Load<Texture2D>("res://src/resources/assets/imgs/cards_and_tokens/"+ assetName.ToLower() +".png");
        return texture2D;
    }

    public AudioStream GetLocalAssetInAudioStream(LocalAssetFileNameEnum route){
        string assetName = Enum.GetName(route);
        AudioStream stream = GD.Load<AudioStream>("res://src/resources/assets/audios/"+ assetName.ToLower() +".mp3");
        return stream;
    }

    public string GetSingletonAutoLoad(SingletonAutoLoadEnum singletonAutoLoadEnum){
        string singletonName = Enum.GetName(singletonAutoLoadEnum);
        return "/root/" + singletonName;
    }
}