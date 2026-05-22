using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("Componentes de UI")]
    [SerializeField] private GameObject panelDialogo; // El fondo del diálogo
    [SerializeField] private TextMeshProUGUI textoDialogo; // El componente de texto

    private void Awake()
    {
        // Clonamos el patrón Singleton que usaste en tu AudioManager
        Instance = this;
        
        // Nos aseguramos de que el panel empiece oculto
        if (panelDialogo != null) 
            panelDialogo.SetActive(false);
    }

    // Este método lo llamarán los NPCs para mostrar su texto
    public void MostrarTexto(string texto)
    {
        panelDialogo.SetActive(true);
        textoDialogo.text = texto;
        
        // Cancela invocaciones previas por si hablas muy rápido con otro NPC
        CancelInvoke(nameof(OcultarTexto)); 
        
        // Oculta el texto automáticamente después de 4 segundos (puedes cambiarlo)
        Invoke(nameof(OcultarTexto), 4f);
    }

    private void OcultarTexto()
    {
        panelDialogo.SetActive(false);
    }
}
