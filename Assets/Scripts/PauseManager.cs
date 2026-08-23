using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Paneles")]
    public GameObject panelPausa;
    public GameObject panelOpciones;

    [Header("Sliders de la UI")]
    public Slider sliderMaster;
    public Slider sliderMusica;
    public Slider sliderSFX;

    [Header("Audio Mixer Central")]
    public AudioMixer masterMixer;

    private bool estaPausado = false;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);

        if (sliderMaster != null)
        {
            sliderMaster.minValue = 0.0001f;
            sliderMaster.maxValue = 1f;
            sliderMaster.value = 1f;
        }

        if (sliderMusica != null)
        {
            sliderMusica.minValue = 0.0001f;
            sliderMusica.maxValue = 1f;
            sliderMusica.value = 1f;
        }

        if (sliderSFX != null)
        {
            sliderSFX.minValue = 0.0001f;
            sliderSFX.maxValue = 1f;
            sliderSFX.value = 1f;
        }
    }

    public void TogglePausa()
    {
        if (estaPausado)
            ReanudarJuego();
        else
            PausarJuego();
    }

    public void PausarJuego()
    {
        estaPausado = true;
        if (panelPausa != null) panelPausa.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReanudarJuego()
    {
        estaPausado = false;
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
        Time.timeScale = 1f;
    }

    public void AbrirOpciones()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }

    public void VolverAPausa()
    {
        if (panelOpciones != null) panelOpciones.SetActive(false);
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    public void ReiniciarPartida()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenu()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }

    public void SetVolumenMaster(float value)
    {
        if (masterMixer == null) return;
        float dB = Mathf.Log10(value) * 20f;
        masterMixer.SetFloat("VolMaster", dB);
    }

    public void SetVolumenMusica(float value)
    {
        if (masterMixer == null) return;
        float dB = Mathf.Log10(value) * 20f;
        masterMixer.SetFloat("VolMusica", dB);
    }

    public void SetVolumenSFX(float value)
    {
        if (masterMixer == null) return;
        float dB = Mathf.Log10(value) * 20f;
        masterMixer.SetFloat("VolSFX", dB);
    }
}