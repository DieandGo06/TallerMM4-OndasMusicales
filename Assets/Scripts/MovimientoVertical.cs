using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    public enum Estado { sube, baja, sostenido };

    [Header("Basicos")]
    public Estado estado;
    [Range(0, 20)] public float velocidadSubida;
    [Range(0, 20)] public float velocidadBajada;
    [Range(-5.5f, 4.5f)] public float alturaMinima;
    [Range(-5.5f, 4.5f)] public float alturaMaxima;
    [Range(0, 0.5f)] public float rangoVariacionAltura;

    [Space(10)]
    [Header("Avanzados")]
    //[Range(-2, 2)] public float aceleracionInicialSubida;
    [Range(0, 2)] public float tiempoSostenidoEnMinimo;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo;



    Rigidbody2D rBody;
    //private float alturaMinimaPantalla = -8;
    [HideInInspector] public float variacionAltura;
    //private float duracionDeAceleracionInicial = 0.5f;
    private float cronometro;



    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        estado = Estado.sube;
        if (rangoVariacionAltura == 0) variacionAltura = 0;
        else variacionAltura = Random.Range(-rangoVariacionAltura, rangoVariacionAltura);
    }

    private void FixedUpdate()
    {
        StartCoroutine(Movimiento());
    }


    IEnumerator Movimiento()
    {
        if (estado == Estado.sube)
        {
            //Acelaracion Inicial
            //if ()

            //Tiempo sostenido en la altura maxima
            if (transform.position.y >= alturaMaxima + variacionAltura)
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
            if (transform.position.y <= alturaMinima + variacionAltura)
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

    void Acelerar()
    {

    }


    #region Botones de Editor
    [Button]
    void PosicionarHijo()
    {
        Transform child = transform.GetChild(0).transform;
        child.localPosition = new Vector3(0, -(child.localScale.y / 2), 0);
    }

    [Button]
    void PosicionarEnAlturaMinima()
    {
        transform.position = new Vector3(transform.position.x, alturaMinima, transform.position.z);
    }

    [Button]
    void PosicionarEnAlturaMedia()
    {
        transform.position = new Vector3(transform.position.x, (alturaMinima + alturaMaxima) / 2, transform.position.z);
    }

    [Button]
    void PosicionarEnAlturaMaxima()
    {
        transform.position = new Vector3(transform.position.x, alturaMaxima, transform.position.z);
    }
    #endregion
}
