using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CameraPan : MonoBehaviour
{
    public int currentCamPos = 0;
    GameManager gameManager;
    [SerializeField] GameObject cauldron;
    // Update is called once per frame
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && gameManager.gameState != 0)
        {
            OnButtonPress(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && gameManager.gameState != 0)
        {
            OnButtonPress(1);
        }
    }
    public void OnButtonPress(int cameraPos)
    {
        if (gameManager.gameState != 0)
        {
            switch (cameraPos)
            {
                case 0:
                    GoHere(new Vector3(970, 0, -926));
                    cauldron.GetComponent<CauldronVisuals>().StartTheRock(Mathf.Abs(cauldron.transform.position.x - 970) / 100, false);
                    break;
                case 1:
                    GoHere(new Vector3(-970, 0, -926));
                    cauldron.GetComponent<CauldronVisuals>().StartTheRock(Mathf.Abs(cauldron.transform.position.x + 970) / 100, true);
                    break;
                default:
                    GoHere(new Vector3(0, 0, -926));
                    break;
            }
        }
    }
    void GoHere(Vector3 hi)
    {
        LeanTween.move(gameObject, hi, 1f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(cauldron, new Vector3(hi.x, -540, 0), 1f).setEase(LeanTweenType.easeInOutQuart);
        SoundManager.PlayRandomSound(SoundType.CAULDRON_MOVE, 0.1f);
    }
}
