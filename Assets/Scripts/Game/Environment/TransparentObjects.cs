using System.Collections.Generic;
using UnityEngine;

public class TransparentObjects : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial;
    [SerializeField] private List<Renderer> _renderers;
    
    private List<Material> _myMaterials   = new List<Material>();
    private int _layer;
    private void Awake()
    {
        for (var i = 0; i < _renderers.Count; i++)
        {
            _myMaterials.Add(_renderers[i].material);
        }
        _layer = gameObject.layer;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("T "+other.tag);
        for (var i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material = _transparentMaterial;
        }
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnTriggerExit(Collider other)
    {
        for (var i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].material = _myMaterials[i];
        }
        gameObject.layer = _layer;
    }
}
