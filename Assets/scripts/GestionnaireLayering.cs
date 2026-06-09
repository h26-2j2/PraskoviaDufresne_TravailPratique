using UnityEngine;

public class GestionnaireLayering : MonoBehaviour
{
    // --- PARAMÈTRES DE TRI ---
    public int baseSortingOrder = 5;      // Valeur de base pour le tri des calques
    public float precisionMultiplier = 10f; // Multiplicateur pour ajuster la sensibilité du changement de calque
    public float verticalOffset = 0.5f;     // Décalage vertical pour ajuster le point de pivot du tri

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // On récupère le composant SpriteRenderer au démarrage.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Si aucun SpriteRenderer n'est présent, on arrête l'exécution pour cet objet.
        if (spriteRenderer == null)
        {
            return;
        }

        // On calcule une position verticale ajustée par l'offset.
        float adjustedY = transform.position.y - verticalOffset;
        
        // On calcule le nouvel ordre de tri en fonction de la position Y : 
        // plus l'objet est bas (Y petit), plus son ordre de tri est élevé.
        int newOrder = baseSortingOrder - Mathf.RoundToInt(adjustedY * precisionMultiplier);
        
        // On applique le nouvel ordre, en s'assurant qu'il ne descende pas en dessous d'une limite définie.
        spriteRenderer.sortingOrder = Mathf.Max(newOrder, 20); 
    }
}