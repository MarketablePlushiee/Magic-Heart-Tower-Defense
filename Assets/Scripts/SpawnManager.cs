using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabEnemigo; //
    public float radioSpawn = 15f; // Distancia exacta desde el centro
    public float tiempoEntreOleadas = 2f; //[cite: 10]
    private float timer; //[cite: 10]

    [Header("Configuración de Variantes")]
    [Range(0f, 1f)]
    public float probabilidadEnemigoFuerte = 0.3f; // 30% de probabilidad de que salga uno pesado[cite: 10]

    [Header("Dificultad Progresiva")]
    public float reduccionTiempoPorSegundo = 0.02f; // Qué tan rápido aumentará la dificultad
    public float tiempoMinimoEntreOleadas = 0.4f; // El límite máximo de velocidad para que no colapse el iPad

    // Esta variable la controla el Brain para iniciar el juego
    [HideInInspector]
    public bool puedeSpawnear = false;

    void Start()
    {
        timer = tiempoEntreOleadas;
    }

    void Update()
    {
        // Si el jugador está en el menú, no calcula ni descuenta tiempo
        if (!puedeSpawnear) return;

        // --- LÓGICA DE DIFICULTAD PROGRESIVA ---
        // Reduce el tiempo entre oleadas gradualmente cada fotograma
        if (tiempoEntreOleadas > tiempoMinimoEntreOleadas)
        {
            tiempoEntreOleadas -= reduccionTiempoPorSegundo * Time.deltaTime;
        }

        // --- BUCLE DE SPAWN ---
        timer -= Time.deltaTime; //[cite: 10]
        if (timer <= 0) //[cite: 10]
        {
            SpawnearEnemigo(); //[cite: 10]
            timer = tiempoEntreOleadas; //[cite: 10]
        }
    }

    void SpawnearEnemigo()
    {
        // Creamos un punto en un círculo plano (2D) y lo normalizamos para obligarlo a estar en el borde exterior
        Vector2 puntoCirculo = Random.insideUnitCircle.normalized * radioSpawn;

        // Traspasamos los datos a coordenadas 3D del mundo (X y Z) manteniendo la altura del suelo
        Vector3 posicionAleatoria = new Vector3(puntoCirculo.x, 1.5f, puntoCirculo.y);

        // Instanciar el clon del cráneo en el mapa exterior
        GameObject nuevoCraneo = Instantiate(prefabEnemigo, posicionAleatoria, Quaternion.identity); //[cite: 10]

        // Buscamos el componente del enemigo para alterar sus estadísticas al vuelo
        EnemigoCraneo scriptEnemigo = nuevoCraneo.GetComponent<EnemigoCraneo>(); //[cite: 10]

        if (scriptEnemigo != null) //[cite: 10]
        {
            if (Random.value <= probabilidadEnemigoFuerte) //[cite: 10]
            {
                // ESTADÍSTICAS DEL CRÁNEO PESADO
                scriptEnemigo.vida = 2f;            //[cite: 10]
                scriptEnemigo.dañoAlCorazon = 20f;   //[cite: 10]
                scriptEnemigo.velocidad = 1.2f;     //[cite: 10]

                // Feedback visual
                nuevoCraneo.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //[cite: 10]
            }
            else
            {
                // ESTADÍSTICAS DEL CRÁNEO NORMAL
                scriptEnemigo.vida = 1f; //[cite: 10]
                scriptEnemigo.dañoAlCorazon = 10f; //[cite: 10]
                scriptEnemigo.velocidad = 2f; //[cite: 10]
            }
        }
    }
}