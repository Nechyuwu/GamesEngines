using UnityEngine;

public class jugador_coleccion : MonoBehaviour {
    public int contadorColeccionables = 0;

    public void AgregarColeccionable() {
        contadorColeccionables++;
        Debug.Log("Coleccionables recogidos: " + contadorColeccionables);
    }
}