using UnityEngine;

public class BouteilleSpawner : MonoBehaviour
{
    // --- PARAMÈTRES DE GÉNÉRATION ---
    public GameObject bouteillePrefab; // Le modèle de bouteille à faire apparaître
    public Transform Collectibles;      // Référence au conteneur parent dans la hiérarchie
    public float intervalle = 3f;      // Délai de base entre deux apparitions
    private float minuterie;           // Temps restant avant la prochaine génération

    void Update()
    {
        // On réduit le temps restant à chaque image.
        minuterie -= Time.deltaTime;

        // Lorsque la minuterie est épuisée, on génère un nouvel objet.
        if (minuterie <= 0)
        {
            // On instancie la bouteille à la position du spawner avec une variation aléatoire sur l'axe Y.
            Instantiate(bouteillePrefab, new Vector3(transform.position.x, Random.Range(-2f, 2f), 0), Quaternion.identity, Collectibles);
            
            // On réinitialise la minuterie avec l'intervalle de base et une variation aléatoire 
            // pour éviter un rythme trop régulier et mécanique.
            minuterie = intervalle + Random.Range(-0.5f, 1.0f);
        }
    }
}