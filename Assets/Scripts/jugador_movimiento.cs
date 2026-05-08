using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EstadosMovimiento{
    quieto,
    caminando,
    saltando,
}

[RequireComponent(typeof(Rigidbody))]
public class jugador_movimiento : MonoBehaviour
{
    public float velocidad_movimiento = 0.5f;

    public float velocidad_rotacion = 1.0f;

    private float _velocidad_por_fotograma = 0.0f;

    public delegate void cambio_estado_evento(EstadosMovimiento estado_nuevo);
    public event cambio_estado_evento hay_gente_escuchando_el_estado;
    private EstadosMovimiento estado_actual = EstadosMovimiento.quieto; 

    private Rigidbody rigid_body;
    private PlayerInput entradas_del_jugador;
    private InputAction movimiento;
    private Animator controlador_de_animacion;

    private InputAction saltar;

    private Transform indicacion_direccion;

    void Start(){
        entradas_del_jugador = GetComponent<PlayerInput>();
        rigid_body = GetComponent<Rigidbody>();
        controlador_de_animacion = GetComponent<Animator>();

        movimiento = entradas_del_jugador.actions.FindAction("movimiento");

        _velocidad_por_fotograma = velocidad_movimiento / 60;

        indicacion_direccion = Camera.main.gameObject.transform;

        saltar = entradas_del_jugador.actions.FindAction("saltar");
        saltar.performed += salta_jugador_salta;
    }

    void salta_jugador_salta(InputAction.CallbackContext _){
        bool estamos_tocando_suelo = false;
        Ray rayo_hacia_el_suelo = new Ray(transform.position, new Vector3(0, -0.3f, 0));

        Debug.Log($"el rayo tiene la informacion {rayo_hacia_el_suelo}");

        RaycastHit chocamos_con;

        if (Physics.Raycast(rayo_hacia_el_suelo, out chocamos_con, 1.1f)){
            Debug.Log($"el rayo choco con {chocamos_con.collider.gameObject.name}");

            if (chocamos_con.collider.CompareTag("suelo")){
            estamos_tocando_suelo = true;
            }
        }

        if (estamos_tocando_suelo){
            rigid_body.AddForce(Vector3.up * 300f);
        }
        
    }
    
    void FixedUpdate(){
        Vector2 direccion = movimiento.ReadValue<Vector2>();

        if(direccion.magnitude > 0.1){
            cambiar_estado(EstadosMovimiento.caminando);
        }
        else{
            cambiar_estado(EstadosMovimiento.quieto);
        }

        Debug.Log($"El estado actual del controlador del movimiento es {estado_actual}");

        Debug.Log($"El valor de la direccion actual es: {direccion}");

        avanzar(direccion);
        //rotar(direccion); 
    }

    void avanzar(Vector2 direccion_joystick){
        if (Mathf.Abs(direccion_joystick.y) > 0.5f){
            cambiar_estado(EstadosMovimiento.caminando);
        }
        else{
            cambiar_estado(EstadosMovimiento.quieto);
        }


        Vector3 hacia_adelante = transform.forward * direccion_joystick.y;
        hacia_adelante += transform.right * direccion_joystick.x;
 
        rigid_body.MovePosition(transform.position + ((hacia_adelante * direccion_joystick.magnitude) * _velocidad_por_fotograma));
    }
 
    void rotar(Vector2 direccion_joystick){
        if (Mathf.Abs(direccion_joystick.x) > 0.5f){
            controlador_de_animacion.SetBool("estoy_volteando", true);
        }
        else{
            controlador_de_animacion.SetBool("estoy_volteando", false);
        }
        float voltear = velocidad_rotacion * direccion_joystick.x;
 
        Quaternion rotacion = Quaternion.Euler(0f, voltear, 0f);
        rigid_body.MoveRotation(transform.rotation * rotacion);
    }

    void cambiar_estado(EstadosMovimiento estado_nuevo){
        estado_actual = estado_nuevo;
        
        if(hay_gente_escuchando_el_estado != null){
            hay_gente_escuchando_el_estado.Invoke(estado_nuevo);
        }
    }
}
