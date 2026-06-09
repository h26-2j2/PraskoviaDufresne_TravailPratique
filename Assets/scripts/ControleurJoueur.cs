using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControleurJoueur : MonoBehaviour
{
    // --- PARAMÈTRES DE MOUVEMENT ---
    public float forceDeplacement = 500f;
    public float vitesseDeplacement = 5f;
    public float forceSaut = 12f; 
    public Transform spriteObjet;
    public int objectifCanettes = 10;
    public Transform pointDeDepart;
    public int vies = 5;

    // --- INPUTS ---
    public InputAction mouvementAction;
    public InputAction sautAction;
    public InputAction ramasserAction;

    // --- AUDIO ---
    [Header("Paramètres Audio")]
    public AudioClip sonMarche;
    public AudioClip sonImpactSaut;
    public AudioClip sonRamasser;
    public AudioClip sonBlessé;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 directionInput;
    private GestionnaireDegats gestionnaireDegats;

    // --- ÉTATS DU JOUEUR ---
    private bool estEnLair = false;
    private float altitude = 0f;
    private float vitesseSaut = 0f;
    private float graviteManuelle = -24f; 
    private int scoreCanettes = 0;
    private float timerNoyade = 0f;
    private bool estDansLEau = false;
    private bool estSurUnNenuphar = false;

    private GameObject canetteProche = null;
    private Transform currentNenuphar = null;

    private void OnEnable()
    {
        // On active les actions d'entrée dès que l'objet est actif pour que le joueur puisse bouger immédiatement.
        mouvementAction.Enable();
        sautAction.Enable();
        ramasserAction.Enable();
    }

    private void OnDisable()
    {
        // On désactive les entrées pour éviter des comportements imprévus si le script est arrêté.
        mouvementAction.Disable();
        sautAction.Disable();
        ramasserAction.Disable();
    }

    void Start()
    {
        // On récupère les composants essentiels au démarrage pour éviter de les chercher inutilement dans l'Update.
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (spriteObjet != null) { anim = spriteObjet.GetComponent<Animator>(); }
        gestionnaireDegats = GetComponent<GestionnaireDegats>();
    }

    void Update()
    {
        directionInput = mouvementAction.ReadValue<Vector2>();
        GestionnaireJeu manager = FindFirstObjectByType<GestionnaireJeu>();

        // Si le nénuphar disparaît (détruit), on nettoie ma référence pour éviter les erreurs.
        if (currentNenuphar != null && !currentNenuphar) currentNenuphar = null;

        // --- LOGIQUE DE SAUT ---
        // Si le joueur n'est pas déjà en l'air, on permets au joueur de sauter en réinitialisant les états de "snap".
        if (sautAction.WasPressedThisFrame() && !estEnLair)
        {
            estEnLair = true;
            vitesseSaut = forceSaut;
            currentNenuphar = null;
            estSurUnNenuphar = false;
           
            if (anim != null) anim.SetTrigger("Sauter");

            if (audioSource != null && sonImpactSaut != null)
            {
                audioSource.PlayOneShot(sonImpactSaut);
            }
        }

        if (estEnLair)
        {
            // On applique une gravité manuelle pour simuler une trajectoire de saut plaisante à jouer.
            vitesseSaut += graviteManuelle * Time.deltaTime;
            altitude += vitesseSaut * Time.deltaTime;
            
            if (altitude <= 0) 
            { 
                altitude = 0; 
                estEnLair = false; 
                // À l'atterrissage, on vérifie si le joueur est sur un nénuphar pour qu'il se "colle" dessus.
                if (currentNenuphar != null)
                {
                    estSurUnNenuphar = true;
                }
            }
            if (spriteObjet != null) spriteObjet.localPosition = new Vector3(0, altitude, 0);
        }

        // --- LOGIQUE DE SNAP ---
        // On force la position du joueur à celle du nénuphar pour qu'il suive parfaitement le mouvement.
        if (currentNenuphar != null && !estEnLair && estSurUnNenuphar)
        {
            transform.position = currentNenuphar.position;
        }

        // --- LOGIQUE DE NOYADE ---
        // Si le joueur touche l'eau sans être sur un nénuphar, on lance un chrono de survie.
        if (estDansLEau && !estSurUnNenuphar)
        {
            timerNoyade += Time.deltaTime;
            if (timerNoyade >= 1.0f)
            {
                vies--;
                if (audioSource != null && sonBlessé != null) audioSource.PlayOneShot(sonBlessé);
                if (gestionnaireDegats != null) gestionnaireDegats.DeclencherFlicker(0.6f);
                if (manager != null) { manager.MettreAJourVies(vies); }
                
                if (vies <= 0)
                {
                    if (manager != null) { manager.ActiverDefaite(); }
                    this.enabled = false;
                }
                else
                {
                    transform.position = pointDeDepart.position;
                    timerNoyade = 0f;
                }
            }
        }
        else
        {
            timerNoyade = 0f;
        }

        // --- LOGIQUE ANIMATIONS ---
        // On mets à jour l'animateur en fonction de la direction pour que les visuels correspondent aux touches.
        if (anim != null)
        {
            anim.SetFloat("vitesse", directionInput.magnitude);
            if (directionInput.magnitude > 0.1f)
            {
                if (Mathf.Abs(directionInput.y) >= Mathf.Abs(directionInput.x))
                {
                    anim.SetInteger("direction", directionInput.y > 0 ? 1 : 2);
                }
                else
                {
                    anim.SetInteger("direction", 0);
                    spriteObjet.localScale = new Vector3(directionInput.x > 0 ? -1 : 1, 1, 1);
                }
            }
        }

        // --- RAMASSAGE ---
        // Si le joueur appuie sur "ramasser", on détruis l'objet proche et on avertis le gestionnaire de jeu.
        if (ramasserAction.WasPressedThisFrame())
        {
            if (canetteProche != null)
            {
                if (anim != null) anim.SetTrigger("Ramasser");
                if (manager != null) { manager.AjouterType(canetteProche.tag); }
                Destroy(canetteProche);
                canetteProche = null;
                scoreCanettes++;

                if (audioSource != null && sonRamasser != null)
                {
                    audioSource.PlayOneShot(sonRamasser);
                }
            }
        }

        GererSonMarche();
    }

    void FixedUpdate()
    {
        // Utilise FixedUpdate pour la physique. Si le joueur est sur un nénuphar, il passe en mode Kinematic
        // pour que la physique du joueur ne bloque pas celle de la plateforme.
        if (currentNenuphar != null && !estEnLair && estSurUnNenuphar && currentNenuphar.gameObject.activeInHierarchy)
        {
            rb.linearVelocity = Vector2.zero;
            if (rb.bodyType != RigidbodyType2D.Kinematic) rb.bodyType = RigidbodyType2D.Kinematic;
            
            if (this.gameObject.activeInHierarchy && currentNenuphar.gameObject.activeInHierarchy)
            {
                transform.position = currentNenuphar.position;
            }
        }
        else
        {
            // Sinon, on reviens en Dynamic pour permettre les déplacements classiques.
            if (rb.bodyType != RigidbodyType2D.Dynamic) rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = directionInput * vitesseDeplacement;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // détecte quand le joueur touche un nénuphar, l'eau, ou un objet collectable.
        if (collision.CompareTag("Nenuphar"))
        {
            currentNenuphar = collision.transform;
            if (!estEnLair) estSurUnNenuphar = true;
        }
        else if (collision.CompareTag("Eau"))
        {
            estDansLEau = true;
        }
        else if (collision.CompareTag("Canette") || collision.CompareTag("Bouteille") || collision.CompareTag("SacPoubelle"))
        {
            canetteProche = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Quand le joueur quitte un trigger, on réinitialise les variables pour éviter que l'état ne reste "coincé".
        if (collision.CompareTag("Nenuphar"))
        {
            if (!estEnLair)
            {
                currentNenuphar = null;
                estSurUnNenuphar = false;
            }
        }
        else if (collision.CompareTag("Eau"))
        {
            estDansLEau = false;
        }
        else if (collision.CompareTag("Canette") || collision.CompareTag("Bouteille") || collision.CompareTag("SacPoubelle"))
        {
            if (canetteProche == collision.gameObject) canetteProche = null;
        }
    }

    private void GererSonMarche()
    {
        // Gestion propre de la boucle sonore : joue le son que si le joueur est actif et se déplace.
        if (audioSource == null || sonMarche == null) return;

        if (directionInput.magnitude > 0.1f && !estEnLair)
        {
            if (audioSource.clip != sonMarche || !audioSource.isPlaying)
            {
                audioSource.clip = sonMarche;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying && audioSource.clip == sonMarche)
            {
                audioSource.Stop();
                audioSource.clip = null; 
            }
        }
    }
}