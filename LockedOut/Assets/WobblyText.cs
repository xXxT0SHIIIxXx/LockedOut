using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class WobblyText : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    public float speed;
    public float freq;
    public float amplitude;
    // Update is called once per frame
    void Update()
    {
        textComponent.ForceMeshUpdate();
        var info = textComponent.textInfo;

        for (int i = 0; i < info.characterCount; i++)
        {
            var charinfo = info.characterInfo[i];

            if (!charinfo.isVisible)
            {
                continue;
            }

            var verts = info.meshInfo[charinfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charinfo.vertexIndex + j];

                verts[charinfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * speed + orig.x * freq) * amplitude, 0);
            }
        }

        for (int i = 0; i < info.meshInfo.Length; i++)
        {
            var meshInfo = info.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh,i);
        }
    }
}
