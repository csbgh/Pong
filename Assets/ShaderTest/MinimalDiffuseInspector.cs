using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class MinimalDiffuseInspector : ShaderGUI
{
    override public void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // render the shader properties using the default GUI
        base.OnGUI(materialEditor, properties);

        // get the current keywords from the material
        Material targetMat = materialEditor.target as Material;
        string[] keyWords = targetMat.shaderKeywords;

        /*#pragma multi_compile RIM_LIGHTING_ON RIM_LIGHTING_OFF
        #pragma multi_compile SPECULAR_ON SPECULAR_OFF
        #pragma multi_compile EMISSIVE_ON EMISSIVE_OFF*/
        bool rimLighting = keyWords.Contains("RIM_LIGHTING_ON");
        bool specular = keyWords.Contains("SPECULAR_ON");
        bool emissive = keyWords.Contains("EMISSIVE_ON");

        EditorGUI.BeginChangeCheck();
        rimLighting = EditorGUILayout.Toggle("Rim Lighting", rimLighting);
        specular = EditorGUILayout.Toggle("Specular", specular);
        emissive = EditorGUILayout.Toggle("Emissive", emissive);
        if (EditorGUI.EndChangeCheck())
        {
            // if the checkbox is changed, reset the shader keywords
            var keywords = new List<string>();
            keywords.Add(rimLighting ? "RIM_LIGHTING_ON" : "RIM_LIGHTING_OFF");
            keywords.Add(specular ? "SPECULAR_ON" : "SPECULAR_OFF");
            keywords.Add(emissive ? "EMISSIVE_ON" : "EMISSIVE_OFF");
            targetMat.shaderKeywords = keywords.ToArray();
            EditorUtility.SetDirty(targetMat);
        }
    }

}
#endif