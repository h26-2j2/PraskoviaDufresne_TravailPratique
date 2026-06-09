using UnityEngine;

public class CanetteControleur : MonoBehaviour
{
    // --- PARAMÈTRES DE MOUVEMENT ET D'ÉTAT ---
    public float vitesseDérive = 2f;
    public float vitesseVersRivage = 3f;
    public float amplitudeFlottement = 0.2f;
    public float vitesseFlottement = 5f;
    public float yShore = -3.0f;           // Position Y cible du rivage
    public float tempsSurRivage = 4.0f;    // Durée de séjour sur le rivage avant de repartir
    
    public bool estÀPortée = false;        // Indique si le joueur est proche pour interagir
    
    private Vector3 positionActive;
    
    // Définition des états de déplacement de la canette
    private enum Etat { DriftRiver, DipToShore, DriftShore, Finished }
    private Etat etatActuel = Etat.DriftRiver;
    
    private float dipTriggerX;
    private float timerSurRivage = 0f;
    private float spawnX;

    void Start() 
    { 
        positionActive = transform.position;
        spawnX = positionActive.x;
        
        // Inclinaison initiale pour un effet visuel plus naturel
        transform.rotation = Quaternion.Euler(0, 0, 15f); 
        // Détermine aléatoirement le point où la canette commence à se diriger vers le rivage
        dipTriggerX = Random.Range(-2.0f, 2.0f);
    }

    void Update()
    {
        // Gestion de la machine à états pour le trajet de la canette
        switch (etatActuel)
        {
            case Etat.DriftRiver:
                positionActive.x += vitesseDérive * Time.deltaTime;
                if (positionActive.x >= dipTriggerX) etatActuel = Etat.DipToShore;
                break;

            case Etat.DipToShore:
                positionActive.x += (vitesseDérive * 0.5f) * Time.deltaTime;
                positionActive.y -= vitesseVersRivage * Time.deltaTime;
                
                // Transition une fois que la canette atteint la profondeur du rivage
                if (positionActive.y <= yShore) {
                    positionActive.y = yShore;
                    etatActuel = Etat.DriftShore;
                }
                break;

            case Etat.DriftShore:
                positionActive.x += (vitesseDérive * 0.8f) * Time.deltaTime;
                timerSurRivage += Time.deltaTime;
                if (timerSurRivage >= tempsSurRivage) etatActuel = Etat.Finished;
                break;
                
            case Etat.Finished:
                positionActive.x += vitesseDérive * Time.deltaTime;
                positionActive.y += 0.5f * Time.deltaTime;
                break;
        }

        // Application de l'oscillation verticale (effet de flottement)
        float bobbingY = Mathf.Sin(Time.time * vitesseFlottement) * amplitudeFlottement;
        transform.position = new Vector3(positionActive.x, positionActive.y + bobbingY, positionActive.z);

        // Nettoyage : destruction de l'objet s'il quitte la zone de jeu définie
        if (Mathf.Abs(transform.position.x - spawnX) > 35f) { 
            Destroy(gameObject);
        }
    }

    // Vérifie si l'interaction est possible (si l'état permet le ramassage et si le joueur est proche)
    public bool EstInteractuable() 
    {
        return (etatActuel == Etat.DriftShore && estÀPortée);
    }
    
    // Détection de la proximité du joueur via les triggers
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) estÀPortée = true;
    }
    
    private void OnTriggerExit2D(Collider2D other) { 
        if (other.CompareTag("Player")) estÀPortée = false; 
    }
}