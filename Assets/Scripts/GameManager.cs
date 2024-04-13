using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region Variables
    
    // GameObject Variables
    [SerializeField] private GameObject startScreen, grimoire;

    // TextMeshProUGUI Variables

    // Boolean Variables
    public static bool gameActive, gameStarted;

    // Integer Variables

    // BoxCollider2D Variables

    // SpriteRenderer Variables

    // Sprite Variables

    // Image Variables

    // Script Variables
    /* private InputMap input; */

    #endregion

    // Called when the game is loaded
    private void Awake()
    {
        /* input = new InputMap(); */
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameActive = false;
        gameStarted = false;
        OpenStart();
    }

    #region Menu Operations
    
    // Deactivates every menu
    void CloseMenus()
    {
        startScreen.SetActive(false);
        grimoire.SetActive(false);
    }

    // Opens the Start Screen
    public void OpenStart()
    {
        gameActive = false;
        gameStarted = false;
        CloseMenus();
        startScreen.SetActive(true);
    }

    // Opens the Pause Menu
    public void OpenGrimoire()
    {
        gameActive = false;
        CloseMenus();
        grimoire.SetActive(true);
    }

    #endregion

    #region Input
    
    // Called when the script is enabled
    private void OnEnable()
    {
        /* input.Enable();
        input.Menus.Grimoire.performed += OnPausePerformed;
        input.Menus.CloseStart.performed += OnStartPerformed; */
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        /* input.Disable();
        input.Menus.Grimoire.performed -= OnPausePerformed;
        input.Menus.CloseStart.performed -= OnStartPerformed; */
    }

    // Called when any of the binds associated with PauseMenu in input are used
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // only opens the pause menu if the game is active
        if(grimoire.activeSelf) { CloseMenus(); }
        else if(gameStarted) { OpenGrimoire(); }
    }

    // Called when any of the binds associated with CloseStart in input are used
    private void OnStartPerformed(InputAction.CallbackContext context)
    {
        // only opens the level selection menu if the start menu is active
        if(startScreen.activeSelf) { CloseMenus(); }
    }

    #endregion

    // Causes the game to close
    public void Quit()
    {
        Application.Quit();
    }
}
