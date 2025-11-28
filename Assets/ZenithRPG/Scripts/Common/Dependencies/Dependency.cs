using UnityEngine;

namespace DC_ARPG
{
    public abstract class Dependency : MonoBehaviour
    {
        protected virtual void BindAll(MonoBehaviour monoBehaviourInScene) { }

        protected void FindAllObjectsToBind()
        {
            MonoBehaviour[] allMonoInScene = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

            for (int i = 0; i < allMonoInScene.Length; i++)
            {
                BindAll(allMonoInScene[i]);
            }
        }

        protected void Bind<T>(MonoBehaviour bindObject, MonoBehaviour target) where T : class
        {
            if (target is IDependency<T>) (target as IDependency<T>).Construct(bindObject as T);
        }

    }
}
