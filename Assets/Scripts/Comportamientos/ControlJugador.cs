/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{
    using UnityEngine;

    /// <summary>
    /// El comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {
        #region references
        ScreenToWorld screenToWorld;
        #endregion
        #region properties
        Vector3 worldPoint;
        #endregion

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();

            if (screenToWorld != null && Input.GetMouseButton(0))
            {
                worldPoint = screenToWorld.ScreenToWorldPoint(Input.mousePosition);
                direccion.lineal.x = worldPoint.x - agente.transform.position.x;
                direccion.lineal.z = worldPoint.z - agente.transform.position.z;
            }
            else
            {
                // Direccion actual
                direccion.lineal.x = Input.GetAxis("Horizontal");
                direccion.lineal.z = Input.GetAxis("Vertical");
            }
            
            // Resto de cálculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }

        void Start()
        {
            screenToWorld = GetComponent<ScreenToWorld>();
        }
    }
}