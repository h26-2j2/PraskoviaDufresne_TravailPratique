using UnityEngine;

public class CanettesSpawner : MonoBehaviour
{
    // --- PARAMÈTRES DE GÉNÉRATION ---
    public GameObject canettePrefab;     // Le modèle (prefab) de la canette à faire apparaître
    public Transform parentCollectibles; // Le conteneur parent pour organiser la hiérarchie
    public float intervalleSpawn = 3f;   // Temps entre chaque génération
    
    // Permet de décaler le point d'apparition pour éviter qu'il n'apparaisse directement sur le spawner
    public Vector3 spawnOffset = new Vector3(-10f, 0f, 0f); 
    
    private float minuterie;

    void Update()
    {
        // On décrémente la minuterie à chaque image.
        minuterie -= Time.deltaTime;

        // Lorsque la minuterie atteint zéro, on déclenche l'apparition.
        if (minuterie <= 0)
        {
            SpawnCanette();
            
            // On réinitialise la minuterie avec une valeur aléatoire pour varier le rythme de jeu.
            minuterie = intervalleSpawn + Random.Range(-0.5f, 1.0f); 
        }
    }

    void SpawnCanette()
    {
        // On calcule la position finale en ajoutant le décalage (offset) à la position du spawner.
        Vector3 spawnPos = transform.position + spawnOffset;
        
        // On instancie la canette à la position calculée, avec une rotation nulle.
        Instantiate(canettePrefab, spawnPos, Quaternion.identity, parentCollectibles);
    }
}