using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] private List<Equipment> equipmentBasedData;
    public List<Equipment> equipmentInfor;
    private string userFilePath;

    private void Awake()
    {
        userFilePath = Application.persistentDataPath + Constant.USER_DATA_FILE;
        LoadFromJson();
    }

    public void SaveToJson()
    {
        Debug.Log("Save to json");
        UserData userData = new UserData();

        //Update List<int> weapons from user data
        foreach (Equipment equipment in equipmentInfor)
        {
            userData.equipments.Add((int)equipment.equipmentStatus);
        }

        //Save data into json file
        if (userData.equipments.Count > 0)
        {
            string json = JsonUtility.ToJson(userData);
            File.WriteAllText(userFilePath, json);
        }
        else
        {
            Debug.Log("Save Json Fail");
        }
    }

    private void LoadFromJson()
    {
        Debug.Log("Load from json");
        equipmentInfor = equipmentBasedData;

        //Debug
        // foreach (Equipment equipment in equipmentInfor)
        // {
        //     Debug.Log("Equipment: " + equipment.name);
        // }

        if (File.Exists(userFilePath))
        {
            string json = File.ReadAllText(userFilePath);
            UserData userData = JsonUtility.FromJson<UserData>(json);

            //update skin infor follow user data
            foreach (var equipment in userData.equipments)
            {
                equipmentInfor[userData.equipments.IndexOf(equipment)].equipmentStatus = (EquipmentStatus)equipment;
            }
        }
        else
        {
            //create data by basedSkin/Weapon data in first time
            SaveToJson();
        }
    }
}

public class UserData
{
    //index in list equal weapon/skin id, data inside equal weapon/skin status
    public List<int> equipments;

    public UserData()
    {
        equipments = new List<int>();
    }
}