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
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        characterName.text = currentCharacter.characterName;
        characterName.color = currentCharacter.bodyColor.material.color;
        characterPoint.text = currentCharacter.currentPoint.ToString();
        pointImage.color = currentCharacter.bodyColor.material.color;
    }
}
