using UnityEngine;
public enum EquipmentStatus
{
    Locked = 0,
    Unlocked = 1,
    Equiped = 2
}

public enum EquipmentType
{
    Hat = 0,
    Pant = 1,
    Shield = 2,
    FullSet = 3,
    Weapon = 4
}

[CreateAssetMenu(menuName = "Equipment")]
public class Equipment : ScriptableObject
{
    public int id;
    public string equipmentName;
    public EquipmentType equipmentType;
    public EquipmentStatus equipmentStatus;
    public GameObject equipmentModel;
    public GameObject bulletPrefab;
    public Material equipmentMaterial;
    public Sprite equipmentImage;
    public int equipmentPrize;
    public string equipmentAbility;
    public float attackRange;
    public float attackSpeed;
}
