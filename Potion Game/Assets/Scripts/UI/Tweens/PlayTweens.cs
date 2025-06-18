using UnityEngine;

public class PlayTweens : MonoBehaviour
{
    private TitleTween[] titleTweens;
    public void Awake()
    {
        titleTweens = GetComponentsInChildren<TitleTween>();
    }
    public void PlayAllTweens()
    {
        for (int i = 0; i < titleTweens.Length; i++)
        {
            titleTweens[i].TweenTitle();
        }
    }

    public void PlaySettingsTweens()
    {

    }
}
