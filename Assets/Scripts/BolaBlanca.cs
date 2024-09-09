using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BolaBlanca : MonoBehaviour
{
    private Vector3 posicionInicial;
    Rigidbody rb;
    GameManager gameManager;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicionInicial = new Vector3(5.5f, 1.535772f, 0.25f);
        transform.position = posicionInicial;
        gameManager = FindObjectOfType<GameManager>();
    }
    private void FixedUpdate()
    {
        if (transform.position.y < 1.37f)
        {
            //Llama al metodo necesario para cambiar de turno
            gameManager.meteBolaBlanca();

            //Rehubica la bola en posicion
            transform.position = posicionInicial;
            rb.velocity = Vector3.zero; // Para detener la bola
            rb.angularVelocity = Vector3.zero; // Y su rotacion
            rb.constraints = RigidbodyConstraints.FreezePositionY; // Para que no pueda saltar de la mesa 
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lisas" || collision.gameObject.tag == "Rayadas" || collision.gameObject.tag == "Negra")
        {
            gameManager.bolaGolpeada();
        }
    }*/
}
