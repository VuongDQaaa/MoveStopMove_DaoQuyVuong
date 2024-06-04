using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private MapData currentMap;
    [SerializeField] private int currentLevel;

    void Awake()
    {
        if (!PlayerPrefs.HasKey(Constant.PLAYERFREFS_KEY_LEVEL))
        {
            PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_LEVEL, 1);
            PlayerPrefs.Save();
            currentLevel = 1;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt(Constant.PLAYERFREFS_KEY_LEVEL);
        }
        UpdateMapByLevel();
    }

    private void UpdateMapByLevel()
    {
        string path = $"Prefabs/MapData/ScriptableMap/MapLevel{currentLevel}";
        MapData foundedMap = Resources.Load<MapData>(path);
        if (foundedMap != null)
        {
            currentMap = foundedMap;
        }
        else
        {
            Debug.Log("Not find map");
        }
    }

    public MapData GetMapData()
    {
        return currentMap;
    }

    public void UpdateMapLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt(Constant.PLAYERFREFS_KEY_LEVEL, currentLevel);
        PlayerPrefs.Save();
        UpdateMapByLevel();
    }

    public int GetMapLevel()
    {
        return currentLevel;
    }
}
