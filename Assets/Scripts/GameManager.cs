using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    enum JugadorActual { Jugador1, Jugador2 } // Lista de constantes 
    JugadorActual jugadorActual;
    bool tiroFinalJ1 = false;
    bool tiroFinalJ2 = false;
    int j1BolasRestantes = 7;
    int j2BolasRestantes = 7;
    public Camaras cambio;
    private bool jugador1EsLisas = false;
    private bool jugador2EsLisas = false;
    // Variables para el paso de turno 
    bool tiempoEsperaPorBolas = true;
    bool gameOver = false;
    public float tiempoDisparo = 3f;
    private float tiempoActual;
    bool mantieneTurno = false;
    bool cambiaJugador = false; // DESCARTADA
    CamaraTaco tiro;
    // Variables para saber si el jugador dio bola o no
    bool dioBola = false;
    bool disparoHecho = false;

    public TextMeshProUGUI j1Bolas;
    public TextMeshProUGUI j2Bolas;
    public TextMeshProUGUI turnoActual;
    public TextMeshProUGUI mensajeGrande;
    public TextMeshProUGUI tipoDeBolas;

    void Start()
    {
        tiro = GameObject.FindWithTag("MainCamera").GetComponent<CamaraTaco>();
        jugadorActual = JugadorActual.Jugador1;
        tiempoActual = tiempoDisparo;
    }
    private void Update()
    {
        if (tiempoEsperaPorBolas == true && gameOver == false)
        {
            if (disparoHecho == false)
            {
                return; // No hace nada si no se ha disparado
            }
            tiempoActual = tiempoActual - Time.deltaTime;
            if (tiempoActual > 0)
            {
                return; // Espera antes de seguir
            }
            bool todasDetenidas = true;
            List<GameObject> todasLasBolas = new List<GameObject>();
            todasLasBolas.AddRange(GameObject.FindGameObjectsWithTag("Lisas"));
            todasLasBolas.AddRange(GameObject.FindGameObjectsWithTag("Rayadas"));
            todasLasBolas.AddRange(GameObject.FindGameObjectsWithTag("Negra"));
            todasLasBolas.AddRange(GameObject.FindGameObjectsWithTag("Blanca"));

            foreach (GameObject bola in todasLasBolas) // Bucle para revisar si las bolas estan quietas
            {
                if (bola.GetComponent<Rigidbody>().velocity.magnitude >= 0.01f) // Muy pequeno margen de movimiento
                {
                    todasDetenidas = false;
                    break;
                }
            }
            if (todasDetenidas == true) // Solo se ejecuta cuando todas estan detenidas
            {
                print("se detuverion todas las bolas");
                if (dioBola)
                {
                    tiempoEsperaPorBolas = false;
                    turnoSiguiente();
                }
                else
                {
                    tiempoEsperaPorBolas = false;
                    turnoSiguiente();
                }
                dioBola = false;
                tiempoActual = tiempoDisparo;
                //turnoSiguiente();
            }
        }
    }
    private void OnCollisionEnter(Collision collision) // Este script va al collider donde caen las bolas
    {
        if (collision.gameObject.tag == "Lisas" || collision.gameObject.tag == "Rayadas" || collision.gameObject.tag == "Negra")
        {
            //print("Hubo colison");
            if (revisarBolas(collision.gameObject.GetComponent<Bolas>()))
            {
                Destroy(collision.gameObject);
            }
        }
    }
    bool revisarBolas(Bolas bola) // Revisa sobre el script Bolas
    {
        if (bola.esBolaBlanca())
        {
            if (meteBolaBlanca())
            {
                mantieneTurno = false;
                meteBolaBlanca();
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (bola.esBolaNegra())
        {
            if (jugadorActual == JugadorActual.Jugador1)
            {
                if (tiroFinalJ1)
                {
                    gano("Jugador 1");
                }
            }
            else
            {
                if (tiroFinalJ2)
                {
                    gano("Jugador 2");
                }
            }
            bolaNegraAntesDeTiempo();
            return true;
        }
        else // Si es lisa o rayada
        {
            bool esBolaLisa = bola.esBolaLisa();
            if (!jugador1EsLisas && !jugador2EsLisas)
            {
                if (jugadorActual == JugadorActual.Jugador1)
                {
                    actualizarTipoDeBolas(esBolaLisa); // Asigna el tipo de bola al Jugador 1
                    jugador2EsLisas = !esBolaLisa; // Asigna el tipo de bola al Jugador 2
                }
                else if (jugadorActual == JugadorActual.Jugador2)
                {
                    actualizarTipoDeBolas(!esBolaLisa); // Asigna el tipo de bola al Jugador 2
                    jugador1EsLisas = esBolaLisa; // Asigna el tipo de bola al Jugador 1
                }
            }

            if ((jugadorActual == JugadorActual.Jugador1 && (esBolaLisa != jugador1EsLisas)) ||
              (jugadorActual == JugadorActual.Jugador2 && (esBolaLisa != jugador2EsLisas)))
            {
                infraccion();
                mantieneTurno = false;
            }
            else
            {
                mantieneTurno = true;
                if (bola.esBolaLisa())
                {
                    j1BolasRestantes--;
                    j1Bolas.text = "JUGADOR 1 FALTAN " + j1BolasRestantes;
                    if (j1BolasRestantes <= 0)
                    {
                        tiroFinalJ1 = true;
                    }
                    if (jugadorActual != JugadorActual.Jugador1)
                    {
                        cambiaJugador = true;
                    }
                }
                else
                {
                    j2BolasRestantes--;
                    j2Bolas.text = "JUGADOR 2 FALTAN " + j2BolasRestantes;
                    if (j2BolasRestantes <= 0)
                    {
                        tiroFinalJ2 = true;
                    }
                    if (jugadorActual != JugadorActual.Jugador2)
                    {
                        cambiaJugador = true;
                    }
                }
                if (!mantieneTurno)
                {
                    turnoSiguiente();
                }
            }
        }

        return true;
    }
    public bool meteBolaBlanca()
    {
        if (jugadorActual == JugadorActual.Jugador1)
        {
            if (tiroFinalJ1)
            {
                meteBolaBlancaRectaFinal("Jugador 1");
                return true;
            }
        }
        else
        {
            if (tiroFinalJ2)
            {
                meteBolaBlancaRectaFinal("Jugador 2");
                return true;
            }
        }
        mantieneTurno = false;
        cambiaJugador = true;
        //turnoSiguiente();
        return false;
    }
    private void bolaNegraAntesDeTiempo()
    {
        if (jugadorActual == JugadorActual.Jugador1)
        {
            perdio("Jugador 1 Perdio la Partida!");
        }
        else
        {
            perdio("Jugador 2 Perdio la Partida!");
        }
    }
    private void meteBolaBlancaRectaFinal(string jugador)
    {
        perdio(jugador + " Perdio por haber metido la bola blanca en la Recta Final");
    }
    public void turnoSiguiente()
    {
        if(mantieneTurno == false) // No pasa de turno si el jugador emboca las bolas que le corresponen
        {
            if (jugadorActual == JugadorActual.Jugador1)
            {
                jugadorActual = JugadorActual.Jugador2;
                turnoActual.text = "TURNO DEL JUGADOR 2";
            }
            else
            {
                jugadorActual = JugadorActual.Jugador1;
                turnoActual.text = "TURNO DEL JUGADOR 1";
            }
        }
        actualizarTextoTipoDeBolas(); // Va cambiando con que tipo de bolas juega cada jugador en la UI
        cambiaJugador = false;
        cambio.cambioCamaras();
        tiempoEsperaPorBolas = true; // Reinicia para el nuevo turno
        tiempoActual = tiempoDisparo; // Reinicia el temporizador
        disparoHecho = false; // Reinicia la variable
        dioBola = false; // Reinicia la variable
        mantieneTurno = false;
    }
    private void perdio(string mensaje)
    {
        mensajeGrande.gameObject.SetActive(true);
        mensajeGrande.text = mensaje;
    }
    private void gano(string jugador)
    {
        mensajeGrande.gameObject.SetActive(true);
        mensajeGrande.text = jugador + " GANO LA PARTIDA!";
    }
    private void infraccion()
    {
        print("El Jugador esta jugando con otro tipo de Bola");
    }
    public void golpeoBolas()
    {
        dioBola = true;
        disparoHecho = true;
    }
    private void actualizarTextoTipoDeBolas()
    {
        if(jugadorActual == JugadorActual.Jugador1)
        {
            tipoDeBolas.text = jugador1EsLisas ? "CON LISAS" : "CON RAYADAS";
        }
        else
        {
            tipoDeBolas.text = jugador2EsLisas ? "CON LISAS" : "CON RAYADAS";
        }
    }
    private void actualizarTipoDeBolas(bool esLisas)
    {
        if (jugadorActual == JugadorActual.Jugador1)
        {
            jugador1EsLisas = esLisas;
        }
        else
        {
            jugador2EsLisas = esLisas;
        }
        actualizarTextoTipoDeBolas();
    }
}

