using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Boton de "JUGAR", para cambiar entre escenas (menu y juego)
    //Boton de "JUGAR", para cambiar entre escenas (menu y juego)
// Assign the EXACT name of your main menu scene in the inspector for safety
[SerializeField] private string mainMenuSceneName = "MenuPrincipal";

// Used by BOTH Play and Pause buttons
public void IrAEscena(string nombreEscena)
{
    // CASE 1: You are in the game and pressing PAUSE (target is the Main Menu)
    if (nombreEscena == mainMenuSceneName)
    {
        Time.timeScale = 0f; // Freeze game actions/physics
        // Load menu on top without wiping the game stage
        SceneManager.LoadScene(nombreEscena, LoadSceneMode.Additive);
    }
    // CASE 2: You are in the menu and pressing PLAY/RESUME (target is the Game Stage)
    else
    {
        Scene targetScene = SceneManager.GetSceneByName(nombreEscena);

        if (targetScene.isLoaded)
        {
            // If the game stage is already open, unfreeze it and close this menu overlay
            Time.timeScale = 1f;
            SceneManager.UnloadSceneAsync(gameObject.scene.name);
        }
        else
        {
            // If starting fresh from boot up, load the game scene normally
            Time.timeScale = 1f;
            SceneManager.LoadScene(nombreEscena);
        }
    }
}

    //Boton de "SALIR", para cerrar el juego
    public void SalirApp(){
        Application.Quit();
        Debug.Log("Application has quit.");
    }
}
