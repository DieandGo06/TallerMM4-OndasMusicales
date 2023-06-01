using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class MovimientoVertical : MonoBehaviour
{
    public enum Estado { sube, baja, sostenido };

    [Header("Basicos")]
    public Estado estado;
    [Range(0, 20)] public float velocidad;
    [Range(-5.5f, 4.5f)] public float alturaMinima;
    [Range(-5.5f, 4.5f)] public float alturaMaxima;
    [Range(0, 0.5f)] public float rangoVariacionAltura;

    [Space(10)]
    [Header("Avanzados")]
    [Range(0, 2)] public float tiempoSostenidoEnMinimo;
    [Range(0, 2)] public float tiempoSostenidoEnMaximo;


    Rigidbody2D rBody;
    private float alturaRelativa;
    private float variacionAltura;
    private float alturaMinimaPantalla = -8;



    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        estado = Estado.sube;
    }

    private void FixedUpdate()
    {
        StartCoroutine(Movimiento());
    }


    IEnumerator Movimiento()
    {
        if (estado == Estado.sube)
        {
            if (transform.position.y >= alturaMaxima + variacionAltura)
            {
                estado = Estado.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMaximo);
                variacionAltura = Random.Range(-rangoVariacionAltura, rangoVariacionAltura);
                estado = Estado.baja;
                yield break;
            }
            rBody.MovePosition(transform.position + (velocidad * Vector3.up * Time.deltaTime));
            if (estado == Estado.baja) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }

        if (estado == Estado.baja)
        {
            if (transform.position.y <= alturaMinima + variacionAltura)
            {
                estado = Estado.sostenido;
                yield return new WaitForSeconds(tiempoSostenidoEnMinimo);
                variacionAltura = Random.Range(-rangoVariacionAltura, rangoVariacionAltura);
                estado = Estado.sube;
                yield break;
            }
            rBody.MovePosition(transform.position - (velocidad * Vector3.up * Time.deltaTime));
            if (estado == Estado.sube) Debug.LogWarning("ESTE MENSAJE NO DEBRIA VERSE");
            yield break;
        }
        yield return null;
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
