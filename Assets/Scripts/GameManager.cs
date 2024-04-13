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
    private int currPage;

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

        currPage = 0;
        
        DeactivatePages();
        SetToPage(currPage);
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

    // Sets the Grimoire to the provided page
    private void SetToPage(int page)
    {
        // deactivates previous page if one exists
        if(page > 0)
        { grimoire.transform.GetChild(page - 1).gameObject.SetActive(false); }
        if(page < (grimoire.transform.childCount - 1))
        { grimoire.transform.GetChild(page + 1).gameObject.SetActive(false); }

        grimoire.transform.GetChild(page).gameObject.SetActive(true);
    }

    // Deactivates each page in the Grimoire
    private void DeactivatePages()
    {
        for(int i = 0; i < grimoire.transform.childCount; i++)
        { grimoire.transform.GetChild(i).gameObject.SetActive(false); }
    }

    #endregion

    #region Input
    
    // Called when the script is enabled
    private void OnEnable()
    {
        /* input.Enable();
        input.Menus.ToggleGrimoire.performed += OnPausePerformed;
        input.Menus.CloseStart.performed += OnStartPerformed;
        input.Menus.FlipForward.performed += OnFlipForwardPerformed;
        input.Menus.FlipBackward.performed += OnFlipBackwardPerformed; */
    }

    // Called when the script is disabled
    private void OnDisable()
    {
        /* input.Disable();
        input.Menus.ToggleGrimoire.performed -= OnPausePerformed;
        input.Menus.CloseStart.performed -= OnStartPerformed;
        input.Menus.FlipForward.performed -= OnFlipForwardPerformed;
        input.Menus.FlipBackward.performed -= OnFlipBackwardPerformed; */
    }

    // Called when any of the binds associated with PauseMenu in input are used
    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        // only opens the pause menu if the game is active
        if(grimoire.activeSelf)
        { 
            CloseMenus();
            gameActive = true;
        }
        else if(gameStarted) { OpenGrimoire(); }
    }

    // Called when any of the binds associated with CloseStart in input are used
    private void OnStartPerformed(InputAction.CallbackContext context)
    {
        // only opens the level selection menu if the start menu is active
        if(startScreen.activeSelf) 
        { 
            CloseMenus();
            gameStarted = true;
        }
    }

    // Flips the Grimoire forward a page (if any are left)
    private void OnFlipForwardPerformed(InputAction.CallbackContext context)
    {
        // ensures Grimoire is open first
        if((grimoire.activeSelf) && (currPage + 1 < grimoire.transform.childCount))
        { SetToPage(++currPage); }
    }

    // Flips the Grimoire backward a page (if any are left)
    private void OnFlipBackwardPerformed(InputAction.CallbackContext context)
    {
        // ensures Grimoire is open first
        if((grimoire.activeSelf) && (currPage - 1 >= 0))
        { SetToPage(--currPage); }
    }

    #endregion

    // Causes the game to close
    public void Quit()
    {
        Application.Quit();
    }
}
