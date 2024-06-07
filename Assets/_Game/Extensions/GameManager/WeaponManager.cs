using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField]private List<Weapon> basedData;
    public List<Weapon> weaponInfor;
    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + Constant.WEAPON_DATA_FILE_NAME;
        LoadFromJson();
    }

    public void SaveToJson()
    {
        //Convert objects list into json string
        string json = JsonUtility.ToJson(new WeaponListWrapper{weapons = weaponInfor}, true);
        Debug.Log("Save location:" + filePath);

        //Save string into json file
        File.WriteAllText(filePath, json);
        LoadFromJson();

        //Debug
        foreach (Weapon item in weaponInfor)
        {
            Debug.Log("Weapon Name:" + item.weaponName + " - " + item.weaponStatus);
        }
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
            SaveToJson();
            Debug.Log(filePath);
        }
    }


    [System.Serializable]
    public class WeaponListWrapper
    {
        public List<Weapon> weapons;
    }
}
