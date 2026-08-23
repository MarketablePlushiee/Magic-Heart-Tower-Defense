using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Brain : MonoBehaviour
{
    public float Healt = 100f;
    public float Mana = 10f;
    public float velocity = 5f;
    public float velOfShoot = 0.5f;
    public float BulletVel = 10f;

    [Header("Costos de Habilidades")]
    public float costoRoca = 3f;
    public float costoTornado = 2f;

    [Header("Regeneración de Maná")]
    public float manaRegenRate = 1f;
    public float manaRegenInterval = 3f;
    private float manaRegenTimer;

    float maxHealt;
    float maxMana;

    [Header("Referencias UI Barras")]
    public Slider HealtBar;
    public TextMeshProUGUI cantidadVida;
    public Slider ManaBar;
    public TextMeshProUGUI cantidadMana;

    [Header("Paneles de Estado (UI)")]
    public GameObject menuInicioPanel;
    public GameObject gameOverPanel;

    [Header("UI del Temporizador")]
    public TextMeshProUGUI textoTimer;
    private float tiempoDeJuego = 0f;
    private bool juegoIniciado = false;

    [Header("Audio del Juego")]
    public AudioSource audioCorazon;
    public AudioSource audioGameOver;
    public AudioSource musicaFondoAmbiente;

    [Header("Referencias de Escena")]
    public SpawnManager spawnManager;

    [Header("Juice Animación Corazón (Unidad III)")]
    public Transform modeloCorazonMesh;
    public float duracionImpacto = 0.3f;
    public float fuerzaSquash = 0.3f;
    private Vector3 escalaOriginalCorazon;
    private float ultimaVidaDetectada;
    private float ultimoManaDetectado;
    private bool animandoImpacto = false;
    private float timerAnimacion = 0f;

    [Header("Juice: Flash Shader Color")]
    public Renderer rendererCorazon;
    public Color colorFlashDano = Color.white;
    private Color colorOriginalShader;
    private float timerFlashColor = 0f;

    private bool haciendoBrincoVida = false;
    private bool haciendoBrincoMana = false;

    void Start()
    {
        maxHealt = Healt;
        maxMana = Mana;

        if (HealtBar != null) HealtBar.maxValue = maxHealt;
        if (ManaBar != null) ManaBar.maxValue = maxMana;

        if (modeloCorazonMesh != null)
        {
            escalaOriginalCorazon = modeloCorazonMesh.localScale;
        }

        if (rendererCorazon != null && rendererCorazon.material.HasProperty("_ColorDano"))
        {
            colorOriginalShader = rendererCorazon.material.GetColor("_ColorDano");
        }

        // Forzamos que las escalas iniciales de los textos arranquen en su tamaño nativo
        if (cantidadVida != null) cantidadVida.transform.localScale = Vector3.one;
        if (cantidadMana != null) cantidadMana.transform.localScale = Vector3.one;

        ultimaVidaDetectada = Healt;
        ultimoManaDetectado = Mana;

        Time.timeScale = 0f;
        if (menuInicioPanel != null) menuInicioPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        manaRegenTimer = manaRegenInterval;
    }

    void Update()
    {
        ActualizarUI();
        RegenerarMana();
        ControlarTemporizador();
        ComprobarGameOver();
        ProcesarJuiceDaño();
        ProcesarFlashShader();
        DetectarGastoManaJuice();
    }

    void ActualizarUI()
    {
        Healt = Mathf.Clamp(Healt, 0f, maxHealt);
        Mana = Mathf.Clamp(Mana, 0f, maxMana);

        if (HealtBar != null) HealtBar.value = Healt;
        if (cantidadVida != null) cantidadVida.text = Healt.ToString("F0");

        if (ManaBar != null) ManaBar.value = Mana;
        if (cantidadMana != null) cantidadMana.text = Mana.ToString("F0") + "/" + maxMana.ToString("F0");
    }

    void RegenerarMana()
    {
        if (!juegoIniciado) return;

        manaRegenTimer -= Time.deltaTime;
        if (manaRegenTimer <= 0f)
        {
            Mana += manaRegenRate;
            manaRegenTimer = manaRegenInterval;
        }
    }

    void ControlarTemporizador()
    {
        if (juegoIniciado && Healt > 0)
        {
            tiempoDeJuego += Time.deltaTime;

            int minutes = Mathf.FloorToInt(tiempoDeJuego / 60f);
            int seconds = Mathf.FloorToInt(tiempoDeJuego % 60f);

            if (textoTimer != null)
            {
                textoTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    void ProcesarJuiceDaño()
    {
        if (Healt < ultimaVidaDetectada)
        {
            ultimaVidaDetectada = Healt;
            if (modeloCorazonMesh != null)
            {
                animandoImpacto = true;
                timerAnimacion = 0f;
                timerFlashColor = 0.15f;
            }

            // CORRECCIÓN: El brinco elástico se le aplica solo al TEXTO, no a la barra completa
            if (cantidadVida != null && !haciendoBrincoVida)
            {
                StartCoroutine(BrincoTextoUI(cantidadVida.transform, true));
            }
        }
    }

    void ProcesarFlashShader()
    {
        if (timerFlashColor > 0f && rendererCorazon != null)
        {
            timerFlashColor -= Time.unscaledDeltaTime;
            rendererCorazon.material.SetColor("_ColorDano", colorFlashDano);
        }
        else if (rendererCorazon != null && rendererCorazon.material.HasProperty("_ColorDano"))
        {
            rendererCorazon.material.SetColor("_ColorDano", colorOriginalShader);
        }

        if (animandoImpacto && modeloCorazonMesh != null)
        {
            timerAnimacion += Time.unscaledDeltaTime;
            float progreso = timerAnimacion / duracionImpacto;

            if (progreso <= 1.0f)
            {
                float atenuacion = Mathf.Sin(progreso * Mathf.PI * 3f) * (1f - progreso);
                float deformacionY = atenuacion * fuerzaSquash;
                float deformacionXZ = -atenuacion * (fuerzaSquash * 0.5f);

                modeloCorazonMesh.localScale = new Vector3(
                    escalaOriginalCorazon.x + deformacionXZ,
                    escalaOriginalCorazon.y + deformacionY,
                    escalaOriginalCorazon.z + deformacionXZ
                );
            }
            else
            {
                modeloCorazonMesh.localScale = escalaOriginalCorazon;
                animandoImpacto = false;
            }
        }
    }

    void DetectarGastoManaJuice()
    {
        if (Mana < ultimoManaDetectado)
        {
            // CORRECCIÓN: El brinco elástico se le aplica solo al TEXTO del maná
            if (cantidadMana != null && !haciendoBrincoMana)
            {
                StartCoroutine(BrincoTextoUI(cantidadMana.transform, false));
            }
        }
        ultimoManaDetectado = Mana;
    }

    // Corrutina elástica segura para textos TMP (Garantiza el tamaño arcade sin romper las barras)
    IEnumerator BrincoTextoUI(Transform textoTransform, bool esVida)
    {
        if (esVida) haciendoBrincoVida = true;
        else haciendoBrincoMana = true;

        textoTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f); // El texto salta un 30%
        yield return new WaitForSecondsRealtime(0.1f);
        textoTransform.localScale = Vector3.one; // Regresa al tamaño exacto base

        if (esVida) haciendoBrincoVida = false;
        else haciendoBrincoMana = false;
    }

    public bool ConsumirMana(float cantidad)
    {
        if (Mana >= cantidad)
        {
            Mana -= cantidad;
            return true;
        }
        return false;
    }

    void ComprobarGameOver()
    {
        if (Healt <= 0 && juegoIniciado)
        {
            juegoIniciado = false;
            Time.timeScale = 0f;

            AudioSource[] todosLosAudios = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
            foreach (AudioSource emisor in todosLosAudios)
            {
                emisor.Stop();
            }

            if (audioGameOver != null) audioGameOver.Play();
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
        }
    }

    public void BotonFuncionalJugar()
    {
        if (menuInicioPanel != null) menuInicioPanel.SetActive(false);

        Time.timeScale = 1f;
        juegoIniciado = true;

        if (audioCorazon != null) audioCorazon.Play();
        if (musicaFondoAmbiente != null && !musicaFondoAmbiente.isPlaying) musicaFondoAmbiente.Play();

        if (spawnManager != null) spawnManager.puedeSpawnear = true;
    }

    public void BotonFuncionalSalir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    public void BotonFuncionalReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}