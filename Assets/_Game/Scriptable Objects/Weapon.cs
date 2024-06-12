using UnityEngine;
public enum WeaponStatus
{
    Locked = 0,
    Unlocked = 1,
    Equiped = 2
}

public enum SkinType
{
    Hat = 0,
    Pant = 1,
    Shield = 2,
    FullSet = 3
}

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string id;
    public string weaponName;
    public WeaponStatus weaponStatus;
    public Sprite weaponImage;
    public int weaponPrize;
    public string weaponAbility;
    public GameObject weapmonPrefab;
    public GameObject bulletPrefab;
    public float attackRange;
    public float attackSpeed;
}

[CreateAssetMenu(menuName = "Skin")]
public class Skin : ScriptableObject
{
    public int id;
    public string skinName;
    public SkinType skinType;
    public WeaponStatus weaponStatus;
    public int skinPrize;
    public Sprite skinImnage;
    public string skinAbility;
    public GameObject skinModel;
    public Material skinMaterial;
    public float attackRange;
    public float attackSpeed;
}
