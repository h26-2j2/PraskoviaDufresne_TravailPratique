using UnityEngine;

public class GestionnaireDegats : MonoBehaviour
{
    private SpriteRenderer sr;
    private float flickerTimer = 0f;
    private bool estEnTrainDeFlicker = false;

    void Start()
    {
        // On cherche le SpriteRenderer sur cet objet ou ses enfants pour manipuler l'affichage.
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Si l'effet de clignotement est actif, on ajuste l'opacité du sprite.
        if (estEnTrainDeFlicker)
        {
            flickerTimer -= Time.deltaTime;
            
            // On alterne l'alpha entre 0.3 et 1.0 pour créer l'effet de clignotement.
            float alpha = (Mathf.Sin(flickerTimer * 50f) > 0) ? 0.3f : 1.0f;
            if (sr != null) sr.color = new Color(1, 1, 1, alpha);

            // Une fois le temps écoulé, on réinitialise l'opacité à la normale.
            if (flickerTimer <= 0)
            {
                estEnTrainDeFlicker = false;
                if (sr != null) sr.color = Color.white;
            }
        }
    }

    // Méthode publique appelée par d'autres scripts pour déclencher l'effet de dégâts.
    public void DeclencherFlicker(float duree)
    {
        flickerTimer = duree;
        estEnTrainDeFlicker = true;
    }
}