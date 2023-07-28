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

    [HideInInspector] public UnityEvent PunteroPressed;
    [HideInInspector] public UnityEvent<Barra> PunteroTriggerEnter;
    [HideInInspector] public UnityEvent<Barra> PunteroTriggerStay;
    [HideInInspector] public UnityEvent<Barra> PunteroTriggerExit;




    private void Awake()
    {
        PunteroPressed = new UnityEvent();
        PunteroTriggerEnter = new UnityEvent<Barra>();
        PunteroTriggerStay = new UnityEvent<Barra>();
        PunteroTriggerExit = new UnityEvent<Barra>();
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
            PunteroPressed.Invoke();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMousePressed = false;
            transform.position = new Vector3(-10, 0, 0);
            GameManager.instance.hayInterccion = false;
            GameManager.instance.RegresarColorFondo();
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
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerEnter.Invoke(barraScript);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerStay.Invoke(barraScript);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Barra>() != null)
        {
            Barra barraScript = collision.GetComponent<Barra>();
            PunteroTriggerExit.Invoke(barraScript);
        }
    }
}
