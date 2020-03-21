using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image bGImage;
    [SerializeField] private Image joystickImage;
    private Vector3 inputVector;

    public static bool JoystickPressed;
    
    
    public virtual void OnDrag (PointerEventData pod)
    {
        Vector2 pos;
        var bgImageSizeDelta = bGImage.rectTransform.sizeDelta;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bGImage.rectTransform,
                pod.position,
                pod.pressEventCamera,
                out pos))
        {
            pos.x /= bgImageSizeDelta.x;
            pos.y /= (bgImageSizeDelta.y);

            
            //Let's assume that we are working with a 3D world. If necessary, change inputVector to use y-axis.
            inputVector = new Vector3 (pos.x, 0, pos.y);

            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystickImage.rectTransform.anchoredPosition = new Vector3 (inputVector.x * (bgImageSizeDelta.x / 2.5f),
                inputVector.z * (bgImageSizeDelta.y / 2.5f));

            //Get input vectors from joystick to use in movement
            Horizontal();
            Vertical();
        }
    }

    public virtual void OnPointerDown (PointerEventData pod)
    {
        OnDrag (pod);
        JoystickPressed = true;
    }

    public virtual void OnPointerUp (PointerEventData pod)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero;
        JoystickPressed = false;
    }

    public float Horizontal ()
    {
        return inputVector.x != 0 ? inputVector.x : Input.GetAxis ("Horizontal");
    }
    public float Vertical ()
    {
        return inputVector.z != 0 ? inputVector.z : Input.GetAxis ("Vertical");
    }

}