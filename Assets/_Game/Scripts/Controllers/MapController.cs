using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject botPrefab;
    [SerializeField] GameObject playerPrefab;
    [Header("Map atributes")]
    public List<GameObject> currentCharacters;
    [SerializeField] private List<Transform> spawnPostions;
    [SerializeField] private int botLimit;
    [SerializeField] private int botSpawnedCount;
    [SerializeField] private int Alived;
    private int spawnPostionIndex;
    private bool showWinUI;
    // Start is called before the first frame update
    void Awake()
    {
        showWinUI = false;
        botSpawnedCount = 0;
        spawnPostionIndex = 0;
        Alived = botLimit + 1;
        SpawnPlayer();
        StartCoroutine(SpawnBot());
    }

    void Update()
    {
        ShowWinUI();
    }

    public void RemoveCharacter(GameObject character)
    {
        GameObject foudedCharacter = currentCharacters.Find(obj => obj == character);
        if (foudedCharacter != null)
        {
            currentCharacters.Remove(foudedCharacter);
            Alived--;
            Destroy(foudedCharacter);
        }
    }

    public int GetAlived()
    {
        return Alived;
    }

    private bool IsVictory()
    {
        //Check Win condition (Only player alive)
        GameObject foundedPlayer = currentCharacters.Find(player => player.gameObject.tag == Constant.TAG_PLAYER);
        if(Alived == 1 && foundedPlayer != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowWinUI()
    {
        //Open win UI when player win
        if(IsVictory() && showWinUI == false && GameManager.Instance.currentGameState == GameState.Playing)
        {
            UIManager.Instance.OpenUI<CanvasWin>();
            showWinUI = true;
        }
    }
    
    private void SpawnPlayer()
    {
        //spawn player
        GameObject player = Instantiate(playerPrefab, transform);
        currentCharacters.Add(player);
        player.GetComponent<PlayerController>().SetCurrentMap(transform);
        GameManager.Instance.playerController = player.GetComponent<PlayerController>();

        //set postion for player
        Vector3 spawPos = spawnPostions[0].transform.position;
        spawPos.y = 1;
        player.transform.position = spawPos;
        spawnPostionIndex++;

        //set target for camera
        CameraController.Instance.SetCamTarget(player);
    }

    public void RevivePlayer()
    {
        //spawn player
        GameObject player = Instantiate(playerPrefab, transform);
        currentCharacters.Add(player);
        player.GetComponent<PlayerController>().SetCurrentMap(transform);
        GameManager.Instance.playerController = player.GetComponent<PlayerController>();
        Alived++;

        //set postion for player
        int randomIndex = Random.Range(0, spawnPostions.Count - 1);
        Vector3 spawPos = spawnPostions[randomIndex].transform.position;
        spawPos.y = 1;
        player.transform.position = spawPos;

        //set target for camera
        CameraController.Instance.Reset();
        CameraController.Instance.SetCamTarget(player);
    }

    IEnumerator SpawnBot()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            if (currentCharacters.Count < 10 && botSpawnedCount < botLimit)
            {
                if (spawnPostionIndex < spawnPostions.Count)
                {
                    //Create bot from prefab
                    GameObject newBot = Instantiate(botPrefab, transform);

                    //Add bot into list
                    currentCharacters.Add(newBot);
                    botSpawnedCount++;
                    newBot.GetComponent<BotController>().SetCurrentMap(transform);

                    //Set postion for bot
                    Vector3 spawnPos = spawnPostions[spawnPostionIndex].transform.position;
                    spawnPos.y = 1;
                    newBot.transform.position = spawnPos;
                    spawnPostionIndex++;
                }
                else
                {
                    spawnPostionIndex = 0;
                }
            }
        }
        StartCoroutine(SpawnBot());
    }

}
