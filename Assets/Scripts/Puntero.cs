using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Puntero : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rb;
    bool isMousePressed;
    public Vector2 posicion;
    
    public UnityEvent<Collider2D> punteroColisiono;
    public UnityEvent<Collider2D> punteroMantieneColision;
    public UnityEvent<Collider2D> punteroColisionTerminada;




    private void Awake()
    {
        punteroColisiono = new UnityEvent<Collider2D>();
        punteroMantieneColision = new UnityEvent<Collider2D>();
        punteroColisionTerminada = new UnityEvent<Collider2D>();
    }

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            isMousePressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            GameManager.instance.hayInterccion = false;
        }

        if (isMousePressed)
        {
            posicion = cam.ScreenToWorldPoint(Input.mousePosition);
            GameManager.instance.punteroPosition = posicion;
            GameManager.instance.hayInterccion = true;
            rb.MovePosition(posicion);
        }
        
#endif

#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 position = cam.ScreenToWorldPoint(touch.position);
            rb.MovePosition(position);
        }
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        punteroColisiono.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        punteroMantieneColision.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        punteroColisionTerminada.Invoke(collision);
    }
}
