using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Assign your canvases in the Inspector window
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject collectiblesCanvas;

    private void Start()
    {
        // Initialize the screen layout by default
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        collectiblesCanvas.SetActive(false);
    }

    public void ShowCollectibles()
    {
        mainMenuCanvas.SetActive(false);
        collectiblesCanvas.SetActive(true);
    }
}
