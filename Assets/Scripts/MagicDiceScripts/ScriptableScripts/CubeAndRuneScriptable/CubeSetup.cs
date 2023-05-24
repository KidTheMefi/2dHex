using UnityEngine;

namespace ScriptableScripts
{
    public abstract class CubeSetup : ScriptableObject
    {
        public abstract RuneScriptable[] GetCubeRuneScriptable();
    }
}