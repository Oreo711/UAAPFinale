using System.Collections;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    public void Link(Entity entity)
    {
        entity.Initialized += OnEntityStartedWork;
    }

    public virtual void Cleanup(Entity entity)
    {
        entity.Initialized -= OnEntityStartedWork;
    }

    protected abstract void OnEntityStartedWork(Entity entity);
}
