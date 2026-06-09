using UnityEngine;

public class ObjetsCollectables : MonoBehaviour
{
    // --- IDENTIFICATION ---
    // Cette variable permet au script du joueur de vérifier le type de déchet collecté
    // afin de mettre à jour le bon compteur dans le GestionnaireJeu.
    public string typeObjet; 

    // Note : Le traitement des collisions et de la collecte est géré directement 
    // par le script 'ControleurJoueur'. Cela permet de centraliser la logique 
    // d'interaction et de garantir que le joueur doit appuyer sur une touche 
    // spécifique pour ramasser l'objet. Ce script est spécifiquement pour le niveau 3. 
}