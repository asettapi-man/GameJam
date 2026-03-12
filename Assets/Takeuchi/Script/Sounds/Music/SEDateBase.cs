using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataBase", menuName = "Audio/AudioDataBase")]
public class SEDataBase : ScriptableObject
{
    [System.Serializable]
    public class SEData
    {
        public string name;
        public AudioClip clip;
    }

    public List<SEData> seList = new List<SEData>();

    public SEDataBase seDataBase = new SEDataBase();
    public AudioClip GetSE(string name)
    {
        foreach (var data in seList)
        {
            if (data.name == name)
                return data.clip;
        }

        Debug.LogWarning("SE‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ: " + name);
        return null;
    }
}