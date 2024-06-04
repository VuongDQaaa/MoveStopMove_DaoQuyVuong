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
            UpdateReward(mapController.GetAlived());
        }
    }

    //Init
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

    //Character methods
    public int GetAliveCharacter()
    {
        return mapController.GetAlived();
    }
    public void RevivePlayer()
    {
        currentGameState = GameState.Playing;
        mapController.RevivePlayer();
    }

    //Map methods
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

    //Reward method
    public void UpdateGold(int gold)
    {
        //Update gold after complete game
        currentGold += gold;
        PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_GOLD, currentGold);
        PlayerPrefs.Save();
    }
    private void UpdateReward(int currentRank)
    {
        if (currentRank == 1)
        {
            rewardGold = 100;
        }
        else if (currentRank <= 5 && currentRank > 1)
        {
            rewardGold = 70;
        }
        else if (currentRank <= 20 && currentRank > 5)
        {
            rewardGold = 30;
        }
        else if(currentRank <= 40 && currentRank > 20)
        {
            rewardGold = 20;
        }
        else
        {
            rewardGold = 0;
        }
    }

    //UI method
    public int GetCurrentGoldInfor()
    {
        return currentGold;
    }
    public void GetReviveUI()
    {
        StartCoroutine(ShowRevieUI());
    }
    public int GetRankInfor()
    {
        //return current rank when player die
        return mapController.GetAlived();
    }
    public int GetRewardInfor()
    {
        //Update reward in game
        UpdateReward(mapController.GetAlived());
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
