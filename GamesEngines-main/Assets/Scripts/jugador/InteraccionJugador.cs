using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class interaccionJugador : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interacciondistancia = 1f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interacciondistancia);
            foreach (Collider collider in colliderArray)
            {
               if (collider.TryGetComponent(out NpcInteractuable npcinteractubale)){
                    npcinteractubale.Interaccion();
                    
                }
            }
        }
    }
}
