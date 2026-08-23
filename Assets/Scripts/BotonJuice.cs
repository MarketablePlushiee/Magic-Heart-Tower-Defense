using UnityEngine;
using UnityEngine.EventSystems; // Requerido para capturar el touch nativo del iPad

public class BotonJuice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Juice Configuración")]
    public float escalaPresionado = 0.85f; // Se encoge un 15% al poner el dedo
    public AudioClip sonidoClick;
    public AudioSource emisorAudioComponente; // Si dejas vacío, usa AudioSource local

    private Vector3 escalaOriginal;

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    // Se ejecuta de inmediato en el milisegundo en que el dedo toca la pantalla del iPad
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal * escalaPresionado; // Feedback elástico visual

        // Feedback Acústico
        if (sonidoClick != null)
        {
            if (emisorAudioComponente != null)
                emisorAudioComponente.PlayOneShot(sonidoClick);
            else
                AudioSource.PlayClipAtPoint(sonidoClick, Camera.main.transform.position);
        }
    }

    // Se ejecuta de forma segura cuando el jugador levanta el dedo del touch
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal; // Devuelve el botón a su tamaño nativo
    }
}