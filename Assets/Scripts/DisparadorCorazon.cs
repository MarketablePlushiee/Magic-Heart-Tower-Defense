using UnityEngine;

public class DisparadorCorazon : MonoBehaviour
{
    public Brain brain;
    public Transform puntaDisparo; // Punto de origen desde el frente del corazón

    [Header("Prefabs de Proyectiles Originales")]
    public GameObject prefabNormal;
    public GameObject prefabRoca;
    public GameObject prefabTornado;

    [Header("Prefabs de Proyectiles Nuevos (Unidad III)")]
    public GameObject prefabHielo;
    public GameObject prefabMagma;

    [Header("Costos de Maná Nuevos")]
    public float costoHielo = 1f;
    public float costoMagma = 2f;

    private float timerDisparoNormal;

    void Start()
    {
        if (brain == null) brain = GetComponent<Brain>();
        timerDisparoNormal = brain.velOfShoot;
    }

    void Update()
    {
        // Detecta si el jugador está manteniendo presionado el Clic Izquierdo del Mouse O la Pantalla Táctil
        bool disparando = (Input.GetMouseButton(0) || Input.touchCount > 0) && Time.timeScale > 0;

        if (disparando)
        {
            timerDisparoNormal -= Time.deltaTime;
            if (timerDisparoNormal <= 0f)
            {
                DispararNormal();
                timerDisparoNormal = brain.velOfShoot;
            }
        }
        else
        {
            timerDisparoNormal = brain.velOfShoot;
        }
    }

    void DispararNormal()
    {
        if (prefabNormal == null) return;

        GameObject bullet = Instantiate(prefabNormal, puntaDisparo.position, transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * brain.BulletVel;
        }
    }

    public void LanzarEspecialRoca()
    {
        if (brain.ConsumirMana(brain.costoRoca) && prefabRoca != null)
        {
            GameObject roca = Instantiate(prefabRoca, puntaDisparo.position, transform.rotation);
            Rigidbody rb = roca.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * (brain.BulletVel * 0.7f);
            }
        }
    }

    public void LanzarEspecialTornado()
    {
        if (brain.ConsumirMana(brain.costoTornado) && prefabTornado != null)
        {
            GameObject tornado = Instantiate(prefabTornado, puntaDisparo.position, transform.rotation);
            Rigidbody rb = tornado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * brain.BulletVel;
            }
        }
    }

    // NUEVA HABILIDAD: DISPARO DE HIELO
    public void LanzarEspecialHielo()
    {
        if (brain.ConsumirMana(costoHielo) && prefabHielo != null)
        {
            GameObject hielo = Instantiate(prefabHielo, puntaDisparo.position, transform.rotation);
            Rigidbody rb = hielo.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * (brain.BulletVel * 1.3f); // El hielo viaja más rápido
            }
        }
    }

    // NUEVA HABILIDAD: DISPARO DE MAGMA
    public void LanzarEspecialMagma()
    {
        if (brain.ConsumirMana(costoMagma) && prefabMagma != null)
        {
            GameObject magma = Instantiate(prefabMagma, puntaDisparo.position, transform.rotation);
            Rigidbody rb = magma.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = transform.forward * (brain.BulletVel * 0.6f); // El magma es pesado y lento
            }
        }
    }
}