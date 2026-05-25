using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EstadosMovimiento {
    quieto,
    caminando,
    dash,
    saltando // NUEVO: Añadimos el estado de salto por si lo necesitas después
}

[RequireComponent(typeof(Rigidbody))]
public class jugador_movimiento : MonoBehaviour {
    [Header("Configuración de Movimiento")]
    public float velocidad_movimiento = 0.5f;
    public float fuerza_dash = 8f;       
    public float cooldown_dash = 2f;      

    [Header("Configuración del Salto")] 
    public float fuerza_salto = 5f; 
    [SerializeField] private Transform chequeoSuelo; 
    [SerializeField] private float radioChequeo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;  

    private float _velocidad_por_fotograma = 0.0f;
    private EstadosMovimiento estado_actual = EstadosMovimiento.quieto;

    private Rigidbody rigid_body;
    private PlayerInput entradas_del_jugador;
    private InputAction movimiento;
    private InputAction saltar;
    private InputAction dash;

    private bool puede_dashear = true;

    private Animator animator;

    void Start() {
        entradas_del_jugador = GetComponent<PlayerInput>();
        rigid_body = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();

        movimiento = entradas_del_jugador.actions.FindAction("movimiento");
        dash = entradas_del_jugador.actions.FindAction("Dash"); 
        saltar = entradas_del_jugador.actions.FindAction("Saltar"); // NUEVO: Buscar la acción "Saltar" en tu Input Action Asset

        _velocidad_por_fotograma = velocidad_movimiento / 60;

        dash.performed += ctx => IntentarDash();
        saltar.performed += ctx => IntentarSaltar(); // NUEVO: Escuchar cuando se presione el botón de salto
    }

   void FixedUpdate() {
        Vector2 direccion = movimiento.ReadValue<Vector2>();

        if (animator != null) {
            animator.SetFloat("VelocidadX", direccion.x);
            animator.SetFloat("VelocidadY", direccion.y);
        }

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            if (direccion.x < -0.1f)
            {
                spriteRenderer.flipX = true; 
            }
            else if (direccion.x > 0.1f)
            {
                spriteRenderer.flipX = false; 
            }
        }

        if (direccion.magnitude > 0.1f && estado_actual != EstadosMovimiento.dash) {
            cambiar_estado(EstadosMovimiento.caminando);    
        } else if (EstaEnSuelo() && estado_actual != EstadosMovimiento.dash) {
            cambiar_estado(EstadosMovimiento.quieto);
        }

        avanzar(direccion);
    }

    void avanzar(Vector2 direccion_joystick) {
        Vector3 hacia_adelante = transform.forward * direccion_joystick.y;
        hacia_adelante += transform.right * direccion_joystick.x;

        rigid_body.MovePosition(transform.position + ((hacia_adelante * direccion_joystick.magnitude) * _velocidad_por_fotograma));
    }

    // NUEVO: Método que se ejecuta al presionar el botón de salto
    void IntentarSaltar() {
        // Solo saltamos si tocamos el suelo
        if (EstaEnSuelo()) {
            // Reproducir sonido de brinco usando tu AudioManager
            if (AudioManager.Instance != null) {
                AudioManager.Instance.Play(AudioManager.SoundType.Brinco);
            }

            // Aplicamos fuerza hacia arriba (Vector3.up) de tipo Impulso
            rigid_body.AddForce(Vector3.up * fuerza_salto, ForceMode.Impulse);
        }
    }

    bool EstaEnSuelo() {
        // Evita errores en la consola si olvidas asignar el Transform en el Inspector
        if (chequeoSuelo == null) return false; 
        
        return Physics.CheckSphere(chequeoSuelo.position, radioChequeo, capaSuelo);
    }

    void IntentarDash() {
        if (puede_dashear) {
            if (AudioManager.Instance != null) {
                AudioManager.Instance.Play(AudioManager.SoundType.Dash);
            }
            
            cambiar_estado(EstadosMovimiento.dash);
            Vector2 direccionInput = movimiento.ReadValue<Vector2>();
            Vector3 direccionDash = Vector3.zero;
            if (direccionInput.magnitude > 0.1f) {
                
                direccionDash = transform.forward * direccionInput.y + transform.right * direccionInput.x;
                direccionDash.Normalize(); 
            } 

            else {
                direccionDash = transform.forward;
            }

            rigid_body.AddForce(direccionDash * fuerza_dash, ForceMode.Impulse);
            
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
}