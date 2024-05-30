using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject aiming;
    [SerializeField] private GameObject transparentZone;
    [SerializeField] private Material playerColor;
    private Vector3 currentRotation;
    private float fixedScale;

    // Start is called before the first frame update
    void Start()
    {
        //Update infor
        characterName = "You";
        bodyColor.material = playerColor;
        pantColor.material = playerColor;

        //
        currentRotation = transform.forward;
        ChangeAnim(AnimationState.idle);
        aiming.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

    private void TransparentZoneControl()
    {
        //control position
        Vector3 newPos = transform.position;
        newPos.y = 0.09f;
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
        if (Input.GetMouseButton(0))
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
        if (Input.GetMouseButtonUp(0))
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
}
