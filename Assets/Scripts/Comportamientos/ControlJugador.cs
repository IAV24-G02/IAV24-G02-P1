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
        [SerializeField]
        private GameObject cheese;
        [SerializeField]
        GameObject cheeseContainer;
        #endregion
        #region properties
        Transform myTransform;
        Vector3 worldPoint;
        int currentCheese = 0;
        [SerializeField]
        private int maxCheese = 5;
        [SerializeField]
        private float radio = 3.0f;
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

            #region Ampliacion Quesos
            if (currentCheese < maxCheese && screenToWorld != null && Input.GetMouseButtonDown(2))
            {
                Vector3 offset = new Vector3(0, 1, 0);
                Vector3 cheesePos = screenToWorld.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(cheese, cheesePos + offset, Quaternion.identity, cheeseContainer.transform);
                currentCheese++;
            }
            #endregion

            // Resto de cálculo de movimiento
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            return direccion;
        }

        void Start()
        {
            myTransform = GetComponent<Transform>();
            screenToWorld = GetComponent<ScreenToWorld>();
        }
    }
}