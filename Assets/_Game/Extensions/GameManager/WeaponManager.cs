using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField]private List<Weapon> basedData;
    public List<Weapon> weaponInfor;
    private string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + Constant.WEAPON_DATA_FILE_NAME;
        LoadFromJson();
    }

    private void SaveToJson(List<Weapon> data)
    {
        //Convert objects list into json string
        string json = JsonUtility.ToJson(new WeaponListWrapper{weapons = data}, true);
        Debug.Log(json);
        Debug.Log("Save location:" + filePath);

        //Save string into json file
        File.WriteAllText(filePath, json);
    }

    private void LoadFromJson()
    {
        if(File.Exists(filePath))
        {
            //If found file, update "List<Wepon> weaponinfor" 
            string json = File.ReadAllText(filePath);
            WeaponListWrapper weaponListWrapper = JsonUtility.FromJson<WeaponListWrapper>(json);
            weaponInfor = weaponListWrapper.weapons;
        }
        else
        {
            //If not found file, create ".json" file from "List<Weapon> basedData" and save it
            weaponInfor = basedData;
            SaveToJson(weaponInfor);
            Debug.Log(filePath);
        }
    }


    [System.Serializable]
    public class WeaponListWrapper
    {
        public List<Weapon> weapons;
    }
}
