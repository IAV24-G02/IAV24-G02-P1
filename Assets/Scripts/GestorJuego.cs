/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

namespace UCM.IAV.Movimiento
{
    public class GestorJuego : MonoBehaviour
    {
        #region parameters
        [SerializeField]
        private int maxCheese = 10;
        #endregion
        #region properties
        public static GestorJuego instance = null;
        private ScreenToWorld screenToWorld = null;
        #endregion
        #region references
        // Prefabs
        [SerializeField]
        private GameObject scenario = null;
        [SerializeField]
        private GameObject rataPrefab = null;
        [SerializeField]
        private GameObject cheesePrefab = null;

        // Textos UI
        [SerializeField]
        private Text fRText;
        [SerializeField]
        private Text ratText;
        [SerializeField]
        private TMP_InputField input;
        [SerializeField]
        private Text cheeseText;

        // GameObject Padres de listas
        private GameObject rataGO = null;
        private GameObject cheeses = null;

        // Variables de timer de framerate
        private int frameRate = 60;
        private int m_frameCounter = 0;
        private float m_timeCounter = 0.0f;
        private float m_lastFramerate = 0.0f;
        private float m_refreshTime = 0.5f;

        // Ratas y Quesos
        private int numRats = 0;
        private int currentCheese = 0;

        // Cámara
        private bool cameraPerspective = true;
        #endregion

        private void Awake()
        {
            // Para que el gestor del juego no se destruya entre escenas
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Lo primero que se llama al activarse (tras el Awake)
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // Delegado para hacer cosas cuando una escena termina de cargar (no necesariamente cuando ha cambiado/switched)
        // Antiguamente se usaba un m�todo del SceneManager llamado OnLevelWasLoaded(int level), ahora obsoleto
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            rataGO = GameObject.Find("Ratas");
            ratText = GameObject.Find("NumRats").GetComponent<Text>();
            fRText = GameObject.Find("Framerate").GetComponent<Text>();
            cheeseText = GameObject.Find("Cheeses").GetComponent<Text>();
            numRats = rataGO.transform.childCount;
            ratText.text = numRats.ToString();
            cheeseText.text = (maxCheese - currentCheese).ToString();
        }

        // Se llama para poner en marcha el gestor
        private void Start()
        {
            screenToWorld = GetComponent<ScreenToWorld>();
            rataGO = GameObject.Find("Ratas");
            cheeses = GameObject.Find("Quesos");
            Application.targetFrameRate = frameRate;
            numRats = rataGO.transform.childCount;
            ratText.text = numRats.ToString();
            cheeseText.text = (maxCheese - currentCheese).ToString();
        }

        // Se llama cuando el juego ha terminado
        void OnDisable()
        { 
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Update is called once per frame
        void Update()
        {
            // Timer para mostrar el frameRate a intervalos
            if (m_timeCounter < m_refreshTime)
            {
                m_timeCounter += Time.deltaTime;
                m_frameCounter++;
            }
            else
            {
                m_lastFramerate = (float)m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;
            }

            // Texto con el framerate y 2 decimales
            fRText.text = (((int)(m_lastFramerate * 100 + .5) / 100.0)).ToString();

            // Input
            if (Input.GetKeyDown(KeyCode.R))
                Restart();
            if (Input.GetKeyDown(KeyCode.T))
                HideScenario();
            if (Input.GetKeyDown(KeyCode.O))
                SpawnRata();
            if (Input.GetKeyDown(KeyCode.P))
                DespawnRata();
            if (Input.GetKeyDown(KeyCode.F))
                ChangeFrameRate();
            if (Input.GetKeyDown(KeyCode.N))
                ChangeCameraView();
            if (Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.KeypadEnter))
                EnterInput();
            if (Input.GetMouseButtonDown(2))
                SpawnCheese();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void HideScenario()
        {
            if (scenario == null)
                return;

            if (scenario.activeSelf)
                scenario.SetActive(false);
            else
                scenario.SetActive(true);
        }

        private void SpawnRata(int quantity = 1)
        {
            if (rataPrefab == null || rataGO == null)
                return;

            for (int i = 0; i < quantity; ++i)
            {
                Instantiate(rataPrefab, rataGO.transform).GetComponent<Separacion>().targEmpty = rataGO;
            }

            numRats += quantity;
            ratText.text = numRats.ToString();
        }

        private void DespawnRata(int quantity = 1)
        {
            if (rataGO == null || rataGO.transform.childCount < 1)
                return;

            // Recolectar referencias de objetos a destruir.
            List<GameObject> toDestroy = new List<GameObject>();
            for (int i = 0; i < quantity && i < rataGO.transform.childCount; i++)
            {
                toDestroy.Add(rataGO.transform.GetChild(i).gameObject);
            }

            // Destruir los objetos recolectados.
            foreach (var obj in toDestroy)
            {
                Destroy(obj);
            }

            numRats -= quantity;
            ratText.text = numRats.ToString();
        }

        private void ChangeFrameRate()
        {
            if (frameRate == 30)
            {
                frameRate = 60;
                Application.targetFrameRate = 60;
            }
            else
            {
                frameRate = 30;
                Application.targetFrameRate = 30;
            }
        }

        private void ChangeCameraView()
        {
            if (cameraPerspective){
                Camera.main.GetComponent<SeguimientoCamara>().offset = new Vector3(0, 15, -2);
                cameraPerspective = false;
            }
            else{
                Camera.main.GetComponent<SeguimientoCamara>().offset = new Vector3(0, 7, -10);
                cameraPerspective = true;
            }
        }

        public void EnterInput()
        {
            // Comprueba de que se trate un número válido
            int inputRats;
            bool isValid = int.TryParse(input.text, out inputRats);
            if (isValid && inputRats >= 0)
            {
                int difference = Mathf.Abs(numRats - inputRats);
                if (inputRats > numRats)
                {
                    SpawnRata(difference);
                }
                else if (inputRats < numRats)
                {
                    DespawnRata(difference);
                }
            }
        }

        private void SpawnCheese()
        {
            if (currentCheese < maxCheese && screenToWorld != null)
            {
                Vector3 offset = new Vector3(0, 1, 0);
                Vector3 cheesePos = screenToWorld.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(cheesePrefab, cheesePos + offset, Quaternion.identity, cheeses.transform);
                currentCheese++;
                cheeseText.text = (maxCheese - currentCheese).ToString();
            }
        }

        public void DespawnCheese(GameObject obj)
        {
            if (obj == null)
                return;

            Destroy(obj);
        }
    }
}