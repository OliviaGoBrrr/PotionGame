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
        if (Input.GetKeyDown(KeyCode.RightArrow) && gameManager.gameStarted == true)
        {
            OnButtonPress(0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && gameManager.gameStarted == true)
        {
            OnButtonPress(1);
        }
    }
    public void OnButtonPress(int cameraPos)
    {
        switch(cameraPos)
        {
            case 0:
                GoHere(new Vector3(0, 0, -10));
                cauldron.GetComponent<CauldronVisuals>().StartTheRock(Mathf.Abs(cauldron.transform.position.x - 0), false);
                break;
            case 1:
                GoHere(new Vector3(-21, 0, -10));
                cauldron.GetComponent<CauldronVisuals>().StartTheRock(Mathf.Abs(cauldron.transform.position.x + 21), true);
                break;
            default:
                GoHere(new Vector3(0, 0, -10));
                break;
        }
    }
    void GoHere(Vector3 hi)
    {
        LeanTween.move(gameObject, hi, 1f).setEase(LeanTweenType.easeOutQuint);
        LeanTween.move(cauldron, new Vector3(hi.x, -5.5f, 0), 1f).setEase(LeanTweenType.easeInOutQuart);
    }
}
