using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionnaireMenu : MonoBehaviour
{
    // Appelé par le bouton "Jouer"
    public void LancerJeu()
    {
        SceneManager.LoadScene("Niveau1"); //Pour permettre que le menu puisse accéder au premier niveau,
                                           // le reste des changements de niveau est codé dans GestionnaireJeu.cs
    }
}