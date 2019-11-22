using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Enum specifying different GameResults.
/// </summary>
public enum GameResult
{
    PLAYERWIN,
    AIWIN,
    TIE
};

/// <summary>
/// A TicTacToeManager class that handles the control functionality in Game.
/// Has methods to handle GameStart, AITurn, PLayerTurn, TurnEnd, GameEnd etc- 
/// The TicTacToeManager class also handles a little of HUD functionality present in the game.
/// </summary>
public class TicTacToeManager : MonoBehaviour 
{
    //Singleton Instance for TicTacToeManager
    private static TicTacToeManager mInstance;
    public static TicTacToeManager pInstance
    {
        get{return mInstance; }
    }

    //TicTacToeAI can be loaded and added as a component to the gameObject.
    private TicTacToeAI mGameAI = null;
    private AudioSource mAudioSource = null;

    public string _GameStartPopupName = null;
    public Text  _CurTurnText;
    public Button _RestartBtn;
    [HideInInspector]
    public bool isGameRunning = false;

    private int curTurn = 0;

    void Awake()
    {
        mInstance = this;
    }

    void Start()
    {
        //Loading a Menu prefab from resources to define the game conditions
        if (!string.IsNullOrEmpty(_GameStartPopupName))
        {
            GameObject gameStart = Resources.Load(_GameStartPopupName) as GameObject;
            if (gameStart)
            {
                GameObject GameStartPopup = GameObject.Instantiate(gameStart) as GameObject;
                GameStartPopup.transform.position = Vector3.zero;
            }
        }
    }

    //OnGameStart is called after the board is instantiated. 
    //The AI script is added as a component and the game begins after the AI Loads. This can be changed to loading from resources.
    public void OnGameStart()
    {
        mGameAI = gameObject.AddComponent<TicTacToeAI>();
        if (mGameAI)
        {
            Debug.Log("Game AI loaded successfully");
            isGameRunning = true;
            _RestartBtn.interactable = true;
            PlayNextTurn();
        }
        else
        {
            Debug.Log("Game AI not ready");
        }
    }

    //PlayNextTurn defines whether the control is given to AI or Player to play the current turn.
    // All Odd Turns go to player
    // All Even turns go to AI
    // This can be manipulated later if needed (Toss functionality can be added)
    public void PlayNextTurn()
    {
        curTurn++;
        if (curTurn % 2 == 0)
            OnAITurn();
        else
            OnPlayerTurn();
    }

    public void OnAITurn()
    {
        _CurTurnText.text = "AI's turn";
        mGameAI.PlayTurn(BoardManager.pInstance.mDifficulty);
    }

    public void OnPlayerTurn()
    {
        _CurTurnText.text = "Player's turn";
    }

    //OnTurnEnd checks for Winning or Tie condition. If nothing persists then the game goes to NextTurn.
    public void OnTurnEnd()
    {
        BoardManager.pInstance.FindEmptyTiles();     
            
        if (BoardManager.pInstance.CheckWinningCondition(BoardManager.pInstance.mTiles))
        { 
            if (IsPlayerTurn())
                OnGameEnd(GameResult.PLAYERWIN);
            else
                OnGameEnd(GameResult.AIWIN); 
        }
        else if (BoardManager.pInstance.mEmptyTiles.Count <= 0)
        {
            OnGameEnd(GameResult.TIE);
        }
        else
        {
            PlayNextTurn();
        }
    }

    //Restarts the scene for game restart. Can be changed if needed.
    public void OnRestart()
    {
        SceneManager.LoadScene("TicTacToe");
    }

    //OnGameEnd stops the gameplay and displays a text result over the board.
    //The "You Win" audio clip is loaded from resources and played if the Player wins the game.
    public void OnGameEnd(GameResult inResult)
    {
        isGameRunning = false;
        Debug.Log("GAME END >>>> " + inResult.ToString());

        if (inResult == GameResult.PLAYERWIN)
        {
            _CurTurnText.text = "You Win";

            AudioClip winClip = (AudioClip)Resources.Load("YouWin");
            if (winClip)
            {
                mAudioSource = gameObject.AddComponent<AudioSource>();
                Debug.Log("Playing Win Clip");
                mAudioSource.PlayOneShot(winClip);
            }                
        }
        else if (inResult == GameResult.AIWIN)
        {
            _CurTurnText.text = "You Lose";
        }
        else
        {
            _CurTurnText.text = "Game Tie";
        }
    }

    public bool IsPlayerTurn()
    {
        return curTurn % 2 == 1 ? true : false; 
    }

    void OnDestroy()
    {
        mInstance = null;
    }
}
