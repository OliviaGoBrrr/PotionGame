using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IngredientBasics : MonoBehaviour
{
    public int temperature;
    public int carbonation;
    public int pazaz;
    public int potency;
    public bool isHoney;
    [SerializeField] private SoundType soundEffectList;
    [SerializeField] private float soundEffectVolume = 0.4f;

    private SliderContainer sliders;

    private RectTransform m_RectTransform;

    private bool isHovered;
    private bool isBounceTweening;
    private float iconSize = 1f;

    private float bounceTimer;

    [SerializeField] CauldronStats cauldron;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliders = GameObject.FindWithTag("SliderContainer").GetComponent<SliderContainer>();
        m_RectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBounceTweening == true)
        {
            bounceTimer += Time.deltaTime;
        }

        if (bounceTimer >= 0.65)
        {

        }
    }

    public void OnClick()
    {
        if (sliders.isClicked == true) return; // no spam

        if (isHovered == true)
        {
            iconSize = 1.2f;
        }
        else
        {
            iconSize = 1f;
        }

        Sequence bounceTween = DOTween.Sequence();
        isBounceTweening = true;
        bounceTween.Append(m_RectTransform.DOScale(0.6f, 0.08f).SetEase(Ease.OutSine));
        bounceTween.Append(m_RectTransform.DOScale(1f, 0.8f).SetEase(Ease.OutElastic)).OnComplete(() =>
        {
            isBounceTweening = false;
        });


        /*
                    if (isHovered == true)
            {
                m_RectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutSine);
            }
            isBounceTweening = false;

        */

        sliders.UpdateSliderStats(temperature, carbonation, pazaz, potency, isHoney);
        SoundManager.PlayRandomSound(soundEffectList, soundEffectVolume);
    }


    public void OnEnter()
    {
        isHovered = true;
        if (isBounceTweening == true) return;
        m_RectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutSine);
    }

    public void OnExit()
    {
        isHovered = false;
        //DOTween.Kill(m_RectTransform);
        if (isBounceTweening == true) return;
        m_RectTransform.DOScale(1f, 0.1f).SetEase(Ease.OutSine);
    }
}
