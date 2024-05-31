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
    // Start is called before the first frame update
    void Start()
    {
        botSpawnedCount = 0;
        spawnPostionIndex = 0;
        SpawnPlayer();
        StartCoroutine(SpawnBot());
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

    private void SpawnPlayer()
    {
        //spawn player
        GameObject player = Instantiate(playerPrefab);
        currentCharacters.Add(player);
        player.GetComponent<PlayerController>().SetCurrentMap(transform);

        //set postion for player
        Vector3 spawPos = spawnPostions[0].transform.position;
        spawPos.y = 1;
        player.transform.position = spawPos;
        spawnPostionIndex++;

        //set target for camera
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
                    GameObject newBot = Instantiate(botPrefab);

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
