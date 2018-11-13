using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HotKeys : MonoBehaviour
{
    // Fields
    #region
	[SerializeField] GameState activeGameState;
	public enum GameState
	{
		MainMenu,
        MainMenuMapSelection,
		MainMenuOptions,
		MainMenuCredits,
		InGame,
        InGameMenu,
        InShop,
        SelectingShopItemLocation,
        UnitMoving,
        UnitAttacking,
        GameOver
    }
    #endregion
    // Properties
    #region
    public GameState ActiveGameState { get { return this.activeGameState; } set { this.activeGameState = value; } }
    #endregion
    // Events
    #region
        void Start()
    {
        SceneManager.sceneLoaded += SceneStatus;
    }
    void Update()
	{
        KeyboardInput();
    }
    #endregion
    // Methods
    #region
    void KeyboardInput()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ExitingSituation();
        }
        if (Input.GetButtonDown("Submit"))
        {
            SubmitSituation();
        }
    }

    void ExitingSituation()
	{
		if (activeGameState == GameState.MainMenu) 
		{
			Application.Quit ();
            Debug.Log("Quiting");
		}
        else if (activeGameState == GameState.InGame)
        {
            ScriptLink.UIcontroller.InGameMenu.SetActive(true);
            ScriptLink.UIcontroller.InGameDefaultMenu.SetActive(true);
            ScriptLink.UIcontroller.InGameMenuOptions.SetActive(false);
            activeGameState = GameState.InGameMenu;
        }
        else if (activeGameState == GameState.InGameMenu)
        {
            ScriptLink.UIcontroller.InGameMenu.SetActive(false);
            activeGameState = GameState.InGame;
        }
        else if(activeGameState == GameState.InShop)
        {
            ScriptLink.UIcontroller.ToggleShop();
        }
        else
        {
            Debug.Log("This functionality is not implemented or the script is broken!");
        }
    }
    void SubmitSituation()
    {
        if(activeGameState == GameState.MainMenu)
        {

        }
        else if (activeGameState == GameState.InGame || activeGameState == GameState.UnitMoving || activeGameState == GameState.UnitAttacking)
        {
            ScriptLink.flowController.ChangeTurn();
            if(ScriptLink.mouseController.SelectedUnit != null)
            {
                ScriptLink.mouseController.UnselectUnit();
            }
        }
    }

    void SceneStatus(Scene scene, LoadSceneMode mode)
    {
        Scene sceneName = SceneManager.GetActiveScene();
        if (sceneName.name == "In Game")
        {
            activeGameState = GameState.InGame;
        }
        else if (sceneName.name == "Main Menu")
        {
            activeGameState = GameState.MainMenu;
            //BackToDefaultMainMenuButton = GameObject.Find ("Back To Default Main Menu Button");
            //DefaultMainMenu = GameObject.Find ("Default Main Menu");
            //Options = GameObject.Find("Options");
            //Credits = GameObject.Find("Credits");
        }
    }
    #endregion
}
