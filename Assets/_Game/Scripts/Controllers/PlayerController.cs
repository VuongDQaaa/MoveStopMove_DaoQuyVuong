using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float speed = 5f;
    private Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = transform.forward;
        ChangeAnim(AnimationState.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (JoyStickController.direction != Vector3.zero)
        {
            currentRotation = JoyStickController.direction;
        }
        DetectTarget();
        Attack();
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        if (Input.GetMouseButton(0))
        {
            ChangeAnim(AnimationState.run);
            currentTarget = null;
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
