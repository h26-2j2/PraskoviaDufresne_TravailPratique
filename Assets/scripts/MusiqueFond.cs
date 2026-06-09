using UnityEngine;

public class MusiqueFond : MonoBehaviour
{
    void Awake()
    {
        // On cherche un objet nommé "MusicManager" dans la hiérarchie de la scène.
        GameObject musicObject = GameObject.Find("MusicManager");

        // Si un gestionnaire de musique existe déjà et qu'il ne s'agit pas de l'instance actuelle,
        // on détruit cet objet pour éviter les doublons sonores lors des changements de scène.
        if (musicObject != null && musicObject != this.gameObject)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // Sinon, on marque cet objet pour qu'il persiste lors du chargement de nouvelles scènes,
            // garantissant ainsi que la musique de fond continue sans interruption.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}