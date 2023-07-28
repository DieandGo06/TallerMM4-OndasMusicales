using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpatiaManager : MonoBehaviour
{
    public int protaCounterAtLowestPoint;
    public Barra leftBarraToProta;
    public Barra rightBarraToProta;
    Barra prota;





    void Start()
    {
        prota = GameManager.instance.protoganista;
        GetBarrasNextToProta();

        GameManager.instance.puntero.Released.AddListener(RegresarFondoA);

        prota.AtLowestPoint.AddListener(() =>
        {
            if (GameManager.instance.hayInterccion)
            {
                if (protaCounterAtLowestPoint == 0)
                {
                    prota.GetValoresEstadoB();
                    leftBarraToProta.GetValoresEstadoB_From(prota.data);
                    rightBarraToProta.GetValoresEstadoB_From(prota.data);
                    leftBarraToProta.movimiento = Barra.Movimiento.manual;
                    rightBarraToProta.movimiento = Barra.Movimiento.manual;
                }
                protaCounterAtLowestPoint++;
                GameManager.instance.CambiarFondo(GameManager.instance.fondoB);
            }
            else if (protaCounterAtLowestPoint >= 0) prota.GetValoresEstadoA();
        });

        prota.AtHighestPoint.AddListener(() =>
        {
            if (!GameManager.instance.hayInterccion)
            {
                if (protaCounterAtLowestPoint > 0)
                {
                    leftBarraToProta.GetValoresEstadoA();
                    rightBarraToProta.GetValoresEstadoA();
                    protaCounterAtLowestPoint = 0;
                }
            }
        });


        prota.StartGoingUp.AddListener(() =>
        {
            if (protaCounterAtLowestPoint == 1)
            {
                ChangeMovmientoBarrasLaterales();
            }
        });
    }

    void FixedUpdate()
    {
        if (leftBarraToProta.movimiento == Barra.Movimiento.manual) leftBarraToProta.heightTarget = prota.transform.position.y;
        if (rightBarraToProta.movimiento == Barra.Movimiento.manual) rightBarraToProta.heightTarget = prota.transform.position.y;
    }

    void GetBarrasNextToProta()
    {
        int index = GameManager.instance.barras.FindIndex(a => a == prota);
        leftBarraToProta = GameManager.instance.barras[index - 1];
        rightBarraToProta = GameManager.instance.barras[index + 1];
    }

    void ChangeMovmientoBarrasLaterales()
    {
        rightBarraToProta.movimiento = Barra.Movimiento.automatico;
        leftBarraToProta.movimiento = Barra.Movimiento.automatico;
        rightBarraToProta.direccion = Barra.Direccion.sube;
        leftBarraToProta.direccion = Barra.Direccion.sube;
    }

    void RegresarFondoA()
    {
        GameManager.instance.CambiarFondo(GameManager.instance.fondoA);
    }
}
