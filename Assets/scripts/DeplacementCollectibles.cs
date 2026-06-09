using UnityEngine;

public class DeplacementCollectibles : MonoBehaviour
{
    // --- PARAMÈTRES DE MOUVEMENT ---
    public float vitesseDerive = 2f;      // Vitesse de déplacement horizontal
    public float amplitudeBobbing = 0.15f; // Hauteur de l'oscillation verticale
    public float vitesseBobbing = 2f;      // Vitesse de l'oscillation verticale

    private Vector3 positionDepart;
    private float spawnX;
    private float offset;

    void Start() 
    {
        // On enregistre la position initiale pour permettre un mouvement relatif.
        positionDepart = transform.position;
        spawnX = transform.position.x;
        // On génère un décalage aléatoire pour que chaque objet ait une phase de flottement différente.
        offset = Random.Range(0f, 6.28f);
    }

    void Update() 
    {
        // On met à jour la position de base en déplaçant l'objet vers la droite.
        positionDepart += Vector3.right * vitesseDerive * Time.deltaTime;
        
        // On calcule l'effet de flottement (bobbing) à l'aide d'une fonction sinusoïdale.
        float bobbing = Mathf.Sin(Time.time * vitesseBobbing + offset) * amplitudeBobbing;
        
        // On applique la nouvelle position en combinant le déplacement horizontal et l'oscillation verticale.
        transform.position = new Vector3(positionDepart.x, positionDepart.y + bobbing, positionDepart.z);

        // On détruit l'objet s'il s'éloigne trop de son point d'origine pour optimiser les performances.
        if (Mathf.Abs(transform.position.x - spawnX) > 35f) 
        {
            Destroy(gameObject);
        }
    }
}