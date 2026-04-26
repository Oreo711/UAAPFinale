using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment
{
    public interface ICoroutinesPerformer
    {
        Coroutine StartCoroutine(IEnumerator coroutineFunction);
        void StopCoroutine(Coroutine coroutine);
    }
}
