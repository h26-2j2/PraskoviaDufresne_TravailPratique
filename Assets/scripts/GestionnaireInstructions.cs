using UnityEngine;
using TMPro;

public class GestionnaireTutoriel : MonoBehaviour
{
    // --- ÉLÉMENTS D'INTERFACE ---
    public GameObject tutorielBulle;     // Référence au panneau contenant la bulle d'aide
    public GameObject fondTutoriel;      // Référence au panneau d'arrière-plan du tutoriel
    public TextMeshProUGUI textInstruction; // Référence au composant texte affichant les instructions

    public string texteDuNiveau; // Texte spécifique au niveau, modifiable depuis l'inspecteur

    void Start()
    {
        // On fige le temps au début du niveau pour permettre au joueur de lire les instructions.
        Time.timeScale = 0f;
        
        // On active les éléments d'interface du tutoriel.
        tutorielBulle.SetActive(true);
        fondTutoriel.SetActive(true);
        
        // On concatène le texte spécifique au niveau à l'instruction de base.
        textInstruction.text += "\n" + texteDuNiveau;
    }

    public void FermerTutoriel()
    {
        // On rétablit l'écoulement normal du temps.
        Time.timeScale = 1f;
        
        // On désactive les éléments d'interface du tutoriel une fois la lecture terminée.
        tutorielBulle.SetActive(false);
        fondTutoriel.SetActive(false);
    }
}