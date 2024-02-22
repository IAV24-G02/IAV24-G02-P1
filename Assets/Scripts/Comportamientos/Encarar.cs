using UnityEngine;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de WANDER a otro agente
    /// </summary>
    public class Encarar : Alineamiento
    {
        #region methods
        public override Direccion GetDireccion()
        {
            Debug.Log("Meordeo 2");
            Vector3 direccion = new Vector3();
            direccion = objetivo.transform.position - transform.position;
            float distancia = direccion.magnitude;

            if (distancia == 0)
                return lastDir;

            Agente objetivoAgente = objetivo.GetComponent<Agente>();
            objetivoAgente.orientacion = Mathf.Atan2(-direccion.x, direccion.z);

            return base.GetDireccion();
        }
        #endregion
    }
}
