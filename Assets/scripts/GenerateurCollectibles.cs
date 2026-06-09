using UnityEngine;

public class GenerateurCollectibles : MonoBehaviour
{
    public GameObject objetSource; // L'objet de base à cloner
    public float intervalle = 3f;  // Temps entre chaque génération
    private float minuterie;
    // Compteur statique permettant d'attribuer un ordre de tri et un décalage unique à chaque objet généré.
    private static int globalSortingCounter = 0; 

    void Start()
    {
        // On désactive l'objet source au démarrage pour qu'il serve uniquement de modèle (prefab).
        if (objetSource != null) objetSource.SetActive(false);
        minuterie = intervalle;
    }

    void Update()
    {
        minuterie -= Time.deltaTime;
        
        if (minuterie <= 0)
        {
            if (objetSource != null)
            {
                // On calcule une position aléatoire pour l'apparition de l'objet.
                Vector3 positionDeSpawn = new Vector3(
                    transform.position.x + Random.Range(-0.8f, 0.8f), 
                    Random.Range(-2f, 2f), 
                    0
                );

                // On crée une instance (clone) de l'objet source.
                GameObject clone = Instantiate(objetSource, positionDeSpawn, Quaternion.identity);
                
                // --- GESTION DU DÉCALAGE ET DU TRI ---
                // On applique un léger décalage sur l'axe Z pour éviter les conflits de rendu (Z-fighting ou masques).
                float zOffset = (globalSortingCounter % 10) * -0.01f;
                clone.transform.position = new Vector3(positionDeSpawn.x, positionDeSpawn.y, zOffset);
                
                // On récupère le SpriteRenderer pour lui assigner un ordre de tri unique,
                // ce qui garantit un affichage correct des objets superposés.
                SpriteRenderer sr = clone.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sortingOrder = 50 + (globalSortingCounter % 10);
                }
                
                globalSortingCounter++;

                clone.SetActive(true);
            }
            
            // On réinitialise la minuterie avec une légère variation aléatoire pour varier le rythme.
            minuterie = intervalle + Random.Range(-0.5f, 0.5f);
        }
    }
}