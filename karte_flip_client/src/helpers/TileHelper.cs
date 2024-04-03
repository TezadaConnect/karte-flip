using System;
using System.Collections.Generic;
using Godot;
using KarteFlipClient;

static class TileHelper {
    public static readonly Vector2I ATLAS_COORD_WHITE = new(1, 0);
    public static readonly Vector2I ATLAS_COORD_BLACK = new(2, 0);
    public static Vector2I GetNextTilePositionByDirectection(Vector2I currentPosition, DirectionEnum directionEnum){
         if(directionEnum == DirectionEnum.RIGHT){
            return new Vector2I(currentPosition.X + 1, currentPosition.Y);
        }

        if(directionEnum == DirectionEnum.LEFT){
            return new Vector2I(currentPosition.X - 1, currentPosition.Y);
        }

        if(directionEnum == DirectionEnum.TOP){
            return new Vector2I(currentPosition.X, currentPosition.Y - 1);
        }

        return new Vector2I(currentPosition.X, currentPosition.Y + 1);
    }

    public static void AddAtlasToken(
        Vector2I position, 
        GridGroundTilemap tilemap, 
        TokenColorEnum tokenColorEnum
    ){
        tilemap.SetCell(
            tilemap.TOKEN_PLACEMENT_LAYER, 
            position, 
            tilemap.ADD_TILE_ACTION, 
            AtlasLocation(tokenColorEnum)
        );
    }

    public static Vector2I AtlasLocation(TokenColorEnum tokenColorEnum){
        if(tokenColorEnum == TokenColorEnum.LIGHT_TOKEN){
            return ATLAS_COORD_WHITE;
        }
        return ATLAS_COORD_BLACK;
    }

    public static CardModel GetRandomCard(){
        CardCollection cardCollection = new();
        List<CardModel> cards = new();
        for (int i = 0; i < cardCollection.GetCardListOfNames().Length; i++){
            cards.Add(cardCollection.GetCardModelByIndex(i));
        }
        return cards[new Random().Next(0, 5)];
    }
}