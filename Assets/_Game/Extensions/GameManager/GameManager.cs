using System.Collections;
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
    private int currentGold;
    private int rewardGold;
    private int currentRank;
    private string killerName;

    void Awake()
    {
        OnInit();
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
        currentRank = mapController.GetAlived();
        UpdateReward();
    }
    private void UpdateReward()
    {
        if (currentRank == 1)
        {
            rewardGold = 50;
        }
        else if (currentRank <= 5 && currentRank > 1)
        {
            rewardGold = 20;
        }
        else if (currentRank <= 20 && currentRank > 5)
        {
            rewardGold = 10;
        }
        else
        {
            rewardGold = 1;
        }
    }

    private void OnInit()
    {
        //Update map
        currentMap = LevelManager.Instance.GetMapData();
        //Open main menu UI
        UIManager.Instance.OpenUI<CanvasMainMenu>();
        //Update gold
        if (!PlayerPrefs.HasKey(Constant.PLAYERFREFS_KEY_GOLD))
        {
            PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_GOLD, 0);
            currentGold = 0;
        }
        else
        {
            currentGold = PlayerPrefs.GetInt(Constant.PLAYERFREFS_KEY_GOLD);
        }
        currentGameState = GameState.Start;
    }

    public int GetAliveCharacter()
    {
        return mapController.GetAlived();
    }

    public void SpawnMap()
    {
        //spawn map based on current level
        if (currentMap != null)
        {
            mapController = Instantiate(currentMap.GetMapPrefab()).gameObject.GetComponent<MapController>();
            Instantiate(currentMap.GetNavMeshSurface(), mapController.transform);
        }
    }

    public void ClearMap()
    {
        //clear current map
        if (mapController != null)
        {
            Destroy(mapController.gameObject);
            mapController = null;
        }
    }

    public void UpdateGold(int gold)
    {
        currentGold += gold;
        PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_GOLD, currentGold);
        PlayerPrefs.Save();
    }

    public int GetGold()
    {
        return currentGold;
    }

    public void GetReviveUI()
    {
        StartCoroutine(ShowRevieUI());
    }

    public void RevivePlayer()
    {
        mapController.SpawnPlayer();
    }

    public int GetRank()
    {
        return currentRank;
    }

    public int GetReward()
    {
        return rewardGold;
    }

    public void UpdateKillerName(string name)
    {
        killerName = name;
    }

    public string GetKillerName()
    {
        return killerName;
    }
    
    IEnumerator ShowRevieUI()
    {
        yield return new WaitForSeconds(1.2f);
        UIManager.Instance.OpenUI<CanvasRevive>();
    }
}
