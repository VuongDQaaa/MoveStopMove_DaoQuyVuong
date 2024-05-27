using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject aiming;
    [SerializeField] private LayerMask transparentLayer;
    private Vector3 currentRotation;
    [SerializeField] private Material tranparentMat;

    // Start is called before the first frame update
    void Start()
    {
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
            MakeTransparentObject();
        }
    }

    private void MakeTransparentObject()
    {
        //make object in attackRange become transparent
        Collider[] objectInViewRadius = Physics.OverlapSphere(transform.position, attackRange, transparentLayer);
        if(objectInViewRadius.Length > 0)
        {
            foreach (Collider item in objectInViewRadius)
            {
                Renderer meshRenderer = item.GetComponent<Renderer>();
                meshRenderer.material = tranparentMat;
            }
        }
    }

    private void ShowAiming()
    {
        if(currentTarget != null)
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
