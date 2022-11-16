using UnityEngine;
using System;
using System.Collections.Generic;

//[RequireComponent(typeof(MyOutline))]

public class ChestBehavior : MonoBehaviour
{
    private MyOutline outline; 
    [NonSerialized] public bool looted = false;
    [SerializeField] public List<Artifact> items;
    [NonSerialized] public InterfaceManager interfaceManager;  ArtifactManager artifactManager; 
    private Animator animator;

    private void Start() 
    {
        Transform additionalCollider = transform.Find("AdditionalCollider");
        outline = additionalCollider.GetComponent<MyOutline>();
        interfaceManager = FindObjectOfType<InterfaceManager>();  
        animator = gameObject.GetComponent<Animator>();

        artifactManager = FindObjectOfType<ArtifactManager>();
    }

    void Update()
    {      
        if (looted && outline.enabled)
        {
            outline.enabled = false;
        }  
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

        looted = true;
    }

    private void TakeItem(Artifact artifact)
    {
        artifactManager.AddArtifact(artifact, true);
        //CloseAlcGame();
        interfaceManager.messageScript.NewMessage("Успех!", "Вы нашли " + artifact.artName);

    }
}
