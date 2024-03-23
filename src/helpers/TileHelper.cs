using Godot;

static class TileHelper {
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

    public static Vector2I GetAtlasPositionFromGameTurnManager(){
        return GameTurnManager.GetInstance().GetAtlasPositionBaseOnPlayerColor();
    }

    public static void AddAtlasFromGameTurnManagerToTilemap(Vector2I position, GridGroundTilemap tilemap){
        tilemap.SetCell(
            tilemap.TOKEN_PLACEMENT_LAYER, 
            position, 
            tilemap.ADD_TILE_ACTION, 
            GetAtlasPositionFromGameTurnManager());
    }
}