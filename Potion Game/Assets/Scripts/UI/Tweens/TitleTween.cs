using UnityEngine;
using UnityEngine.UI;

public class TitleTween : MonoBehaviour
{
    [SerializeField] private float tweenX;
    [SerializeField] private float tweenY;
    [SerializeField] private float tweenDuration;

    public bool moveAcross;
    public bool moveUp;
    public bool spline;
    public bool fadeOut;
    private Image fadeImage;

    [SerializeField] public Vector3[] splineCoords;
    
    public void TweenTitle()
    {
        // Move to a specific X coordinate
        if (moveAcross) { transform.LeanMoveLocalX(tweenX, tweenDuration).setEaseInBack(); }

        // Move to a specific Y coordinate
        else if (moveUp) { transform.LeanMoveLocalY(tweenY, tweenDuration).setEaseInBack(); }

        else if (spline)
        {
            transform.LeanMoveSplineLocal(splineCoords, tweenDuration).setEaseInBack();
        }

        else if (fadeOut)
        {
            fadeImage = transform.GetComponent<Image>();
            fadeImage.CrossFadeAlpha(0, tweenDuration, false);
        }

        // Move to a specific X,Y coordinate
        else { transform.LeanMoveLocal(new Vector2(tweenX, tweenY), tweenDuration); }
    }
}
