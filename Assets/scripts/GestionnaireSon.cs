using UnityEngine;

public class GestionnaireSon : MonoBehaviour
{
    // --- INSTANCE ET COMPOSANTS ---
    // On utilise un pattern Singleton pour que ce gestionnaire soit accessible partout dans le jeu.
    public static GestionnaireSon instance;
    private AudioSource sourceAudio;

    void Awake()
    {
        // On vérifie s'il existe déjà une instance de ce gestionnaire.
        if (instance == null)
        {
            instance = this;
            // On s'assure que cet objet persiste entre les changements de scène.
            DontDestroyOnLoad(gameObject);
            sourceAudio = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            // On détruit les instances en double pour garantir l'unicité du Singleton.
            Destroy(gameObject);
        }
    }

    public void JouerSon(AudioClip clipSon)
    {
        // On vérifie que la source audio et le clip existent avant de lancer la lecture.
        if (sourceAudio != null && clipSon != null)
        {
            sourceAudio.PlayOneShot(clipSon);
        }
    }
}