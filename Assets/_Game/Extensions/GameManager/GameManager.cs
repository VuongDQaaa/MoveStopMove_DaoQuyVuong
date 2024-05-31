using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    Playing,
    Pause
}
public class GameManager : Singleton<GameManager>
{
    public GameState currentGameState;

    void Awake()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    void Start()
    {
        currentGameState = GameState.Start;
    }

    public void SaveGold(int gold)
    {
        if (!PlayerPrefs.HasKey(Constant.PLAYERFREFS_KEY_GOLD))
        {
            PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_GOLD, 0);
            PlayerPrefs.Save();
        }
        else
        {
            int currentGold = PlayerPrefs.GetInt(Constant.PLAYERFREFS_KEY_GOLD);
            currentGold += gold;
            PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_GOLD, currentGold);
            PlayerPrefs.Save();
        }
        Debug.Log("save:" + gold);
    }

    public int GetGold()
    {
        if (!PlayerPrefs.HasKey(Constant.PLAYERFREFS_KEY_GOLD))
        {
            return 0;
        }
        else
        {
            return PlayerPrefs.GetInt(Constant.PLAYERFREFS_KEY_GOLD);
        }
    }
}
