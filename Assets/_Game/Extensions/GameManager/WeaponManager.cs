using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [SerializeField] private List<Weapon> weaponsBasedData;
    [SerializeField] private List<Skin> skinsBasedData;
    public List<Weapon> weaponInfor;
    public List<Skin> skinInfor;
    private string weaponFilePath;
    private string skinFilePath;

    private void Awake()
    {
        weaponFilePath = Application.persistentDataPath + Constant.WEAPON_DATA_FILE_NAME;
        skinFilePath = Application.persistentDataPath + Constant.SKIN_DATA_FILE_NAME;
        LoadFromJson();
    }

    public void SaveToJson()
    {
        //Convert objects list into json string
        string jsonWeapon = JsonUtility.ToJson(new WeaponListWrapper { weapons = weaponInfor }, true);
        string jsonSkin = JsonUtility.ToJson(new SkinListWrapper { skins = skinInfor }, true);
        Debug.Log("Save weapon location:" + weaponFilePath);
        Debug.Log("Save skin location" + skinFilePath);

        //Save string into json file
        File.WriteAllText(weaponFilePath, jsonWeapon);
        File.WriteAllText(skinFilePath, jsonSkin);
        LoadFromJson();

        //Debug
        // foreach (Weapon item in weaponInfor)
        // {
        //     Debug.Log("Weapon Name:" + item.weaponName + " - " + item.weaponStatus);
        // }
    }

    private void LoadFromJson()
    {
        if (File.Exists(weaponFilePath) && File.Exists(skinFilePath))
        {
            //If found file, update "List<Wepon> weaponinfor" and "List<Skin> skinInfor" 
            string jsonWeapon = File.ReadAllText(weaponFilePath);
            string jsonSkin = File.ReadAllText(skinFilePath);

            SkinListWrapper skinListWrapper = JsonUtility.FromJson<SkinListWrapper>(jsonSkin);
            WeaponListWrapper weaponListWrapper = JsonUtility.FromJson<WeaponListWrapper>(jsonWeapon);

            skinInfor = skinListWrapper.skins;
            weaponInfor = weaponListWrapper.weapons;
        }
        else
        {
            //If not found file, create ".json" file from "List<Weapon> basedData" and save it
            weaponInfor = weaponsBasedData;
            skinInfor = skinsBasedData;
            SaveToJson();

            // Debug.Log(weaponFilePath);
            // Debug.Log(skinFilePath);
        }
    }


    [System.Serializable]
    public class WeaponListWrapper
    {
        public List<Weapon> weapons;
    }

    [System.Serializable]
    public class SkinListWrapper
    {
        public List<Skin> skins;
    }
}
