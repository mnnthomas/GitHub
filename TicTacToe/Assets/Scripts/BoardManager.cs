using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enum specifying different AI difficulties
/// </summary>
public enum AI
{
    EASY,
    HARD
};

/// <summary>
/// A Board manager which can instantiate and set board of given size for TicTacToe game with UI buttons
/// Also allows to set each tile data in the board and checks for winning conditions in board 
/// </summary>
public class BoardManager : MonoBehaviour 
{
    //Singleton instance for BoardManager
    private static BoardManager mInstance;
    public static BoardManager pInstance
    {
        get{return mInstance; }
    }

    public int _TileOffset;
    public int _TileSize;
    public GameObject _ParentPanel;
    public TileData _TileDataObj;

    public TileData[, ] mTiles;
    public List<TileData> mEmptyTiles = new List<TileData>();

    [HideInInspector]
    public int mBoardSize;
    [HideInInspector]
    public AI mDifficulty;


    void Awake()
    {
        mInstance = this;
    }

    void Start()
    {
    }

    //Can be called to define board size and AI difficulty. 
    public void SetBoardValues(int inSize = 3, AI inDifficulty = AI.EASY)
    {
        mBoardSize = inSize;
        mDifficulty = inDifficulty;
        InstantiateTiles();
    }

    //Instantiates the board with empty tiles
    void InstantiateTiles()
    {
        mTiles = new TileData[mBoardSize, mBoardSize];
        
        for (int i = 0; i < mBoardSize; i++)
        {
            for (int j = 0; j < mBoardSize; j++)
            {
                mTiles[i, j] = (TileData)GameObject.Instantiate(_TileDataObj);
                mTiles[i, j].gameObject.name = i.ToString() + j.ToString();
                mTiles[i, j].transform.SetParent(_ParentPanel.transform);
                mTiles[i, j].transform.position = new Vector3(j * _TileSize + j * _TileOffset, -i * _TileSize - i * _TileOffset, 0f);
            }
        }
        CenterPanel();
        TicTacToeManager.pInstance.OnGameStart();
    }

    void CenterPanel()
    {
        Vector3 temp = mTiles[mBoardSize/2, mBoardSize/2].transform.position;
        _ParentPanel.transform.position = new Vector3(-temp.x, -temp.y, 0f);
    }

    //Can be called to set tiletype for each tile in board
    public void SetTileData(TileData inTile, TileType inType)
    {
        inTile.UpdateTileType(inType);
    }

    //Finds remaining empty tiles in board
    public List<TileData> FindEmptyTiles()
    {
        mEmptyTiles.Clear();
        for (int i = 0; i < mBoardSize; i++)
        {
            for (int j = 0; j < mBoardSize; j++)
            {
                if (mTiles[i, j].pType == TileType.EMPTY)
                    mEmptyTiles.Add(mTiles[i, j]);
            }
        }

        return mEmptyTiles;
    }

    //Checks for Winning condition in board
    public bool CheckWinningCondition(TileData[, ] inTileData)
    {
        TileType temptype = TileType.EMPTY;

        int matchCount = 0;
        //Check rows for match
        for (int i = 0; i < mBoardSize; i++)
        {
            temptype = inTileData[i, 0].pType;
            matchCount = 0;

            if (temptype == TileType.EMPTY)
                continue;
            
            for (int j = 1; j < mBoardSize; j++)
            {
                if (temptype == inTileData[i, j].pType)
                {
                    matchCount++;
                    if(matchCount == (mBoardSize - 1))
                    {
                      //  Debug.Log("Row "+i+" has match");
                        return true;
                    }
                }
            }
        }

        //Check column for match
        for (int i = 0; i < mBoardSize; i++)
        {
            temptype = inTileData[0, i].pType;
            matchCount = 0;

            if (temptype == TileType.EMPTY)
                continue;

            for (int j = 1; j < mBoardSize; j++)
            {
                if (temptype == inTileData[j, i].pType)
                {
                    matchCount++;
                    if(matchCount == (mBoardSize - 1))
                    {
                      //  Debug.Log("Column "+i+" has match");
                        return true;
                    }
                }
            }
        }

        //Check Diagonals for match
        matchCount = 0;
        for (int i = 1, j = 1; i < mBoardSize; i++, j++)
        {
            temptype = inTileData[0, 0].pType;

            if (temptype == TileType.EMPTY)
                continue;
             
            if (temptype == inTileData[i, j].pType)
            {
                matchCount++;
                if(matchCount == (mBoardSize - 1))
                {
                   // Debug.Log("Diagonals match");
                    return true;
                }
            }
        }

        matchCount = 0;
        for (int i = mBoardSize-2, j = 1; j < mBoardSize; i--, j++)
        {
            temptype = inTileData[mBoardSize-1, 0].pType;

            if (temptype == TileType.EMPTY)
                continue;

            if (temptype == inTileData[i, j].pType)
            {
                matchCount++;
                if(matchCount == (mBoardSize - 1))
                {
                  //  Debug.Log("Diagonals match");
                    return true;
                }
            }
        }       
   
        return false;
    }


    void OnDestroy()
    {
        mInstance = null;
    }

}
