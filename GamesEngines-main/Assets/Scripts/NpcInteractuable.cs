using UnityEngine;

public class NpcInteractuable : MonoBehaviour 
{
   
    [SerializeField] private AudioManager.SoundType sonidoDeInteraccion;

    [TextArea(3, 5)] // Esto hace que el cuadro de texto sea más grande y cómodo en el Inspector
    [SerializeField] private string textoDeInteraccion;

    public void Interaccion()
    {
        Debug.Log($"Interacción hecha con {gameObject.name}");

        // 1. Reproducir Sonido
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play(sonidoDeInteraccion);
        }

        // 2. Mostrar Texto en Pantalla
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.MostrarTexto(textoDeInteraccion);
        }
        else
        {
            Debug.LogWarning("No se encontró el DialogManager en la escena.");
        }
    }
}