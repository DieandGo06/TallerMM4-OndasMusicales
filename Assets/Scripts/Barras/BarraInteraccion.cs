using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class BarraInteraccion : MonoBehaviour
{
    public BarreraData data;
    public BarraMovimiento movimiento;

    private void Awake()
    {
        movimiento = GetComponent<BarraMovimiento>();
    }

    private void Start()
    {
        GetValoresEstadoA();
    }

    private void FixedUpdate()
    {
        SetVariablesLimits();

        //Desinteres
        if (GameManager.instance.pantallas == GameManager.Pantallas.Desinteres)
        {
            if (!movimiento.esProtagonista)
            {
                if (GameManager.instance.hayInterccion)
                {
                    GetValoresEstadoB();
                    movimiento.MovimientoManual();//Cuando hay una interracion, mov manual
                    movimiento.estado = BarraMovimiento.Estado.manual;
                    return;
                }
                else
                {
                    if (movimiento.estado == BarraMovimiento.Estado.manual)
                    {
                        GetValoresEstadoA();
                        movimiento.estado = BarraMovimiento.Estado.baja;
                        movimiento.velocidadBajada = 10f;
                    }
                    if (movimiento.estado == BarraMovimiento.Estado.sube && movimiento.velocidadBajada != data.velocidadBajada_A)
                    {
                        GetValoresEstadoA();
                    }
                    StartCoroutine(movimiento.MovimientoAutomatico());//Siempre que no haya interaccion, mov automatico
                }
            }

            if (movimiento.esProtagonista)
            {
                if (GameManager.instance.hayInterccion)
                {
                    GetValoresEstadoB();
                }
                else
                {
                    GetValoresEstadoA();
                }
                //Siempre se mueve en automatico, ignora la interaccion
                StartCoroutine(movimiento.MovimientoAutomatico());
            }
        }

        //Vanidad
        if (GameManager.instance.pantallas == GameManager.Pantallas.Vanidad)
        {
            if (GameManager.instance.hayInterccion)
            {
                if (movimiento.estado == BarraMovimiento.Estado.manual)
                {
                    GetValoresEstadoB();
                }
            }

            if (GameManager.instance.hayInterccion == false && movimiento.estado == BarraMovimiento.Estado.manual)
            {
                Tareas.Nueva(2, () =>
                {
                    movimiento.estado = BarraMovimiento.Estado.baja;
                    GetValoresEstadoA();
                });

            }


            //Estas 3 lineas se pueden comentar si se logra hacer puntual los cambios
            if (GameManager.instance.hayInterccion)
            {
                GetValoresEstadoB();
                movimiento.estado = BarraMovimiento.Estado.manual;
                if (movimiento.esProtagonista) movimiento.heightTarget = movimiento.alturaMax;
                else movimiento.heightTarget = GameManager.instance.punteroPosition.y / 2;
            }
            else
            {
                Tareas.Nueva(2, () =>
                {
                    movimiento.estado = BarraMovimiento.Estado.baja;
                    GetValoresEstadoA();
                });
            }
            //---------------------------------------------------------------------------------------------------------------

            if (movimiento.estado == BarraMovimiento.Estado.manual) movimiento.MovimientoManual();
            else StartCoroutine(movimiento.MovimientoAutomatico());
            return;

        }

    }



    void SetVariablesLimits()
    {
        //Altura Minima
        if (data.alturaMin_B >= data.alturaMin_A)
        {
            movimiento.alturaMin = Mathf.Clamp(movimiento.alturaMin, data.alturaMin_A, data.alturaMin_B);
        }
        else movimiento.alturaMin = Mathf.Clamp(movimiento.alturaMin, data.alturaMin_B, data.alturaMin_A);

        //Altura Maxima
        if (data.alturaMax_B >= data.alturaMax_A)
        {
            movimiento.alturaMax = Mathf.Clamp(movimiento.alturaMax, data.alturaMax_A, data.alturaMax_B);
        }
        else movimiento.alturaMax = Mathf.Clamp(movimiento.alturaMax, data.alturaMax_B, data.alturaMax_A);
    }




    void RegresoProgresivoAlEstado_A()
    {
        float difAlturaMinima = data.alturaMin_B - data.alturaMin_A;
        float difAlturaMaxima = data.alturaMax_B - data.alturaMax_A;
    }

    [Button(Spacing = ButtonSpacing.Before)]
    public void GetValoresEstadoA()
    {
        movimiento.alturaMin = data.alturaMin_A;
        movimiento.alturaMax = data.alturaMax_A;
        movimiento.velocidadSubida = data.velocidadSubida_A;
        movimiento.velocidadBajada = data.velocidadBajada_A;
        movimiento.rangoVariacionAltura = data.rangoVariacionAltura_A;
        movimiento.tiempoSostenidoEnMinimo = data.tiempoSostenidoEnMinimo_A;
        movimiento.tiempoSostenidoEnMaximo = data.tiempoSostenidoEnMaximo_A;

        //Variacion de altura
        if (movimiento.rangoVariacionAltura == 0) movimiento.variacionAltura = 0;
        else movimiento.variacionAltura = Random.Range(0, movimiento.rangoVariacionAltura);
    }
    [Button]
    public void GetValoresEstadoB()
    {
        movimiento.alturaMin = data.alturaMin_B;
        movimiento.alturaMax = data.alturaMax_B;
        movimiento.velocidadSubida = data.velocidadSubida_B;
        movimiento.velocidadBajada = data.velocidadBajada_B;
        movimiento.rangoVariacionAltura = data.rangoVariacionAltura_B;
        movimiento.tiempoSostenidoEnMinimo = data.tiempoSostenidoEnMinimo_B;
        movimiento.tiempoSostenidoEnMaximo = data.tiempoSostenidoEnMaximo_B;

        //Variacion de altura
        if (movimiento.rangoVariacionAltura == 0) movimiento.variacionAltura = 0;
        else movimiento.variacionAltura = Random.Range(0, movimiento.rangoVariacionAltura);
    }
}
