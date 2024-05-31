using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CanvasCharacter : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Character currentCharacter;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterPoint;
    [SerializeField] private Image pointImage;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            characterName.gameObject.SetActive(true);
            pointImage.gameObject.SetActive(true);
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            UpdateCanvas();
        }
        else
        {
            characterName.gameObject.SetActive(false);
            pointImage.gameObject.SetActive(false);
        }
    }

    private void UpdateCanvas()
    {
        characterName.text = currentCharacter.characterName;
        characterName.color = currentCharacter.GetCharacterColor();
        characterPoint.text = currentCharacter.currentPoint.ToString();
        pointImage.color = currentCharacter.GetCharacterColor();
    }
}
