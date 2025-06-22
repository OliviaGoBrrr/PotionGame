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
    private RectTransform outlineRectTransform;
    private Image outlineImage;

    private bool isHovered;
    private bool isBounceTweening;
    private float iconSize = 1f;

    private float enterTimer;
    private float exitTimer;

    [SerializeField] CauldronStats cauldron;

    [SerializeField] GameObject outlineBorder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliders = GameObject.FindWithTag("SliderContainer").GetComponent<SliderContainer>();
        m_RectTransform = GetComponent<RectTransform>();
        outlineRectTransform = outlineBorder.GetComponent<RectTransform>();
        outlineImage = outlineBorder.GetComponent<Image>();
        outlineImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovered == true && m_RectTransform.localScale != new Vector3(1.2f, 1.2f, 1.2f))
        {
            enterTimer += Time.deltaTime;
            if (enterTimer >= 0.11 && m_RectTransform.localScale != new Vector3(1.2f, 1.2f, 1.2f))
            {
                OnEnter();
                enterTimer = 0;
            }
            
        }

        if (isHovered == false && m_RectTransform.localScale != new Vector3(1f, 1f, 1f))
        {
            exitTimer += Time.deltaTime;
            if (exitTimer >= 0.11 && m_RectTransform.localScale != new Vector3(1f, 1f, 1f))
            {
                OnExit();
                exitTimer = 0;
            }
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
        DOTween.Kill(m_RectTransform);
        DOTween.Kill(outlineRectTransform);

        Sequence bounceTween = DOTween.Sequence();
        isBounceTweening = true;
        bounceTween.Append(m_RectTransform.DOScale(0.8f, 0.08f).SetEase(Ease.OutSine));
        bounceTween.Join(outlineRectTransform.DOScale(0.8f, 0.08f).SetEase(Ease.OutSine));
        bounceTween.Append(m_RectTransform.DOScale(1f, 0.6f).SetEase(Ease.OutElastic)).OnComplete(() =>
        {
            isBounceTweening = false;
        });
        bounceTween.Join(outlineRectTransform.DOScale(1f, 0.6f).SetEase(Ease.OutElastic));


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

        outlineImage.enabled = true;

        if (isBounceTweening == true) return;

        DOTween.Kill(m_RectTransform);
        DOTween.Kill(outlineRectTransform);

        m_RectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutSine);
        outlineRectTransform.DOScale(1.2f, 0.1f).SetEase(Ease.OutSine);


        m_RectTransform.DOLocalRotate(new Vector3(0, 0, -5f), 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            m_RectTransform.DOLocalRotate(new Vector3(0, 0, 5f), 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        });

        outlineRectTransform.DOLocalRotate(new Vector3(0, 0, -5f), 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            outlineRectTransform.DOLocalRotate(new Vector3(0, 0, 5f), 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        });

    }

    public void OnExit()
    {
        isHovered = false;

        outlineImage.enabled = false;

        //DOTween.Kill(m_RectTransform);
        if (isBounceTweening == true) return;


        DOTween.Kill(m_RectTransform);
        DOTween.Kill(outlineRectTransform);

        m_RectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0);
        outlineRectTransform.DOLocalRotate(new Vector3(0, 0, 0), 0);

        m_RectTransform.DOScale(1f, 0.1f).SetEase(Ease.OutSine);
        outlineRectTransform.DOScale(1f, 0.1f).SetEase(Ease.OutSine);


    }
}
