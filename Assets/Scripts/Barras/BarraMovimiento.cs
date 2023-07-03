using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class BarraMovimiento : MonoBehaviour
{
    public enum Estado { sube, baja, sostenido, manual };
    public enum EstadoInteraccion { estado_A, estado_B };

    [Header("Estados")]
    public EstadoInteraccion estadoInteraccion;
    public Estado estado;
    public float alturaActual;
    public float heightTarget;
    public bool esProtagonista;

    [Space(10), Header("Basicos")]
    [Range(-5.5f, 4.5f)] public float alturaMin;
    [Range(-5.5f, 4.5f)] public float alturaMax;
    [Range(0, 20)] public float velocidadSubida;
    [Range(0, 20)] public float velocidadBajada;
    [Range(0, 0.5f)] public float rangoVariacionAltura;

    [Space(10)]
    [Header("Avanzados")]
    [Range(0, 2)] public float tiempoSostenidoEnMinimo;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo;


    Rigidbody2D rBody;
    //private float alturaMinimaPantalla = -8;
    [HideInInspector] public float variacionAltura;


    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //General
        estado = Estado.sube;

        //Protagonista
        if (esProtagonista && GameManager.instance.protoganista == null)
        {
            GameManager.instance.protoganista = gameObject;
            transform.name = "Protagonista";
        }

        //Debuggers
        if (alturaMin > alturaMax)
        {
            Debug.LogError("la altura min de " + transform.name + "no puede ser mayor que la altura max");
        }
    }


    public void MovimientoManual()
    {
        if (heightTarget < alturaMin) heightTarget = alturaMin;
        if (heightTarget > alturaMax) heightTarget = alturaMax;

        float factor = 0.9f;//0 to 1
        float newPosY = heightTarget * (1 - factor) + transform.position.y * factor;
        rBody.MovePosition(new Vector2(transform.position.x, newPosY));
    }


    public IEnumerator MovimientoAutomatico()
    {
        if (estado == Estado.sube)
        {
            //Tiempo sostenido en la altura maxima
            if (transform.position.y >= alturaMax + variacionAltura)
            {
                estado = Estado.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMaximo);
                estado = Estado.baja;
                yield break;
            }
            //Movimiento 
            rBody.MovePosition(transform.position + (velocidadSubida * Vector3.up * Time.deltaTime));
            if (estado == Estado.baja) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }

        if (estado == Estado.baja)
        {
            //Tiempo sostenido en la altura minima
            if (transform.position.y <= alturaMin + variacionAltura)
            {
                estado = Estado.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMinimo);
                estado = Estado.sube;
                yield break;
            }
            //Movimiento
            rBody.MovePosition(transform.position - (velocidadBajada * Vector3.up * Time.deltaTime));
            if (estado == Estado.sube) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }
        yield return null;
    }





    #region Botones de Editor
    [Button(Spacing = ButtonSpacing.Before)]
    void PosicionarEnAlturaMinima()
    {
        PosicionarHijo();
        transform.position = new Vector3(transform.position.x, alturaMin, transform.position.z);
    }

    [Button]
    void PosicionarEnAlturaMaxima()
    {
        PosicionarHijo();
        transform.position = new Vector3(transform.position.x, alturaMax, transform.position.z);

    }

    void PosicionarHijo()
    {
        Transform child = transform.GetChild(0).transform;
        child.localPosition = new Vector3(0, -4.5f, 0);
    }
    #endregion
}
