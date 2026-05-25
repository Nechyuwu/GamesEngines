using UnityEngine;
using TMPro;

public class jugador_coleccion : MonoBehaviour {
    public int contadorColeccionables = 0;

    [Header("UI de Coleccionables")]
    [SerializeField] private TextMeshProUGUI textoContador; 
    private void Start() {
        ActualizarInterfaz();
    }

    public void AgregarColeccionable() {
        contadorColeccionables++;   
        ActualizarInterfaz();
    }
    private void ActualizarInterfaz() {
        if (textoContador != null) {
            textoContador.text = "Objetos:" + contadorColeccionables + "/16";
        } else {
            Debug.LogWarning("No has asignado el componente de Texto en el script jugador_coleccion.");
        }
    }
}