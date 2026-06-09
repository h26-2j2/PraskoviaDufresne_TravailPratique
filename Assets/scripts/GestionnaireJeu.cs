using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GestionnaireJeu : MonoBehaviour
{
    // --- COMPTEURS ET OBJECTIFS ---
    public int countCanettes = 0;
    public int countBouteilles = 0;
    public int countSacs = 0;
    public int objectifCanettes = 5; 
    public int objectifBouteilles = 5;
    public int objectifSacs = 5;
    public int vies = 5; 
    
    // --- ÉLÉMENTS D'INTERFACE ---
    public TextMeshProUGUI textCanettes;
    public TextMeshProUGUI textBouteilles;
    public TextMeshProUGUI textSacs;
    public TextMeshProUGUI vieText;
    public TextMeshProUGUI scoreFinalText; 
    
    public GameObject victoirePanel; 
    public GameObject defaitePanel;

    // --- AUDIO ---
    public AudioClip sonVictoire;
    public AudioClip sonDefaite;

    void Start()
    {
        // On s'assure que le temps est bien écoulé au début de la partie.
        Time.timeScale = 1f; 
        // On désactive les menus de fin de partie au chargement.
        if(victoirePanel != null) victoirePanel.SetActive(false);
        if(defaitePanel != null) defaitePanel.SetActive(false);
        UpdateUI();
    }

    private void ShowResults(bool estVictoire)
    {
        // On génère le texte de résumé avec les scores finaux.
        if (scoreFinalText != null)
        {
            string resultats = "Déchets collectés:\n";
            if (objectifCanettes > 0) resultats += $"Canettes: {countCanettes} / {objectifCanettes}\n";
            if (objectifBouteilles > 0) resultats += $"Bouteilles: {countBouteilles} / {objectifBouteilles}\n";
            if (objectifSacs > 0) resultats += $"Sacs: {countSacs} / {objectifSacs}";
            scoreFinalText.text = resultats;
        }

        // On affiche le panneau correspondant et on joue le son approprié.
        if (estVictoire)
        {
            if (victoirePanel != null) victoirePanel.SetActive(true);
            if (GestionnaireSon.instance != null && sonVictoire != null) 
                GestionnaireSon.instance.JouerSon(sonVictoire);
        }
        else
        {
            if (defaitePanel != null) defaitePanel.SetActive(true);
            if (GestionnaireSon.instance != null && sonDefaite != null) 
                GestionnaireSon.instance.JouerSon(sonDefaite);
        }
        
        // On fige le temps pour arrêter le jeu.
        Time.timeScale = 0f;
    }

    public void MettreAJourVies(int nouvellesVies) 
    { 
        vies = nouvellesVies; 
        UpdateUI(); 
    }

    public void ActiverDefaite() 
    { 
        ShowResults(false); 
    }

    public void AjouterType(string type) 
    { 
        // On incrémente le compteur correspondant au type de déchet collecté.
        if (type == "Canette") { if (countCanettes < objectifCanettes) countCanettes++; }
        else if (type == "Bouteille") { if (countBouteilles < objectifBouteilles) countBouteilles++; }
        else if (type == "SacPoubelle") { if (countSacs < objectifSacs) countSacs++; }
        
        UpdateUI();
        CheckVictory();
    }

    private void CheckVictory()
    {
        // On vérifie si tous les objectifs sont atteints pour déclencher la victoire.
        if (countCanettes >= objectifCanettes && countBouteilles >= objectifBouteilles && countSacs >= objectifSacs)
            ShowResults(true);
    }

    public void UpdateUI()
    {
        // On met à jour les textes de l'interface utilisateur.
        if (textCanettes != null) textCanettes.text = $"Canettes: {countCanettes} / {objectifCanettes}";
        if (textBouteilles != null) textBouteilles.text = $"Bouteilles: {countBouteilles} / {objectifBouteilles}";
        if (textSacs != null) textSacs.text = $"Sacs: {countSacs} / {objectifSacs}";
        if (vieText != null) vieText.text = "Vies: " + vies;
    }

    // --- NAVIGATION ---
    public void RecommencerNiveau() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void ChargerNiveauSuivant() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void RetournerAuMenu() 
    { 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
}