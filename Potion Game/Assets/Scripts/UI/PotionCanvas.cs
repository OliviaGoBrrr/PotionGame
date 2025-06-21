using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PotionCanvas : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] CauldronStats cauldron;
    [SerializeField] Image canvas;
    [SerializeField] Image potionGlass;
    [SerializeField] Image potionLiquid;

    [SerializeField] List<Sprite> potionGlasses;
    [SerializeField] List<Sprite> potionLiquids;
    float R;
    float G;
    float B;
    float A;
    float Alpha;
    bool active = false;
    public void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    public void FadeIn(int potionVariant)
    {
        cauldron.PullStats(out R, out G, out B, out A);
        potionGlass.sprite = potionGlasses[potionVariant];
        potionLiquid.sprite = potionLiquids[potionVariant];
        active = true;
    }
    private void Update()
    {
        switch (active)
        {
            case true:
                if (Alpha <= 1)
                {
                    Alpha += Time.deltaTime * 0.5f;
                    ColourUpdate();
                }
                break;
            case false:
                if (Alpha >= 0)
                {
                    Alpha -= Time.deltaTime * 0.5f;
                    ColourUpdate();
                }
                break;
        }
        if (Input.anyKey == true && Alpha >= 1)
        {
            active = false;
        }
        if (active == false && Alpha <= 0)
        {
            gameManager.PotionSubmitted();
            gameObject.SetActive(false);
        }
    }
    void ColourUpdate()
    {
        canvas.color = new Color(1, 1, 1, Alpha);
        potionGlass.color = new Color(1, 1, 1, Alpha);
        potionLiquid.color = new Color(R / 10, G / 10, B / 10, Mathf.Clamp(Alpha, 0, A / 100));
    }
}
