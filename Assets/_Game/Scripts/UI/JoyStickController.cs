using UnityEngine;

public class JoyStickController : UICanvas
{
    public static Vector3 direction;
    [SerializeField] private GameObject joystick;
    [SerializeField] private RectTransform bg, knob;
    [SerializeField] private float knobRange;
    private Vector3 startPos, currentPos;
    private Vector3 screen;
    private Vector3 mousePos => Input.mousePosition - screen / 2;

    private void Awake()
    {
        screen.x = Screen.width;
        screen.y = Screen.height;
    }
    private void OnEnable()
    {
        direction = Vector3.zero;
    }

    private void OnDisable()
    {
        direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = mousePos;
            joystick.SetActive(true);
            bg.anchoredPosition = startPos;
        }
        if (Input.GetMouseButton(0))
        {
            currentPos = mousePos;
            //calculate postion of knob
            knob.anchoredPosition = Vector3.ClampMagnitude((currentPos - startPos), knobRange) + startPos;

            //Calculate vector direction
            direction = (currentPos - startPos).normalized;
            direction.z = direction.y;
            direction.y = 0;
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystick.SetActive(false);
            direction = Vector3.zero;
        }
    }
}
