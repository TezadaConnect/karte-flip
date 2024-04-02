using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ScoringManager : Node2D {
    private int _blackScore;
    private int _whiteScore;
    private Label _whiteScoreLabel;
    private Label _blackScoreLabel;

    private void CalculateScore(GridGroundTilemap tilemap){
        List<Vector2I> arrayOfTilePositionAtTokenLayer = tilemap.GetUsedCells(
            tilemap.TOKEN_PLACEMENT_LAYER
        ).ToList();

        int blackCount = 0;
        int whiteCount = 0;
        
        foreach (Vector2I element in arrayOfTilePositionAtTokenLayer){
            Vector2I tileAtlas = tilemap.GetCellAtlasCoords(
                tilemap.TOKEN_PLACEMENT_LAYER, element
            );

            if(TileHelper.ATLAS_COORD_BLACK == tileAtlas){
                blackCount++;
            } else {
                whiteCount++;
            }
        }

        _blackScore = blackCount;
        _whiteScore = whiteCount;
    }


    public void ResetScore(){
        _blackScore = 0;
        _whiteScore = 0;
    }

    public void DisplayScore(GridGroundTilemap gridGroundTilemap){
        _blackScoreLabel ??= GetNode<Label>("/root/MainScene/HUDTextureRect/BlackScoreLabel");
        _whiteScoreLabel ??= GetNode<Label>("/root/MainScene/HUDTextureRect/WhiteScoreLabel");
        CalculateScore(gridGroundTilemap);
        _blackScoreLabel.Text = _blackScore + "x";
        _whiteScoreLabel.Text = _whiteScore + "x";
	}
}