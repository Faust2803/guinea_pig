using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    [SerializeField] bool hideWhenNotUse = true;
    [SerializeField] bool _backToStartPosition;
    [SerializeField] bool _displayStartup;
    
    private Vector3 _startPosition;
    private bool _isFirst = true;

    protected override void Start()
    {
        base.Start();
        ChangeActive(_displayStartup);
        
        _startPosition = background.transform.position;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        ChangeActive(true);
        base.OnPointerDown(eventData);
        
        if (_isFirst)
        {
            OnPointerUp(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        ChangeActive(false);
        base.OnPointerUp(eventData);
        if (_backToStartPosition)
        {
            background.transform.position = _startPosition;
        }
        if (_isFirst)
        {
            _isFirst = false;
            OnPointerDown(eventData);
        }
    }

    private void ChangeActive (bool value)
    {
        if (hideWhenNotUse)
            background.gameObject.SetActive(value);
    }
}