using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private MapController mapController;
    [SerializeField] private MapData currentMap;

    void Awake()
    {
        currentMap = LevelManager.Instance.GetMapData();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

    void Start()
    {
        currentGameState = GameState.Start;
        SpawnMap();
    }

    void Update()
    {
        if (currentGameState == GameState.Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public int GetAliveCharacter()
    {
        return mapController.GetAlived();
    }

    private void SpawnMap()
    {
        if(currentMap != null)
        {
            mapController = Instantiate(currentMap.GetMapPrefab()).gameObject.GetComponent<MapController>();
            Instantiate(currentMap.GetNavMeshSurface());
        }
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
