/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Merodear : Encarar
    {
        //Radio del c�rculo de merodeo.
        [SerializeField]
        private float wanderOffset;

        //Distancia del agente a la que se encuentra el c�rculo de merodeo.
        [SerializeField]
        private float wanderRadius;

        //Ratio m�ximo que puede cambiar el merodeo en cada ciclo.
        [SerializeField]
        private float wanderRate;

        //M�xima aceleraci�n que el agente puede adquirir.
        [SerializeField]
        private float maxAcceleration;

        //Objetivo de merodeo del agente.
        private Vector3 wanderTarget = new Vector3(0,0,0);

        //Valores necesarios para crear los ciclos de actualizaci�n de merodeo.
        public float intervalo = 1f; // Intervalo en segundos entre cada ejecuci�n de la acci�n
        private float tiempoTranscurrido = 0f;
        public override Direccion GetDireccion()
        {

            Vector3 targetOrientation;

            Direccion sol = new Direccion();

            tiempoTranscurrido += Time.deltaTime;

            if (tiempoTranscurrido >= intervalo)
            {
                // Resetea el temporizador
                tiempoTranscurrido -= intervalo;

                //A donde mira la rata
                Vector3 delante = agente.transform.forward;

                // Calculando la direcci�n opuesta (hacia atr�s)
                Vector3 atras = -delante;

                // Normalizando la direcci�n y escal�ndola por la distancia deseada
                Vector3 wanderDirection = atras.normalized * 3;

                // Obteniendo la posici�n de merodeo
                Vector3 posicionMerodeo = agente.transform.position + wanderDirection;

                //Actualiza la orientaci�n de merodeo
                wanderTarget = posicionMerodeo;
                wanderTarget += new Vector3(Random.Range(-360, 360), 0, Random.Range(-360, 360)) * wanderRate;

                //Calcula la orientacion combinada objetivo
                targetOrientation = wanderTarget + agente.transform.rotation.eulerAngles;
                //Calcula el centro del c�rculo de merodeo
                wanderTarget = agente.transform.position + wanderOffset * agente.transform.rotation.eulerAngles;
                //Calcula la localizaci�n objetivo.
                wanderTarget += wanderRadius * targetOrientation;

                //Establece la aceleraci�n linear para que vaya a toda la velocidad posible en la direcci�n elegida.
                sol.lineal = new Vector3(maxAcceleration * wanderTarget.x, 0, maxAcceleration * wanderTarget.z);
            }
            return sol;
        }

    }
}