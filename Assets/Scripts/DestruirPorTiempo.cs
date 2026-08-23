using UnityEngine;

public class DestruirPorTiempo : MonoBehaviour
{
    [Header("Tiempo de vida en segundos")]
    public float tiempoDeVida = 3f;

    void Start()
    {
        // Esto le dice a Unity que borre este GameObject de la memoria automáticamente
        // cuando hayan pasado los segundos configurados.
        Destroy(gameObject, tiempoDeVida);
    }
}