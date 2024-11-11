using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameover;
    private PersonajeVida personajeVida;


    private void activarMenu(object sender, EventArgs e)
    {
        gameover.SetActive(true); // Activa el panel de Game Over.
    }

    private void Start()
    {
        personajeVida = FindObjectOfType<PersonajeVida>(); // Encuentra el componente de PersonajeVida en la escena.
        personajeVida.MuerteJugador += activarMenu; // Suscribe el evento MuerteJugador.
    }
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    public void salir(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}
