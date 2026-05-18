using UnityEngine;

public class EnemigoMovimiento : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent Enemigo;
    public float Velocidad;
    public bool Persiguiendo;
    public float Rango;
   public float Distancia;

    public Transform Objetivo;

    private void Update()
    {
        Distancia = Vector3.Distance(Enemigo.transform.position, Objetivo.position);

        if(Distancia < Rango)
        {
            Persiguiendo = true;
        }
        else if(Distancia > Rango + 3)
        {
            Persiguiendo = false;
        }
    
        if(Persiguiendo == false)
        {
            Enemigo.speed = 0;
            
        }else if(Persiguiendo == true)
        {
            Enemigo.speed = Velocidad;
            Enemigo.SetDestination(Objetivo.position);
        }
    }

   
}
