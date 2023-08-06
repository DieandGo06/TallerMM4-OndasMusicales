using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimidezManager : MonoBehaviour
{
    public static TimidezManager instance;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.puntero.Pressed.AddListener(() => GameManager.instance.CambiarFondo(GameManager.instance.fondoB));
        GameManager.instance.puntero.Pressed.AddListener(PunteroPressed);

        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        GameManager.instance.puntero.PunteroTriggerStay.AddListener(ReorganizarBarras);
    }

    private void Update()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (!GameManager.instance.hayInterccion)
            {
                if (barra.esProtagonista)
                {
                    barra.GetValoresEstadoA();
                }
                else
                {
                    //Recien terminar la interaccion
                    if (barra.movimiento == Barra.Movimiento.manual)
                    {
                        barra.movimiento = Barra.Movimiento.automatico;
                        barra.direccion = Barra.Direccion.baja;
                        barra.GetValoresEstadoA();
                        barra.velocidadBajada = 8f;
                        barra.estado = Barra.Estado.transicion;
                        return;
                    }
                    //Tras la bajada acelerada al terminar la interaccion
                    if (barra.direccion == Barra.Direccion.sube && barra.estado == Barra.Estado.transicion)
                    {
                        barra.GetValoresEstadoA();
                    }
                }
            }
        }
    }


    void PunteroPressed()
    {
        foreach (Barra barra in GameManager.instance.barras)
        {
            barra.GetValoresEstadoB();
            barra.movimiento = Barra.Movimiento.manual;
        }
    }

    void ReorganizarBarras(Barra barraColisionada)
    {
        //Barra Principal
        foreach (Barra barra in GameManager.instance.barras)
        {
            if (barra != GameManager.instance.protagonista)
            {
                barra.heightTarget = GameManager.instance.punteroPosition.y;
            }
            else
            {
                barra.heightTarget = -1 - GameManager.instance.punteroPosition.y;
            }
        }
    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }

}
