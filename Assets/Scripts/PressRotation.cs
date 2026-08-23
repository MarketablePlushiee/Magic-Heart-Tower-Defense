using UnityEngine;

public class PressRotation : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    public Brain estadisticas;
    private float rotationSpeed;
    private Camera mainCamera;

    [Header("Juice: Mira Táctica (Unidad III)")]
    public Transform prefabMiraVisual; // Arrastra aquí un objeto/sprite de mira en el suelo
    private Transform miraInstanciada;

    void Start()
    {
        mainCamera = Camera.main;

        // Instancia la mira oculta al arrancar el juego
        if (prefabMiraVisual != null)
        {
            miraInstanciada = Instantiate(prefabMiraVisual);
            miraInstanciada.gameObject.SetActive(false); // Parte apagada
        }
    }

    void Update()
    {
        if (estadisticas == null) return;
        rotationSpeed = estadisticas.velocity;

        // Si el jugador no está tocando la pantalla, apagamos la mira visual
        if (Input.touchCount == 0)
        {
            if (miraInstanciada != null) miraInstanciada.gameObject.SetActive(false);
            return;
        }

        GirarHaciaClicYPosicionarMira();
    }

    void GirarHaciaClicYPosicionarMira()
    {
        // Creamos un rayo desde la posición del touch en la pantalla hacia el mundo 3D
        Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        // Lanzamos el rayo al mundo
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            // 1. POSICIONAR LA MIRA TÁCTICA EN EL SUELO
            if (miraInstanciada != null)
            {
                miraInstanciada.gameObject.SetActive(true);
                // Posiciona la mira exactamente donde toca el dedo, sutilmente sobre el suelo (Y = 0.05f) para evitar z-fighting
                miraInstanciada.position = new Vector3(targetPosition.x, 0.05f, targetPosition.z);

                // Hace que la mira rote sobre su propio eje sutilmente para darle Juice visual
                miraInstanciada.Rotate(Vector3.up * 50f * Time.deltaTime);
            }

            // 2. LÓGICA DE ROTACIÓN DEL JUGADOR
            targetPosition.y = transform.position.y; // Mantiene la Y al nivel del jugador
            Vector3 direction = targetPosition - transform.position;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnDestroy()
    {
        // Limpieza de memoria si se destruye el objeto
        if (miraInstanciada != null)
        {
            Destroy(miraInstanciada.gameObject);
        }
    }
}