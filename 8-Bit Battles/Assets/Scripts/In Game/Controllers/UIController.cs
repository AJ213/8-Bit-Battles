using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject UICanvas;

    public GameObject InGameMenu;
    public GameObject unitStats;
    public GameObject unitMenu;
    public GameObject endTurnButton;
    public GameObject gameStatsBar;
    public GameObject shop;
    public GameObject gameOver;

    public GameObject InGameDefaultMenu;
    public GameObject InGameMenuOptions;

    public GameObject map1;
    public GameObject map2;
    public GameObject map3;

    public GameObject unitComparasion;

    public Text gameOverText;

    public Sprite endTurnButtonRed;
    public Sprite endTurnButtonGreen;

    void ChangeEndTurnButton()
    {
        if(ScriptLink.flowController.IsRedTurn)
        {
            endTurnButton.GetComponent<Image>().sprite = endTurnButtonRed;
        }
        else
        {
            endTurnButton.GetComponent<Image>().sprite = endTurnButtonGreen;
        }
    }
    void OnDisable()
    {
        FlowControl.OnTurnChange -= this.ChangeEndTurnButton;
    }
    void Start()
    {
        FlowControl.OnTurnChange += this.ChangeEndTurnButton;

        UICanvas.SetActive(true);

        if(GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 1)
        {
            map1.SetActive(true);
            map2.SetActive(false);
            map3.SetActive(false);
        }
        else if(GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 2)
        {
            map1.SetActive(false);
            map2.SetActive(true);
            map3.SetActive(false);
        }
        else if (GameObject.FindGameObjectWithTag("Value Carrier").GetComponent<ExtraValues>().mapNum == 3)
        {
            map1.SetActive(false);
            map2.SetActive(false);
            map3.SetActive(true);
        }
    }

    public void ToggleShop()
    {
        if (shop.activeSelf == true)
        {
            
            shop.SetActive(false);
            ToggleAllUI();
        }
        else
        {
            ToggleAllUI();
            
            shop.SetActive(true);
            gameStatsBar.SetActive(false);
            endTurnButton.SetActive(false);
        }
    }

    public void ToggleAllUI()
    {
        bool anyUIOn = shop.activeSelf || gameStatsBar.activeSelf || endTurnButton.activeSelf || unitMenu.activeSelf || unitStats.activeSelf || InGameMenu.activeSelf;
        if (anyUIOn)
        {
            InGameMenu.SetActive(false);
            unitStats.SetActive(false);
            unitMenu.SetActive(false);
            endTurnButton.SetActive(false);
            shop.SetActive(false);
            gameStatsBar.SetActive(false);
        }
        else
        {
            gameStatsBar.SetActive(true);
            endTurnButton.SetActive(true);
            
        }
    }

    public void TurnOnGameOver(bool redWon)
    {
        ToggleAllUI();
        gameOver.SetActive(true);
        if (redWon)
        {
            gameOverText.text = "Game Over. Red team won.";
        }
        else
        {
            gameOverText.text = "Game Over. Green team won.";
        }
    }

    public void ToggleUnitComparasion()
    {
        if (unitComparasion.activeSelf == true)
        {

            unitComparasion.SetActive(false);
        }
        else
        {
            unitComparasion.SetActive(true);
        }
    }
}
