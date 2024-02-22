﻿/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
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
        [SerializeField]
        GameObject rats;
        [SerializeField]
        private float distanciaDeteccion = 5.0f;
        #endregion
        #region references
        private Transform myTransform;
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

            for (int i = 0; i < rats.transform.childCount; i++)
            {
                Vector3 rataPosition = rats.transform.GetChild(i).position;
                if (Vector3.Distance(myTransform.position, rataPosition) < radio)
                {
                    averagePosition += rataPosition;
                    ratasContadas++;
                }
            }

            // Calcular promedio solo si hay ratas dentro del radio
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
        }
    }
}