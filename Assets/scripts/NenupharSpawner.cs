using UnityEngine;

public class NenupharSpawner : MonoBehaviour
{
    // --- PARAMÈTRES DE GÉNÉRATION ---
    public GameObject nenupharPrefab;    // Le modèle de nénuphar à faire apparaître
    public Transform objets_flottants;    // Le dossier parent (conteneur) pour les instances créées
    public float intervalle = 3f;         // Le temps moyen entre deux apparitions
    private float minuterie;              // Le compteur interne pour suivre le temps

    void Update()
    {
        // On réduit le temps restant dans la minuterie à chaque image.
        minuterie -= Time.deltaTime;

        // Lorsque la minuterie atteint zéro ou moins, on génère un nouveau nénuphar.
        if (minuterie <= 0)
        {
            // On instancie le nénuphar à la position actuelle du spawner, avec une variation aléatoire sur l'axe Y.
            Instantiate(nenupharPrefab, new Vector3(transform.position.x, Random.Range(-2f, 2f), 0), Quaternion.identity, objets_flottants);
            
            // On réinitialise la minuterie avec l'intervalle de base, incluant une petite variation aléatoire 
            // pour rendre le rythme d'apparition moins prévisible.
            minuterie = intervalle + Random.Range(-0.5f, 1.0f);
        }
    }
}