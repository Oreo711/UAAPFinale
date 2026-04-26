using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment
{
    public class CoroutinesPerformer : MonoBehaviour, ICoroutinesPerformer
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public Coroutine StartCoroutine(IEnumerator coroutineFunction)
            => base.StartCoroutine(coroutineFunction);

        public void StopCoroutine(Coroutine coroutine)
            => base.StopCoroutine(coroutine);
    }
}