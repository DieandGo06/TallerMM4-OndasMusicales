using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimidezManager : MonoBehaviour
{
    public static TimidezManager instance;
    IEnumerator[] ContadorBajarBarrera;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //    GameManager.instance.puntero.Pressed.AddListener(() => GameManager.instance.CambiarFondo(GameManager.instance.fondoB));
        //    GameManager.instance.puntero.Pressed.AddListener(PunteroPressed);

        //    GameManager.instance.puntero.Released.AddListener(RegresarFondoA);
        //    GameManager.instance.puntero.PunteroTriggerStay.AddListener(ReorganizarBarras);
        ContadorBajarBarrera = new IEnumerator[GameManager.instance.barras.Count];
        GameManager.instance.puntero.PunteroTriggerEnter.AddListener(ColisionEnter);
        GameManager.instance.puntero.PunteroTriggerExit.AddListener(ColisionExit);


    }

    private void Update()
    {
        //    foreach (Barra barra in GameManager.instance.barras)
        //    {
        //        if (!GameManager.instance.hayInterccion)
        //        {
        //            if (barra.esProtagonista)
        //            {
        //                barra.GetValoresEstadoA();
        //            }
        //            else
        //            {
        //                //Recien terminar la interaccion
        //                if (barra.movimiento == Barra.Movimiento.manual)
        //                {
        //                    barra.movimiento = Barra.Movimiento.automatico;
        //                    barra.direccion = Barra.Direccion.baja;
        //                    barra.GetValoresEstadoA();
        //                    barra.velocidadBajada = 8f;
        //                    barra.estado = Barra.Estado.transicion;
        //                    return;
        //                }
        //                //Tras la bajada acelerada al terminar la interaccion
        //                if (barra.direccion == Barra.Direccion.sube && barra.estado == Barra.Estado.transicion)
        //                {
        //                    barra.GetValoresEstadoA();
        //                }
        //            }
        //        }
    }



    void PunteroPressed()
    {

    }

    void ColisionEnter(Barra barraColisionada)
    {
        LevantarBarra(barraColisionada);
        //StopCoroutine(ContadorBajarBarrera[barraColisionada.index]);
        //ContadorBajarBarrera[barraColisionada.index] = null;
    }

    void ColisionExit(Barra barraColisionada)
    {
        if (ContadorBajarBarrera[barraColisionada.index] == null)
        {
            ContadorBajarBarrera[barraColisionada.index] = BajarBarrera(barraColisionada);
            StartCoroutine(ContadorBajarBarrera[barraColisionada.index]);
            ContadorBajarBarrera[barraColisionada.index] = null;
        }

    }







    void LevantarBarra(Barra barraColisionada)
    {
        if (!barraColisionada.esProtagonista)
        {
            barraColisionada.GetValoresEstadoB();
            barraColisionada.direccion = Barra.Direccion.sube;
            barraColisionada.velocidadSubida = 8f;

            barraColisionada.AtHighestPoint.AddListener(() =>
            {
                barraColisionada.GetValoresEstadoB();
                barraColisionada.AtHighestPoint.RemoveAllListeners();
            });
        }
    }

    IEnumerator BajarBarrera(Barra barraColisionada)
    {
        yield return new WaitForSeconds(2f);
        barraColisionada.GetValoresEstadoA();
        barraColisionada.direccion = Barra.Direccion.baja;
        barraColisionada.movimiento = Barra.Movimiento.automatico;
        barraColisionada.velocidadBajada = 8f;

        barraColisionada.AtLowestPoint.AddListener(() =>
        {
            barraColisionada.GetValoresEstadoA();
            barraColisionada.AtLowestPoint.RemoveAllListeners();
        });
    }

    void CancelarCorrutinas()
    {
        StopCoroutine("LevantarBarras");
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
