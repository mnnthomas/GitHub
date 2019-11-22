using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// A Menu script that gets the Game settings before game start.
/// </summary>
public class GameStart : MonoBehaviour 
{
    public Dropdown _BoardSize;
    public Dropdown _Difficulty;


    public void OnStart()
    {
        int boardSize = int.Parse(_BoardSize.captionText.text);
        AI boardDifficulty = (AI)System.Enum.Parse(typeof(AI), _Difficulty.captionText.text);

        BoardManager.pInstance.SetBoardValues(boardSize, boardDifficulty);

        Destroy(this.gameObject);
    }
}
