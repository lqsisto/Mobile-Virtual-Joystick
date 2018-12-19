using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image _BGImage;
    private Image _joystickImage;
    private Vector3 _inputVector;

    public static bool JoystickPressed;

    void Start ()
    {
        _BGImage = transform.Find ("JoystickBG").GetComponent<Image> ();

        _joystickImage = transform.Find ("JoystickFG").GetComponent<Image> ();
    }
    public virtual void OnDrag (PointerEventData pod)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (_BGImage.rectTransform,
                pod.position,
                pod.pressEventCamera,
                out pos))
        {
            pos.x = (pos.x / _BGImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / _BGImage.rectTransform.sizeDelta.y);

            _inputVector = new Vector3 (pos.x * 2 + 1, 0, pos.y * 2 - 1);
            _inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;

            _joystickImage.rectTransform.anchoredPosition = new Vector3 (_inputVector.x * (_BGImage.rectTransform.sizeDelta.x / 2.5f),
                _inputVector.z * (_BGImage.rectTransform.sizeDelta.y / 2.5f));
        }
    }

    public virtual void OnPointerDown (PointerEventData pod)
    {
        OnDrag (pod);
        JoystickPressed = true;
    }

    public virtual void OnPointerUp (PointerEventData pod)
    {
        _inputVector = Vector3.zero;
        _joystickImage.rectTransform.anchoredPosition = Vector3.zero;
        JoystickPressed = false;
    }

    public float Horizontal ()
    {
        if (_inputVector.x != 0)
        {
            return _inputVector.x;
        }
        else
        {
            return Input.GetAxis ("Horizontal");
        }
    }
    public float Vertical ()
    {
        if (_inputVector.z != 0)
        {
            return _inputVector.z;
        }
        else
        {
            return Input.GetAxis ("Vertical");
        }
    }

}