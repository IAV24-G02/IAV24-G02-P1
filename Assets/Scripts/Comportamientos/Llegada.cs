/*    
   Copyright (C) 2024 Ignacio Ligero
   http://www.github.com/theligero

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Ignacio Ligero
   Contacto: iligero@ucm.es
*/
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        public float distancia = 7;

        public float acelMaxima;
        public float velMaxima;

        // El radio para llegar al objetivo
        [SerializeField]
        private float rObjetivo = 6.0f;

        // El radio en el que se empieza a ralentizarse
        [SerializeField]
        private float rRalentizado = 15.0f;

        // El tiempo en el que conseguir la aceleracion objetivo
        private float timeToTarget = 0.1f;

        public void Start()
        {
            objetivo = GameObject.Find("Avatar");
        }

        /// <summary>
        /// Obtiene la direcci�n
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            // Consigue la direcci�n hacia el objetivo
            Vector3 direccion = new Vector3();
            direccion = objetivo.transform.position - transform.position;
            distancia = direccion.magnitude;
            float velObjetivo;

            // Comprueba si ya ha llegado
            if (distancia < rObjetivo)
                velObjetivo = 0.0f;

            // Si estamos fuera del rRalentizado
            else if (distancia > rRalentizado)
                velObjetivo = velMaxima; // entonces se mueve a velocidad m�xima
            // En otro caso calcula la velocidad en escala
            else
                velObjetivo = velMaxima * distancia / rRalentizado;

            //Debug.Log("Velocidad objetivo: " + velObjetivo);

            // La velocidad objetivo combina velocidad y direcci�n
            Vector3 vObjetivo = new Vector3();
            vObjetivo = direccion;
            vObjetivo.Normalize();
            vObjetivo *= velObjetivo;

            // La aceleraci�n intenta conseguir la velocidad objetivo
            Direccion sol = new Direccion();
            sol.lineal = vObjetivo;
            sol.lineal /= timeToTarget;

            // Comprueba si la aceleraci�n es demasiado alta
            if (sol.lineal.magnitude > acelMaxima)
            {
                sol.lineal.Normalize();
                sol.lineal *= acelMaxima;
            }

            sol.angular = 0;
            return sol;
        }
    }
}
