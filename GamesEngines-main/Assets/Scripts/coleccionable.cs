using UnityEngine;

public class Coleccionable : MonoBehaviour {
    public GameObject efectoParticulas;

    private void OnTriggerEnter(Collider other) {
        jugador_coleccion jugador = other.GetComponent<jugador_coleccion>();
        if (jugador != null) {

            jugador.AgregarColeccionable();
            GameObject nuevaParticula = Instantiate(efectoParticulas, transform.position, transform.rotation);
            ParticleSystem sistemaParticulas = nuevaParticula.GetComponent<ParticleSystem>();
            
            if (sistemaParticulas != null) {
   
                Destroy(nuevaParticula, sistemaParticulas.main.duration);
            } else {
    
                Destroy(nuevaParticula, 3f);
            }

            if (AudioManager.Instance != null) {
                AudioManager.Instance.Play(AudioManager.SoundType.Objetorecolectado);
            }

            Destroy(gameObject); 
        }
    }
}