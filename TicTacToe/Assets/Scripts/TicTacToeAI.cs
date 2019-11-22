using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Tic tac toe AI class which is seperated and uses Board and Game Manager's pInstance to play and send EndTurn notification.
/// </summary>
public class TicTacToeAI : MonoBehaviour 
{
    /// <summary>
    /// Allows AI to play the next turn
    /// </summary>
    /// <param name="inDifficulty">defines the AI's difficulty mode</param>
    /// 

    private List<TileData> playableTiles = new List<TileData>();

    public void PlayTurn(AI inDifficulty)
    {
        playableTiles = BoardManager.pInstance.FindEmptyTiles();

        //The AI randomly plays a empty tile on Easy difficulty
        if (inDifficulty == AI.EASY)
        {
            PlayEasyAI();
        }
        else if (inDifficulty == AI.HARD)
        {
            PlayHardAI();
        }
      
        TicTacToeManager.pInstance.OnTurnEnd();
    }


    void PlayEasyAI()
    {
        for(int i = 0 ; i < playableTiles.Count; i++)
        {
            int a = int.Parse(playableTiles[i].pTileID) / 10;
            int b = int.Parse(playableTiles[i].pTileID) % 10;

            BoardManager.pInstance.mTiles[a, b].pType = TileType.O;
            if(BoardManager.pInstance.CheckWinningCondition(BoardManager.pInstance.mTiles))
            {
                playableTiles.RemoveAt(i);
            }
            BoardManager.pInstance.mTiles[a, b].pType = TileType.EMPTY;         
        }

        int rand = Random.Range(0, playableTiles.Count);
        BoardManager.pInstance.SetTileData(playableTiles[rand],TileType.O);
    }

    void PlayHardAI()
    {
        List<TileData> playableTiles = BoardManager.pInstance.FindEmptyTiles();

        //Check for a winning condition
        for(int i = 0 ; i < playableTiles.Count; i++)
        {
            int a = int.Parse(playableTiles[i].pTileID) / 10;
            int b = int.Parse(playableTiles[i].pTileID) % 10;

            BoardManager.pInstance.mTiles[a, b].pType = TileType.O;

            if(BoardManager.pInstance.CheckWinningCondition(BoardManager.pInstance.mTiles))
            {
                BoardManager.pInstance.SetTileData(BoardManager.pInstance.mTiles[a, b], TileType.O);
                Debug.Log("Hard AI played a winning move !");
                return;
            }
            else
            {
                BoardManager.pInstance.mTiles[a, b].pType = TileType.EMPTY;
            }
        }

        //Check to block player's winning condition
        for(int i = 0 ; i < playableTiles.Count; i++)
        {
            int a = int.Parse(playableTiles[i].pTileID) / 10;
            int b = int.Parse(playableTiles[i].pTileID) % 10;

            BoardManager.pInstance.mTiles[a, b].pType = TileType.X;

            if(BoardManager.pInstance.CheckWinningCondition(BoardManager.pInstance.mTiles))
            {
                BoardManager.pInstance.SetTileData(BoardManager.pInstance.mTiles[a, b], TileType.O);
                Debug.Log("Hard AI blocked a winning move for player !");
                return;
            }
            else
            {
                BoardManager.pInstance.mTiles[a, b].pType = TileType.EMPTY;
            }
        }

        //Try to take opposite corner
        if (BoardManager.pInstance.mTiles[0, 0].pType == TileType.X && BoardManager.pInstance.mTiles[BoardManager.pInstance.mBoardSize - 1, BoardManager.pInstance.mBoardSize - 1].pType == TileType.EMPTY)
        {
            BoardManager.pInstance.mTiles[BoardManager.pInstance.mBoardSize - 1, BoardManager.pInstance.mBoardSize - 1].UpdateTileType(TileType.O);
            return;
        }
        else if (BoardManager.pInstance.mTiles[BoardManager.pInstance.mBoardSize - 1, BoardManager.pInstance.mBoardSize - 1].pType == TileType.X && BoardManager.pInstance.mTiles[0, 0].pType == TileType.EMPTY)
        {
            BoardManager.pInstance.mTiles[0, 0].UpdateTileType(TileType.O);
            return;
        }
        else if (BoardManager.pInstance.mTiles[0 , BoardManager.pInstance.mBoardSize - 1].pType == TileType.X && BoardManager.pInstance.mTiles[BoardManager.pInstance.mBoardSize - 1, 0].pType == TileType.EMPTY)
        {
            BoardManager.pInstance.mTiles[BoardManager.pInstance.mBoardSize - 1, 0].UpdateTileType(TileType.O);
            return;
        }
        else if (BoardManager.pInstance.mTiles[ BoardManager.pInstance.mBoardSize - 1, 0].pType == TileType.X && BoardManager.pInstance.mTiles[0,  BoardManager.pInstance.mBoardSize - 1].pType == TileType.EMPTY)
        {
            BoardManager.pInstance.mTiles[0, BoardManager.pInstance.mBoardSize - 1].UpdateTileType(TileType.O);
            return;
        }

        //Try to take the center tile
        if (BoardManager.pInstance.mTiles[(BoardManager.pInstance.mBoardSize / 2), (BoardManager.pInstance.mBoardSize / 2)].pType == TileType.EMPTY)
        {
            BoardManager.pInstance.mTiles[(BoardManager.pInstance.mBoardSize / 2), (BoardManager.pInstance.mBoardSize / 2)].UpdateTileType(TileType.O);
            return;
        }
        

        //Play a random playable tile
        int rand = Random.Range(0, playableTiles.Count);
        BoardManager.pInstance.SetTileData(playableTiles[rand],TileType.O);

    }


}
