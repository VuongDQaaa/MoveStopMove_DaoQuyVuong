using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private bool destroyOnClose = false;

    private void Awake()
    {
        // rect tranform for 720 x 1600 screen and iphone
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 righttop = rect.offsetMax;

            leftBottom.y = 0;
            righttop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = righttop;
        }
    }

    //call first before canvas actived
    public virtual void SetUp()
    { }

    //call after canvas active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    //close canvas after (time)
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }

    //close canvas directly
    public virtual void CloseDirectly()
    {
        if (destroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

