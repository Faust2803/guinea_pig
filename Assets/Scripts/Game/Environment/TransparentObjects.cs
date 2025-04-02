using UnityEngine;

public class TransparentObjects : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial;

    private Material _myMaterial;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _myMaterial = _renderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("T "+other.tag);
        _renderer.material = _transparentMaterial;
    }

    private void OnTriggerExit(Collider other)
    {
        _renderer.material = _myMaterial;
    }
}
