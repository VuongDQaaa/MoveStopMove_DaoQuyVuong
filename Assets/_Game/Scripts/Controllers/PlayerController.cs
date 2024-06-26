using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject aiming;
    [SerializeField] private GameObject zonePrefabs;
    private GameObject transparentZone;
    [SerializeField] private Material playerColor;
    private Vector3 currentRotation;
    private Vector3 currentScale;
    private float fixedScale;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
        //equip weapon first spawn
        EquipBeforeStart();
    }

    // Update is called once per frame
    void Update()
    {
        //Update cam when player upsize
        if (transform.localScale != currentScale)
        {
            CameraController.Instance.ChangeOffSet();
            currentScale = transform.localScale;
        }

        if (isDeath == false)
        {
            if (JoyStickController.direction != Vector3.zero)
            {
                currentRotation = JoyStickController.direction;
            }
            DetectTarget();
            Attack();
            MoveCharacter();
            ShowAiming();
            TransparentZoneControl();
        }
    }

    void OnDestroy()
    {
        Destroy(transparentZone);
    }

    private void EquipBeforeStart()
    {
        //hat
        Equipment hat = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(hat => hat.equipmentType == EquipmentType.Hat
                                                                        && hat.equipmentStatus == EquipmentStatus.Equiped);
        if (hat != null)
        {
            EquipSkin(hat);
        }
        //pant
        Equipment pant = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(pant => pant.equipmentType == EquipmentType.Pant
                                                                        && pant.equipmentStatus == EquipmentStatus.Equiped);
        if (pant != null)
        {
            EquipSkin(pant);
        }
        //hat
        Equipment shield = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(shield => shield.equipmentType == EquipmentType.Shield
                                                                        && shield.equipmentStatus == EquipmentStatus.Equiped);
        if (shield != null)
        {
            EquipSkin(shield);
        }
        //weapon
        Equipment foundedWeapon = EquipmentManager.Instance.equipmentInfor.FirstOrDefault(weapon => weapon.equipmentStatus == EquipmentStatus.Equiped);
        if (foundedWeapon != null)
        {
            EquipWeapon(foundedWeapon);
        }
    }

    private void OnInit()
    {
        //Update infor
        characterName = "You";
        currentPoint = 0;
        bodyColor.material = playerColor;
        pantColor.material = playerColor;
        currentScale = transform.localScale;

        //update player postion
        currentRotation = transform.forward;
        ChangeAnim(AnimationState.idle);
        aiming.SetActive(false);

        //spawn transparent zone
        GameObject spawnedTransparentZone = Instantiate(zonePrefabs, currentMap.transform);
        transparentZone = spawnedTransparentZone;
    }

    private void TransparentZoneControl()
    {
        //control position
        Vector3 newPos = transform.position;
        newPos.y = 0.00009f;
        transparentZone.transform.position = newPos;

        //control scale
        fixedScale = attackRange / Constant.SCALE_TRANSPARENT;
        Vector3 newScale = new Vector3(fixedScale, fixedScale, fixedScale);
        transparentZone.transform.localScale = newScale;
    }

    private void ShowAiming()
    {
        if (currentTarget != null)
        {
            Vector3 aimingPosition = currentTarget.transform.position;
            aimingPosition.y = 0.1f;
            aiming.SetActive(true);
            aiming.transform.position = aimingPosition;
            aiming.transform.localScale = currentTarget.transform.localScale;
        }
        else
        {
            aiming.SetActive(false);
        }
    }

    private void MoveCharacter()
    {
        if (JoyStickController.direction != Vector3.zero)
        {
            ChangeAnim(AnimationState.run);
            isMoving = true;
            isAttack = false;
            if (JoyStickController.direction != Vector3.zero)
            {
                Vector3 newPostion = transform.position + JoyStickController.direction * speed * Time.deltaTime;
                transform.position = CheckGrounded(newPostion);
            }
        }
        if (JoyStickController.direction == Vector3.zero)
        {
            isMoving = false;
            if (isAttack == false)
            {
                ChangeAnim(AnimationState.idle);
            }
        }

        if (isAttack == false)
        {
            //rotate player
            transform.rotation = Quaternion.LookRotation(currentRotation);
        }
    }

    public void ChangeWeapon(Equipment weapon)
    {
        ResetWeapon();
        EquipWeapon(weapon);
    }

    public void ChangeSkin(Equipment Equipment)
    {
        //reset player status before equip Equipment
        if (Equipment.equipmentType == EquipmentType.Hat && currentHat != null)
        {
            foreach (Transform child in hat)
            {
                Destroy(child.gameObject);
            }
            attackRange /= currentHat.attackRange;
            attackSpeed /= currentHat.attackSpeed;
        }
        else if (Equipment.equipmentType == EquipmentType.Pant && currentPant != null)
        {
            attackRange /= currentPant.attackRange;
            attackSpeed /= currentPant.attackSpeed;
        }
        else if (Equipment.equipmentType == EquipmentType.Shield && currentShield != null)
        {
            foreach (Transform child in shield)
            {
                Destroy(child.gameObject);
            }
            attackRange /= currentShield.attackRange;
            attackSpeed /= currentShield.attackSpeed;
        }
        //update new Equipment
        EquipSkin(Equipment);
    }

    private void ResetWeapon()
    {
        //reset player status before equip weapon
        attackRange /= currentWeapon.attackRange;
        attackSpeed /= currentWeapon.attackSpeed;

        //destroy old weapon model and bullet prefab
        foreach (Transform child in weaponHold)
        {
            Destroy(child.gameObject);
        }
        bulletPrefab = null;
        ObjectPooling.Instance.DestroyBulletByAttacker(transform);
    }
}
