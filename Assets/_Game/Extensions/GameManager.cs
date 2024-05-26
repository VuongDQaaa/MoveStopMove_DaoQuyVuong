using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> currentCharacter;

    public void RemoveCharacter(GameObject character)
    {
        GameObject foudedCharacter = currentCharacter.Find(obj => obj == character);
        if(foudedCharacter != null)
        {
            currentCharacter.Remove(foudedCharacter);
            Destroy(foudedCharacter);
        }
    }
}
