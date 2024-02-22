/*    
   Copyright (C) 2024 Laura Wang Qiu
   http://www.github.com/LauraWangQiu

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Laura Wang Qiu
   Contacto: yiwang03@ucm.es
*/
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class HuirDeUnGrupo : ComportamientoAgente
    {
        #region parameters
        [SerializeField]
        private float radio = 3.0f;
        #endregion
        #region properties
        private Transform myTransform = null;
        #endregion

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            Vector3 averagePosition = Vector3.zero;
            int ratasContadas = 0;

            // Recorre la lista de ratas
            for (int i = 0; i < objetivo.transform.childCount; i++)
            {
                // Si la posición de la rata está dentro del radio, se usa su posición
                Vector3 rataPosition = objetivo.transform.GetChild(i).position;
                if (Vector3.Distance(myTransform.position, rataPosition) < radio)
                {
                    averagePosition += rataPosition;
                    ratasContadas++;
                }
            }

            // Calcula la posición promedia de las ratas dentro del radio
            // y calcula la dirección a la que debe ir
            if (ratasContadas > 0)
            {
                averagePosition /= ratasContadas;
                direccion.lineal = myTransform.position - averagePosition;
                direccion.lineal.Normalize();
                direccion.lineal *= agente.aceleracionMax;
            }

            return direccion;
        }

        private void Start()
        {
            myTransform = GetComponent<Transform>();
            objetivo = GameObject.Find("Ratas");
        }
    }
}
