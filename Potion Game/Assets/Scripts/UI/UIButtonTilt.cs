using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class UIButtonTilt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Coroutine Tilt;
    bool Dir = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ResetTilt();
        Tilt = StartCoroutine(InitializeTilt());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetTilt();
        transform.LeanRotateZ(0, 2).setEase(LeanTweenType.easeOutCirc);
    }
    IEnumerator InitializeTilt()
    {
        
        while(true)
        {
            if (Dir == true)
            {
                transform.LeanRotateZ(25, 2).setEase(LeanTweenType.easeOutQuad);
                yield return new WaitForSeconds(2f);
                Dir = false;
            }
            if (Dir == false)
            {
                transform.LeanRotateZ(-25, 2).setEase(LeanTweenType.easeOutQuad);
                yield return new WaitForSeconds(2f);
                Dir = true;
            }
        }
        yield return null;
    }
    void ResetTilt()
    {
        if (Tilt != null) { StopCoroutine(Tilt); }
        LeanTween.cancel(gameObject);
    }
}
