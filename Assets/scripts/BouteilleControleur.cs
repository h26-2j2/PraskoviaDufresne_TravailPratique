using UnityEngine;

public class BouteilleControleur : MonoBehaviour
{
    // --- PARAMÈTRES DE MOUVEMENT ---
    public float vitesseDerive = 2f;      // Vitesse de déplacement horizontal
    public float amplitudeBobbing = 0.15f; // Intensité de l'oscillation verticale
    public float vitesseBobbing = 2f;      // Vitesse de l'oscillation verticale

    private Vector3 positionDepart;
    private float offset;                  // Décalage pour varier le flottement de chaque instance
    private float spawnX;                  // Position X initiale pour gérer la destruction

    void Start() 
    {
        // On initialise la position de départ et on génère un décalage aléatoire.
        positionDepart = transform.position;
        spawnX = transform.position.x;
        offset = Random.Range(0f, 6.28f);
    }

    void Update() 
    {
        // On calcule le déplacement horizontal constant.
        positionDepart += Vector3.right * vitesseDerive * Time.deltaTime;
        
        // On calcule l'effet de flottement (bobbing) via une fonction sinusoïdale.
        // L'utilisation de 'Time.time' permet une animation continue et fluide.
        float bobbing = Mathf.Sin(Time.time * vitesseBobbing + offset) * amplitudeBobbing;
        
        // On met à jour la position en combinant le vecteur de base et l'oscillation verticale.
        transform.position = new Vector3(positionDepart.x, positionDepart.y + bobbing, positionDepart.z);

        // On détruit l'objet s'il parcourt une distance trop grande par rapport à son point de spawn.
        if (Mathf.Abs(transform.position.x - spawnX) > 35f) 
        {
            Destroy(gameObject);
        }
    }
}