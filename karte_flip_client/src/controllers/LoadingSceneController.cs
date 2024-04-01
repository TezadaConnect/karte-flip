using System;
using Godot;

public partial class LoadingSceneController : Node {

    public override void _Ready(){
        TextureRect loadingAssetTexture = GetNode<TextureRect>("LoadingAssetTextureRect");
        LocalAssetFileNameEnum randomAsset = (LocalAssetFileNameEnum)new Random().Next(5);
        Texture2D item = RouteManager.GetLocalAssetInTexture2D(randomAsset);
        loadingAssetTexture.Texture = item;
        AnimationPlayer animateTile = GetNode<AnimationPlayer>("AnimateLoadingAsset");
        animateTile.Play("rotate_asset");
    }
}