using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EstadosMovimiento {
    quieto,
    caminando,
    saltando,
    dash
}

[RequireComponent(typeof(Rigidbody))]
public class jugador_movimiento : MonoBehaviour {
    public float velocidad_movimiento = 0.5f;
    public float fuerza_salto = 5f;
    public float fuerza_dash = 8f;       // Intensidad del dash
    public float cooldown_dash = 2f;      // Tiempo de espera entre dashes
    public LayerMask capaSuelo;
    public Transform chequeoSuelo;
    public float radioChequeo = 0.2f;

    private float _velocidad_por_fotograma = 0.0f;
    private EstadosMovimiento estado_actual = EstadosMovimiento.quieto;

    private Rigidbody rigid_body;
    private PlayerInput entradas_del_jugador;
    private InputAction movimiento;
    private InputAction saltar;
    private InputAction dash;

    private bool puede_dashear = true;

    void Start() {
        entradas_del_jugador = GetComponent<PlayerInput>();
        rigid_body = GetComponent<Rigidbody>();

        movimiento = entradas_del_jugador.actions.FindAction("movimiento");
        saltar = entradas_del_jugador.actions.FindAction("Saltar");
        dash = entradas_del_jugador.actions.FindAction("Dash"); 

        _velocidad_por_fotograma = velocidad_movimiento / 60;

        saltar.performed += ctx => IntentarSaltar();
        dash.performed += ctx => IntentarDash();
    }

    void FixedUpdate() {
        Vector2 direccion = movimiento.ReadValue<Vector2>();

       if (EstaEnSuelo() && estado_actual == EstadosMovimiento.saltando) {
    cambiar_estado(EstadosMovimiento.quieto);
}

        if (direccion.magnitude > 0.1f && estado_actual != EstadosMovimiento.dash) {
            cambiar_estado(EstadosMovimiento.caminando);    
        } else if (EstaEnSuelo() && estado_actual != EstadosMovimiento.dash) {
            cambiar_estado(EstadosMovimiento.quieto);
        }

        avanzar(direccion);
    }

    void avanzar(Vector2 direccion_joystick) {
        //if (estado_actual == EstadosMovimiento.dash) return; // no mover normal durante dash

        Vector3 hacia_adelante = transform.forward * direccion_joystick.y;
        hacia_adelante += transform.right * direccion_joystick.x;

        rigid_body.MovePosition(transform.position + ((hacia_adelante * direccion_joystick.magnitude) * _velocidad_por_fotograma));
    }

    void IntentarSaltar() {
        if (EstaEnSuelo()) {
            rigid_body.AddForce(Vector3.up * fuerza_salto, ForceMode.Impulse);
            cambiar_estado(EstadosMovimiento.saltando);
        }
    }

      bool EstaEnSuelo() {
        return Physics.CheckSphere(chequeoSuelo.position, radioChequeo, capaSuelo);
    }

    void IntentarDash() {
        if (puede_dashear) {
            cambiar_estado(EstadosMovimiento.dash);
            Vector3 direccion = transform.forward; // dash hacia adelante
            rigid_body.AddForce(direccion * fuerza_dash, ForceMode.Impulse);
            puede_dashear = false;
            StartCoroutine(ResetearCooldown());
        }
    }

    IEnumerator ResetearCooldown() {
        yield return new WaitForSeconds(cooldown_dash);
        puede_dashear = true;
        cambiar_estado(EstadosMovimiento.quieto);
    }

  

    void cambiar_estado(EstadosMovimiento estado_nuevo) {
        estado_actual = estado_nuevo;
    }



    void OnDrawGizmosSelected() {
    if (chequeoSuelo != null) {
        Gizmos.color = Color.red; // color de la esfera
        Gizmos.DrawWireSphere(chequeoSuelo.position, radioChequeo);
    }
}
}