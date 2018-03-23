﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerBehaviour : MonoBehaviour, IInteractable, IContainer
{  
    // fields
    public Container container_config;
    [SerializeField]
    private Container container_runtime;

    // properties
    public Container Container
    {
        get { return container_runtime; }
    }    

    // Unity methods
    private void Start()
    {
        container_runtime = Instantiate(container_config);
    }

    // methods
    public void AddContent(Object obj)
    {
        container_runtime.AddContent((Item)obj);
    }

    public void RemoveContent(Object obj)
    {
        container_runtime.RemoveContent((Item)obj);
    }   

    // =========== Interaction System Implementation
    public GameObject Interactor;
    [SerializeField]
    private GameEventArgs Interactor_Set;
    [SerializeField]
    private GameEventArgs Interactor_Release;
    [SerializeField]
    private GameEventArgs ContiainerOpen;
    [SerializeField]
    private GameEventArgs ContainerClose;

    [SerializeField]
    private bool interacting = false;
    public void Interact(object token)
    {
        if (interacting)
        {
            //close it
            interacting = false;
            ContainerClose.Raise(gameObject);
        }
        else
        {
            //open it
            interacting = true;
            var data = ScriptableObject.CreateInstance<ContainerEventData>().Init(container_runtime);
            ContiainerOpen.Raise(data, gameObject);
        }
    }

    public void SetInteractor(params Object[] args)
    {   
        Interactor = (GameObject)args[1];
        Interactor.GetComponent<IInteractor>().Interaction_Set(this);
        Interactor_Set.Raise(gameObject, Interactor);
    }

    public void ReleaseInteractor(params Object[] args)
    {
        if (args[0] == gameObject && Interactor != null)
        {
            Interactor.GetComponent<IInteractor>().Interaction_Release();
            Interactor_Release.Raise(gameObject, Interactor);
            Interactor = null;
        }
    }
}
