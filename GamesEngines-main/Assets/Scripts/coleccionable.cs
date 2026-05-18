using UnityEngine;

public class Coleccionable : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        jugador_coleccion jugador = other.GetComponent<jugador_coleccion>();
        if (jugador != null) {
            jugador.AgregarColeccionable();
            Destroy(gameObject); // Elimina el objeto al recogerlo
        }
    }
}