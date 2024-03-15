using System;
using Godot;

class RouteManager{
    private static RouteManager mRouteManagerInstance;

    public static RouteManager GetIntance(){
        if(mRouteManagerInstance == null){
            mRouteManagerInstance = new RouteManager();
        }
        return mRouteManagerInstance;
    }

    public string GetSceneFilePath(SceneFileNameEnum fileNameEnum){
        string sceneName = Enum.GetName(fileNameEnum);
        return "res://src/resources/screen_scenes/" + sceneName.ToLower() + ".tscn";
    }

    public string GetLocalAssetFilePath(LocalAssetFileNameEnum route){
        string assetName = Enum.GetName(route);
        return "res://src/resources/assets/imgs/cards_and_tokens/"+ assetName.ToLower() +".png";
    }
}