using UnityEngine;

public class Coleccionable : MonoBehaviour {
    public GameObject efectoParticulas;


    private void OnTriggerEnter(Collider other) {
        jugador_coleccion jugador = other.GetComponent<jugador_coleccion>();
        if (jugador != null) {

            jugador.AgregarColeccionable();
          Instantiate(efectoParticulas, transform.position, Quaternion.identity);
         AudioManager.Instance.Play(AudioManager.SoundType.Objetorecolectado);
            Destroy(gameObject); // Elimina el objeto al recogerlo
        }
    }
}