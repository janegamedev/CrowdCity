using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Variables/Bool variable")]
    public class BoolVariable : ScriptableObject
    {
        public bool value;

        public void Set(bool v)
        {
            value = v;
        }
    }
}