using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// TileType defines different ownerships of a Tile
/// X - Player Tile
/// O - AI Tile
/// EMPTY - Empty Tile
/// </summary>
public enum TileType
{
    X,
    O,
    EMPTY
};

/// <summary>
/// A Tile Data class which defines each tile's Type, pID and DisplayText in the board  
/// </summary>
public class TileData : MonoBehaviour 
{
    public Text _TileText;
    public string pTileID;

    private TileType mType = TileType.EMPTY;
    public  TileType pType
    {
        get{ return mType; }
        set{ UpdateTileType(value); }
    }

    public void Start()
    {
        pTileID = gameObject.name;
    }


    public void UpdateTileType(TileType inType)
    {
        mType = inType;
        if (inType == TileType.O)
        {
            _TileText.text = "O";
        }
        else if (inType == TileType.X)
        {
            _TileText.text = "X";
        }
        else
        {
            _TileText.text = "";
        }
    }

    //OnClick functionality for Player clicks on Tile
    public void OnClickTile()
    {
        if (TicTacToeManager.pInstance.IsPlayerTurn() && mType == TileType.EMPTY && TicTacToeManager.pInstance.isGameRunning)
        {
            pType = TileType.X;
            TicTacToeManager.pInstance.OnTurnEnd();
        }
    }
}
