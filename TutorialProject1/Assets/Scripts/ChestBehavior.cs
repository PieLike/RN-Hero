using UnityEngine;
using System;
using System.Collections.Generic;

//[RequireComponent(typeof(MyOutline))]

public class ChestBehavior : MonoBehaviour
{
    private MyOutline outline; 
    [NonSerialized] public bool looted;
    [SerializeField] public List<Artifact> items;
    [NonSerialized] public InterfaceManager interfaceManager;  ArtifactManager artifactManager; 
    private Animator animator;
    [NonSerialized] public bool canUse; Animator interactionAnimator;

    private void Start() 
    {
        Transform additionalCollider = transform.Find("AdditionalCollider");
        outline = additionalCollider.GetComponent<MyOutline>();
        interfaceManager = FindObjectOfType<InterfaceManager>();  
        animator = gameObject.GetComponent<Animator>();

        artifactManager = FindObjectOfType<ArtifactManager>();

        Transform interactButton = transform.Find("InteractButton");
        interactionAnimator = interactButton.GetComponent<Animator>();
    }

    void Update()
    {      
        if (looted && outline.enabled)
        {
            outline.enabled = false;
        }  

        /*if (canUse && Input.GetKeyDown(KeyCode.E) && MainVariables.inInterface == false)
        {
            Open();
        }*/
    }  

    public void Open() 
    {          
        animator.SetBool("isOpen", true);

        if (items != null  && items.Count > 0)
            TakeItem(items[0]);
        //выбрасываем из него лут
        /*gameObject.GetComponent<LootDroping>().Drop();
        //меняем статус на облутанный и на всякий случай убираем обводку 
        if (items != null && items.Count > 0)
            interfaceManager.newItemMessageScript.ShowItemsMessege(items);*/

        interactionAnimator.SetBool("Show", false);
        canUse = false;
        looted = true;
    }

    private void TakeItem(Artifact artifact)
    {
        artifactManager.AddArtifact(artifact, true);
        interfaceManager.messageScript.NewMessage("Успех!", "Вы нашли " + artifact.artName);
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canUse == false && looted == false)
        {
            interactionAnimator.SetBool("Show", true);
            OnGameStartAndUpdate.AddInteraction(Open);
            canUse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "HeroUse" && canUse && looted == false)
        {
            interactionAnimator.SetBool("Show", false);
            OnGameStartAndUpdate.AddInteraction(Open);
            canUse = false;
        }
    }
}
