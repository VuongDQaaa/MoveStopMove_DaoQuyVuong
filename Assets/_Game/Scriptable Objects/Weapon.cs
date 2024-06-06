using UnityEngine;
using Unity.UI;
public enum WeaponStatus
{
    Locked = 0,
    Unlocked = 1,
    Equiped = 2
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
