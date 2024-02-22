/*    
   Copyright (C) 2024 Laura Wang Qiu & Agust�n Castro de Troya
   http://www.github.com/LauraWangQiu | http://www.github.com/AgusCDT

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autore: Laura Wang Qiu & Agust�n Castro de Troya
   Contacto: yiwang03@ucm.es | acastrod@ucm.es
*/

using UnityEngine;

namespace UCM.IAV.Movimiento
{
    public class SeguirQueso : ComportamientoAgente
    {
        #region parameters
        [SerializeField]
        private float radio = 5.0f;
        #endregion
        #region references
        private GestorJuego gestor = null;
        #endregion
        #region properties
        private Transform myTransform = null;
        #endregion

        #region methods
        public override Direccion GetDireccion()
        {
            Direccion direccion = new Direccion();
            int quesosContados = 0;
            Vector3 averagePosition = Vector3.zero;
            for (int i = 0; i < objetivo.transform.childCount; i++)
            {
                Vector3 quesoPosition = objetivo.transform.GetChild(i).position;
                if (Vector3.Distance(myTransform.position, quesoPosition) < radio)
                {
                    averagePosition += quesoPosition;
                    quesosContados++;
                }
                if (Vector3.Distance(myTransform.position, quesoPosition) < 1.0f)
                {
                    gestor.DespawnCheese(objetivo.transform.GetChild(i).gameObject);
                }
            }

            if (quesosContados > 0)
            {
                averagePosition /= quesosContados;
                direccion.lineal = averagePosition - myTransform.position;
                direccion.lineal.Normalize();
                direccion.lineal *= agente.aceleracionMax;              
            }

            return direccion;
        }

        void Start()
        {           
            objetivo = GameObject.Find("Quesos");
            GameObject gJ = GameObject.Find("GestorJuego");
            if (gJ != null)
            {
                gestor = gJ.GetComponent<GestorJuego>();
            }
            myTransform = GetComponent<Transform>();
        }
        #endregion
    }
}


