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

    public string GetSceneFilePath(RouteEnum route){
        string sceneName = Enum.GetName(route);
        return "res://src/resources/screen_scenes/" + sceneName.ToLower() + ".tscn";
    }
}