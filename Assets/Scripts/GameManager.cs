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
    [SerializeField] private TextMeshProUGUI tableOfContents, pageNums;

    // Boolean Variables
    public static bool gameActive, gameStarted;

    // Integer Variables
    private int currPage, maxPage;

    // Animator Variables
    [SerializeField] private Animator grimoireAnimator;

    // BoxCollider2D Variables

    // SpriteRenderer Variables

    // Sprite Variables

    // Image Variables

    // Script Variables

    #endregion

    // Called when the game is loaded
    private void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        gameActive = false;
        gameStarted = false;

        currPage = 1;
        maxPage = 4;
        
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
        grimoireAnimator.SetBool("Opened", true);
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

    // Called when any of the binds associated with PauseMenu in input are used
    public void OnPausePerformed(InputAction.CallbackContext context)
    {
        // ensures button release doesn't call function
        if(context.canceled) { return; }
        
        // only opens the pause menu if the game is active
        if(grimoire.activeSelf)
        { 
            /* grimoireAnimator.SetBool("Opened", false);
            gameActive = true;
            yield return new WaitForSeconds(0.5f);
            CloseMenus(); */
            StartCoroutine(CloseGrimoire());
        }
        else if(gameStarted) { OpenGrimoire(); }
    }

    // Called when any of the binds associated with CloseStart in input are used
    public void OnStartPerformed(InputAction.CallbackContext context)
    {
        // only opens the level selection menu if the start menu is active
        if(startScreen.activeSelf) 
        { 
            OpenGrimoire();
            gameStarted = true;
        }
    }

    // Flips the Grimoire forward a page (if any are left)
    public void OnFlipForwardPerformed(InputAction.CallbackContext context)
    {
        // ensures Grimoire is open first
        if((context.performed) && (grimoire.activeSelf) && (currPage < maxPage)
            && (currPage + 1 < grimoire.transform.childCount))
        { SetToPage(++currPage); }
    }

    // Flips the Grimoire backward a page (if any are left)
    public void OnFlipBackwardPerformed(InputAction.CallbackContext context)
    {
        // ensures Grimoire is open first
        if((context.performed) && (grimoire.activeSelf) 
            && (currPage - 1 >= 0))
        { SetToPage(--currPage); }
    }

    #endregion

    #region Add Summons

    // Adds Face Heap to the Grimoire
    private void AddFaceHeap()
    {
        tableOfContents.text += "\nψ F̴̻̠̙̃͐̿̄̈a̴̡̰͚͒̓̾̀͜c̵̡͐ȩ̶̡̛̮͈̭͒̐̇͠ ̸̺̓Ȟ̶̦̻̜̝ͅè̸̞̼̠̏̿̋̚ͅa̶̧̤͙̞̐p̶̯̌ ⛥";
        pageNums.text += "\n...6";
        maxPage = 5;
    }

    // Adds Archangel to the Grimoire
    private void AddArchangel()
    {
        tableOfContents.text += "\n⛧ Ä̴̡̞̪̖͖́͊͝r̵̛͍̜̐̉̔c̵̢̤̜͈̏ĥ̸̡̝͗͒͌ā̷͉͙̪̹͒͜n̴̡̛̖̠̐͘̕g̴̡̨̖͎̩͊̿͝ȅ̴̱͖̿͠l̶̲̺̈̃͗ ⛧";
        pageNums.text += "\n...7";
        maxPage = 6;
    }

    // Adds Ice Monster to the Grimoire
    private void AddIceMonster()
    {
        tableOfContents.text += "\n☠ Ī̴̼̙̫̪̺̀̊̚͘c̷̡̛̜̼̼̿e̵̺̻̝͇̅̀̈́ ̵̥̭͍͘M̶̞̺̯̽o̶̿̅̅͜n̷̡̙͍͚̳̊͂̓ş̸̝̻͔̘̅̐̏͠t̵̢͓̱͉͑ě̷̺͚̇̑̕r̷͓̻̲̖͐̌̂͠͝ ☠";
        pageNums.text += "\n...8";
        maxPage = 7;
    }

    // Adds Sumdoo to the Grimoire
    private void AddSumdo()
    {
        tableOfContents.text += "\n⛧ Ş̸̼̜̗͊̂̐̃̕u̶̧͖̠̭͌̐͌̕̚m̶͍͓̊d̵̡̻̫̃̾̿͜o̵͕͇̓̓̂̏͜o̷̺͍̒̒̎̀̓ ⛥";
        pageNums.text += "\n...9";
        maxPage = 8;
    }

    // Adds Crying Sun to the Grimoire
    private void AddCryingSun()
    {
        tableOfContents.text += "\n☠ C̵͎̫͌̈́̈́̄̚r̷̡͍̫͆̀̋y̴̲̱̺̗̭͑́͝i̴͙͎̞͇̟̍n̶̡̗̻̞͍̋̒̎̕g̵̠̮͔̜̹̍̓̑ ̵̩̦̤̥͌S̸̩̹͙̲͌u̶͜͝ń̷̰̺̟̭͇̎̂́ ψ";
        pageNums.text += "\n..10";
        maxPage = 9;
    }

    // Adds Lucifer to the Grimoire
    private void AddLucifer()
    {
        tableOfContents.text += "\n⛥ L̶̢̑ŭ̴̮̈́̇ć̸̫̈́͝ĭ̴̬̐̕͝͠f̴͓̅́͂͛e̸̛̘r̸̖͇͚̃ ⛥";
        pageNums.text += "\n..11";
        maxPage = 10;
    }

    #endregion
    
    // Causes the game to close
    public void Quit()
    {
        Application.Quit();
    }

    // Closes the Grimoire
    private IEnumerator CloseGrimoire()
    {
        grimoireAnimator.SetBool("Opened", false);
        gameActive = true;
        yield return new WaitForSeconds(0.5f);
        CloseMenus();
    }
}
