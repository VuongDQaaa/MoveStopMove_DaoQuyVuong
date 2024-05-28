using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField] private Material transparentMat;
    private MeshRenderer meshRenderer;
    private Material originMat;
    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
        originMat = meshRenderer.material;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constant.TAG_TRANSPARENT))
        {
            meshRenderer.material = transparentMat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_TRANSPARENT))
        {
            meshRenderer.material = originMat;
        }
    }
}
