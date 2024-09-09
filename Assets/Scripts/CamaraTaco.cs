using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamaraTaco : MonoBehaviour
{
    public float velocidadMovimiento;
    public Transform bolaBlanca;
    public Vector3 distanciaCam;
    public Transform taco;
    public GameObject taco1;
    public Vector3 offsetTaco;
    public int fuerzaMaxima;
    private float fuerzActual;
    private float movimientoHorizontal;
    private GameObject camarasGO; // Acceder al GameObject vacio con camaras
    private Camaras camaras; // Variable del tipo script camaras
    public Slider barraFuerza;
    GameManager gamemanager;
    void Start()
    {
        reseteoCamara();
        camarasGO = GameObject.FindGameObjectWithTag("CAMARAS");
        camaras = camarasGO.GetComponent<Camaras>(); // Accede al script
        barraFuerza.minValue = 0;
        barraFuerza.maxValue = fuerzaMaxima;
        barraFuerza.value = 0;
        gamemanager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        movimientoHorizontal = Input.GetAxis("Mouse X") * velocidadMovimiento * Time.deltaTime;
        transform.RotateAround(bolaBlanca.transform.position, Vector3.up, movimientoHorizontal);

        if (Input.GetKey(KeyCode.Mouse0)) // Mantener presionando el click para que cargue la barra
        {
            fuerzActual = fuerzActual + Time.deltaTime * 20;
            fuerzActual = Mathf.Clamp(fuerzActual, 0, fuerzaMaxima); // Limita la fuerza
            barraFuerza.value = fuerzActual; // Mantiene actualizada la barra            
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 direccion = transform.forward; // Que dispare a donde apunta la camara (y el taco)
            direccion = new Vector3(direccion.x, 0, direccion.z).normalized; // Ignorar y, para que no salga volando
            bolaBlanca.gameObject.GetComponent<Rigidbody>().AddForce(direccion * fuerzActual, ForceMode.Impulse);

            fuerzActual = 0; // Reestablece los valores cuando se suelta el click
            barraFuerza.value = 0; // Reestablece los valores cuando se suelta el click
            camaras.cambioCamaras();
            taco1.SetActive(false);
            barraFuerza.gameObject.SetActive(false); // Para desactivar la barra de fuerza en la cam aerea, tuve que usar gameobject,
                                                     // porque al ser slider no me permitia usar solamente el setactive 
            RegistrarGolpeo(); // LLama al metodo del gamemanager, para que habilite su update
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            reseteoCamara();
            taco1.SetActive(true);
            barraFuerza.gameObject.SetActive(true);
        }

    }
    public void reseteoCamara() 
    {
        transform.position = bolaBlanca.position + distanciaCam; // Posicion de la camara con respecto a la bola Blanca
        transform.LookAt(bolaBlanca.position); // Para que la camara mire a la bola blanca
        transform.localEulerAngles = new Vector3(3f, transform.localEulerAngles.y, 0); // Rotacion de camara
        alinearTaco();
    }
    public void alinearTaco()
    {
        taco.position = transform.position + transform.rotation * offsetTaco; // Posicion del taco respecto a la cam
        taco.LookAt(bolaBlanca.transform); // Para que el taco mire a la bola blanca (igual que la camara)
    }
    private void RegistrarGolpeo()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.golpeoBolas();
        }
    }
}
