using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class EnemigoCraneo : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    public float vida = 3f;
    public float velocidad = 2f;
    public float dañoAlCorazon = 10f;
    private float velocidadBase;

    [Header("Efectos Especiales (VFX) - Unidad III")]
    public GameObject vfxEfectoMuerte;
    public GameObject vfxChispasQuemadura;
    public GameObject vfxAuraCongelado;

    [Header("Sonido y Mezcla (SFX)")]
    public AudioMixerGroup canalMixerSFX;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoHitDaño;

    [Range(0f, 2f)]
    public float volumenSonidoHit = 1f;

    private AudioSource audioSourceLocal;
    private bool yaEstaMuerto = false;

    private bool estaRalentizado = false;
    private bool estaQuemándose = false;

    [Header("Juice: Animación Procedural (Unidad III)")]
    public float velocidadRebote = 8f;
    public float amplitudRebote = 0.15f;
    private Vector3 escalaOriginal;

    private Transform objetivo;
    private Brain brain;
    private Rigidbody rb;
    private Collider colliderLocal;

    [Header("Juice: Flash de Daño Retro (Universal)")]
    public Renderer rendererEnemigo;       // El MeshRenderer de tu cráneo
    public float duracionFlash = 0.1f;     // Qué tan rápido parpadea
    private bool estaEnFlash = false;

    void Start()
    {
        escalaOriginal = transform.localScale;
        velocidadBase = velocidad;
        rb = GetComponent<Rigidbody>();
        colliderLocal = GetComponent<Collider>();

        // Configuración del AudioSource local conectado al Mixer
        audioSourceLocal = gameObject.AddComponent<AudioSource>();
        audioSourceLocal.playOnAwake = false;
        audioSourceLocal.spatialBlend = 1f;

        if (canalMixerSFX != null)
        {
            audioSourceLocal.outputAudioMixerGroup = canalMixerSFX;
        }

        GameObject corazonObj = GameObject.Find("Corazon");
        if (corazonObj != null) objetivo = corazonObj.transform;

        GameObject brainObj = GameObject.Find("Brain");
        if (brainObj != null) brain = brainObj.GetComponent<Brain>();
    }

    void FixedUpdate()
    {
        if (yaEstaMuerto) return;

        if (objetivo != null && Time.timeScale > 0)
        {
            Vector3 direccion = (objetivo.position - transform.position).normalized;
            direccion.y = 0;

            if (rb != null)
            {
                rb.linearVelocity = direccion * velocidad;
            }
            else
            {
                transform.position += direccion * velocidad * Time.fixedDeltaTime;
            }

            if (direccion != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direccion);
            }
        }
        else if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (yaEstaMuerto) return;

        if (objetivo != null && Time.timeScale > 0)
        {
            float oscilacion = Mathf.Sin(Time.time * velocidadRebote);
            float cambioY = oscilacion * amplitudRebote;
            float cambioXZ = -oscilacion * (amplitudRebote * 0.5f);

            transform.localScale = new Vector3(
                escalaOriginal.x + cambioXZ,
                escalaOriginal.y + cambioY,
                escalaOriginal.z + cambioXZ
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (yaEstaMuerto) return;
        EvaluarImpacto(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (yaEstaMuerto) return;
        EvaluarImpacto(other.gameObject);
    }

    void EvaluarImpacto(GameObject objetoImpactado)
    {
        if (objetoImpactado.CompareTag("PlayerCorazon"))
        {
            AtacarBase();
            return;
        }

        if (objetoImpactado.CompareTag("Bala") || objetoImpactado.name.ToLower().Contains("bala") || objetoImpactado.name.ToLower().Contains("bullet"))
        {
            QuitarVida(1f);
            ReproducirSonidoHit();
            GatillarFlashDaño();

            AudioSource audioBala = objetoImpactado.GetComponent<AudioSource>();
            if (audioBala != null && audioBala.clip != null)
            {
                AudioSource.PlayClipAtPoint(audioBala.clip, objetoImpactado.transform.position, 1f);
            }

            Destroy(objetoImpactado);
        }
        else if (objetoImpactado.CompareTag("Tornado") || objetoImpactado.name.ToLower().Contains("tornado"))
        {
            QuitarVida(1f);
            ReproducirSonidoHit();
            GatillarFlashDaño();
        }
        else if (objetoImpactado.CompareTag("Roca") || objetoImpactado.name.ToLower().Contains("roca"))
        {
            Morir();
        }
        else if (objetoImpactado.CompareTag("Hielo") || objetoImpactado.name.ToLower().Contains("hielo") || objetoImpactado.name.ToLower().Contains("ice"))
        {
            QuitarVida(0.5f);
            ReproducirSonidoHit();
            GatillarFlashDaño();
            StartCoroutine(AplicarEfectoHielo(3f));
        }
        else if (objetoImpactado.CompareTag("Magma") || objetoImpactado.name.ToLower().Contains("magma") || objetoImpactado.name.ToLower().Contains("lava"))
        {
            QuitarVida(1f);
            ReproducirSonidoHit();
            GatillarFlashDaño();
            StartCoroutine(AplicarEfectoMagma(3.5f));
        }
    }

    IEnumerator AplicarEfectoHielo(float duracion)
    {
        if (estaRalentizado || yaEstaMuerto) yield break;
        estaRalentizado = true;

        velocidad = velocidadBase * 0.5f;

        GameObject auraInstanciada = null;
        if (vfxAuraCongelado != null)
        {
            auraInstanciada = Instantiate(vfxAuraCongelado, transform.position, Quaternion.identity);
            auraInstanciada.transform.SetParent(transform);
            auraInstanciada.transform.localScale = Vector3.one * 1.5f;
        }

        yield return new WaitForSeconds(duracion);

        if (auraInstanciada != null) Destroy(auraInstanciada);
        velocidad = velocidadBase;
        estaRalentizado = false;
    }

    IEnumerator AplicarEfectoMagma(float duracion)
    {
        if (estaQuemándose || yaEstaMuerto) yield break;
        estaQuemándose = true;

        GameObject chispasInstanciadas = null;

        if (vfxChispasQuemadura != null)
        {
            chispasInstanciadas = Instantiate(vfxChispasQuemadura, transform.position, Quaternion.identity);
            chispasInstanciadas.transform.SetParent(transform);
            chispasInstanciadas.transform.localScale = Vector3.one * 1.5f;
        }

        float timer = 0f;
        while (timer < duracion && !yaEstaMuerto)
        {
            yield return new WaitForSeconds(0.7f);
            if (!yaEstaMuerto)
            {
                QuitarVida(0.5f);
                ReproducirSonidoHit();
                GatillarFlashDaño();
            }
            timer += 0.7f;
        }

        if (chispasInstanciadas != null) Destroy(chispasInstanciadas);
        estaQuemándose = false;
    }

    void GatillarFlashDaño()
    {
        if (rendererEnemigo != null && !estaEnFlash && !yaEstaMuerto)
        {
            StartCoroutine(CorrutinaFlashUniversal());
        }
    }

    // SOLUCIÓN ATÓMICA: Parpadeo de visibilidad ultra veloz. Funciona con CUALQUIER Shader y textura del mundo.
    IEnumerator CorrutinaFlashUniversal()
    {
        estaEnFlash = true;

        // Primer parpadeo (Apagado/Encendido rápido que emula el daño clásico)
        rendererEnemigo.enabled = false;
        yield return new WaitForSeconds(duracionFlash * 0.5f);

        if (!yaEstaMuerto) rendererEnemigo.enabled = true;
        yield return new WaitForSeconds(duracionFlash * 0.5f);

        // Segundo ciclo para asegurar que el ojo humano lo capte en el iPad
        if (!yaEstaMuerto) rendererEnemigo.enabled = false;
        yield return new WaitForSeconds(duracionFlash * 0.5f);

        if (rendererEnemigo != null) rendererEnemigo.enabled = true;
        estaEnFlash = false;
    }

    void ReproducirSonidoHit()
    {
        if (sonidoHitDaño != null && audioSourceLocal != null && vida > 0 && !yaEstaMuerto)
        {
            audioSourceLocal.PlayOneShot(sonidoHitDaño, volumenSonidoHit);
        }
    }

    public void QuitarVida(float cantidad)
    {
        if (yaEstaMuerto) return;

        vida -= cantidad;
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        if (yaEstaMuerto) return;
        yaEstaMuerto = true;

        if (vfxEfectoMuerte != null)
        {
            GameObject explosion = Instantiate(vfxEfectoMuerte, transform.position, Quaternion.identity);
            float factorEscala = escalaOriginal.x * 1.4f;
            explosion.transform.localScale = new Vector3(factorEscala, factorEscala, factorEscala);
        }

        float tiempoEsperaDestruccion = 0.1f;

        if (sonidoMuerte != null && audioSourceLocal != null)
        {
            audioSourceLocal.PlayOneShot(sonidoMuerte, 1f);
            tiempoEsperaDestruccion = sonidoMuerte.length;
        }

        ApagarComponentesEnMuerte();

        Destroy(gameObject, tiempoEsperaDestruccion);
    }

    void AtacarBase()
    {
        if (yaEstaMuerto) return;
        yaEstaMuerto = true;

        if (brain != null) brain.Healt -= dañoAlCorazon;

        if (vfxEfectoMuerte != null)
        {
            GameObject explosion = Instantiate(vfxEfectoMuerte, transform.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * escalaOriginal.x;
        }

        float tiempoEsperaDestruccion = 0.1f;

        if (sonidoMuerte != null && audioSourceLocal != null)
        {
            audioSourceLocal.PlayOneShot(sonidoMuerte, 1f);
            tiempoEsperaDestruccion = sonidoMuerte.length;
        }

        ApagarComponentesEnMuerte();

        Destroy(gameObject, tiempoEsperaDestruccion);
    }

    void ApagarComponentesEnMuerte()
    {
        if (colliderLocal != null) colliderLocal.enabled = false;
        if (rb != null) rb.linearVelocity = Vector3.zero;
        if (rendererEnemigo != null) rendererEnemigo.enabled = false; // Asegura apagar el mesh principal

        foreach (Transform hijo in transform)
        {
            if (!hijo.gameObject.name.Contains("VFX") && !hijo.gameObject.name.Contains("Particle"))
            {
                hijo.gameObject.SetActive(false);
            }
        }
    }
}