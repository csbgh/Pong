using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class ZPUnityEditor : Editor
{
    protected GUIStyle bold_style = new GUIStyle();
    protected GUIStyle foldout_bold_style = new GUIStyle();

    void Awake()
    {
        bold_style.fontStyle = FontStyle.Bold;
        //bold_style.normal.textColor = new Color32(190, 190, 190, 255);
        bold_style.normal.textColor = new Color32(50, 50, 50, 255);
        //bold_style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        foldout_bold_style = new GUIStyle(EditorStyles.foldout);
        foldout_bold_style.fontStyle = FontStyle.Bold;
    }

    protected bool Foldout(bool foldout_bool, string text, bool foldout_bold = true)
    {
        if (foldout_bold)
        {
            return EditorGUILayout.Foldout(foldout_bool, text, foldout_bold_style);
        }
        else
        {
            return EditorGUILayout.Foldout(foldout_bool, text);
        }
    }

    protected void Foldout(SerializedProperty foldout_bool, string text, bool foldout_bold = true)
    {
        if(foldout_bold)
        {
            foldout_bool.boolValue = EditorGUILayout.Foldout(foldout_bool.boolValue, text, foldout_bold_style);
        }
        else
        {
            foldout_bool.boolValue = EditorGUILayout.Foldout(foldout_bool.boolValue, text);
        }
    }

    protected void Seperator()
    {
        EditorGUILayout.Separator();
    }

    protected void StartGroup(string title, bool seperator = true, bool indent = true)
    {
        if (seperator)
            EditorGUILayout.Separator();

        EditorGUILayout.LabelField(title, bold_style);

        if(indent)
            EditorGUI.indentLevel++;
    }

    protected void EndGroup(bool indent = true)
    {
        if(indent)
            EditorGUI.indentLevel--;
    }

    protected void BeginHorizontal()
    {
        EditorGUILayout.BeginHorizontal();
    }

    protected void EndHorizontal()
    {
        EditorGUILayout.EndHorizontal();
    }

    protected void BeginDisabled(bool enabled = true)
    {
        if (enabled)
        {
            EditorGUI.BeginDisabledGroup(true);
        }
    }

    protected void EndDisabled(bool enabled = true)
    {
        if (enabled)
        {
            EditorGUI.EndDisabledGroup();
        }
    }

    protected void ResizeArray(SerializedProperty prop, int size)
    {
        if (prop.arraySize != size)
        {
            for (int i = 0; i < Mathf.Max(prop.arraySize, size);)
            {
                if (i + 1 > size)
                {
                    prop.DeleteArrayElementAtIndex(i);
                }
                else if (i + 1 > prop.arraySize)
                {
                    prop.InsertArrayElementAtIndex(prop.arraySize);
                    ++i;
                }
                else
                {
                    ++i;
                }
            }
        }
    }

    protected void ArrayRow(string title, SerializedProperty prop)
    {
        BeginHorizontal();
        Label(title);
        for (int i = 0; i < prop.arraySize; ++i)
        {
            //GUIStyle g = new GUIStyle(GUI.skin.textField);
            //g.fixedWidth = 30.0f;
            //prop.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(prop.GetArrayElementAtIndex(i).floatValue, g);
            prop.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(prop.GetArrayElementAtIndex(i).floatValue);
        }
        EndHorizontal();
    }

    protected void FloatArrayRow(string title, string property_name, SerializedProperty[] props)
    {
        BeginHorizontal();
        Label(title);
        for (int i = 0; i < props.Length; ++i)
        {
            if (props[i].FindPropertyRelative(property_name) != null)
                props[i].FindPropertyRelative(property_name).floatValue = EditorGUILayout.FloatField(props[i].FindPropertyRelative(property_name).floatValue);
            else
                props[i].FindPropertyRelative(property_name).floatValue = EditorGUILayout.FloatField(0.0f);
        }
        EndHorizontal();
    }

    protected void IntArrayRow(string title, string property_name, SerializedProperty[] props)
    {
        BeginHorizontal();
        Label(title);
        for (int i = 0; i < props.Length; ++i)
        {
           // props[i].FindPropertyRelative(property_name).floatValue = EditorGUILayout.FloatField(props[i].FindPropertyRelative(property_name).floatValue);
            props[i].FindPropertyRelative(property_name).intValue = EditorGUILayout.IntField(props[i].FindPropertyRelative(property_name).intValue);
        }
        EndHorizontal();
    }

    protected void IntField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.intValue = EditorGUILayout.IntField(title, prop.intValue);
    }

    protected void FloatField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.floatValue = EditorGUILayout.FloatField(title, prop.floatValue);
    }

    protected bool FloatFieldChanged(string title, SerializedProperty prop)
    {
        if (prop == null)
            return false;

        float start_val = prop.floatValue;
        prop.floatValue = EditorGUILayout.FloatField(title, prop.floatValue);

        return start_val != prop.floatValue;
    }

    protected void Vec2Field(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.vector2Value = EditorGUILayout.Vector2Field(title, prop.vector2Value);
    }

    protected void Vec3Field(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.vector3Value = EditorGUILayout.Vector3Field(title, prop.vector3Value);
    }

    protected void Vec4Field(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.vector4Value = EditorGUILayout.Vector4Field(title, prop.vector4Value);
    }

    protected void ColorField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.colorValue = EditorGUILayout.ColorField(title, prop.colorValue);
    }

    protected void Texture2DField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        Texture2D cur_texture = (Texture2D)prop.objectReferenceValue;
        GUIContent content = new GUIContent();
        prop.objectReferenceValue = (Texture2D)EditorGUILayout.ObjectField(content, cur_texture, typeof(Texture2D), false);
    }

    protected void StringField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.stringValue = EditorGUILayout.TextField(title, prop.stringValue);
    }

    protected void CurveField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.animationCurveValue = EditorGUILayout.CurveField(title, prop.animationCurveValue);
    }

    protected void EnumField(string title, SerializedProperty prop, System.Type type)
    {
        if (prop == null)
            return;

        System.Enum.ToObject(type, prop.enumValueIndex);

        System.Enum e = EditorGUILayout.EnumPopup(title, (System.Enum)System.Enum.ToObject(type, prop.enumValueIndex));
        prop.enumValueIndex = Convert.ToInt32(e);
    }

    protected void EnumIntField(string title, SerializedProperty prop, System.Type type)
    {
        if (prop == null)
            return;

        System.Enum.ToObject(type, prop.intValue);

        System.Enum e = EditorGUILayout.EnumPopup(title, (System.Enum)System.Enum.ToObject(type, prop.intValue));
        prop.intValue = Convert.ToInt32(e);
    }

    protected void MaskField(string title, SerializedProperty prop, System.Type type)
    {
        if (prop == null)
            return;

        string[] values = Enum.GetNames(type);

        prop.intValue = EditorGUILayout.MaskField(title, prop.intValue, values);
    }

    protected void MaskField(string title, SerializedProperty prop, string[] values)
    {
        if (prop == null)
            return;

        prop.intValue = EditorGUILayout.MaskField(title, prop.intValue, values);
    }

    protected void BoolField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.boolValue = EditorGUILayout.Toggle(title, prop.boolValue);
    }

    protected void ObjectField(string title, Type type, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, type, false);
    }

    protected void ObjectField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, typeof(GameObject), false);
    }

    protected void SceneObjectField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, typeof(GameObject), true);
    }

    protected void SceneObjectField(string title, SerializedProperty prop, Type type)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, type, true);
    }

    protected void AudioClipField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, typeof(AudioClip), false);
    }

    protected void MaterialField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        prop.objectReferenceValue = EditorGUILayout.ObjectField(title, prop.objectReferenceValue, typeof(Material), false);
    }

    protected void PropertyField(string title, SerializedProperty prop)
    {
        if (prop == null)
            return;

        EditorGUILayout.PropertyField(prop, true);
    }

    protected void AudioClipArrayField(string length_str, string item_str, SerializedProperty array)
    {
        if (array == null)
            return;

        array.arraySize = EditorGUILayout.IntField(length_str, array.arraySize);

        for (int i = 0; i < array.arraySize; i++)
        {
            AudioClip cur_object = (AudioClip)array.GetArrayElementAtIndex(i).objectReferenceValue;
            AudioClip new_object = (AudioClip)EditorGUILayout.ObjectField(item_str + " " + i.ToString(), cur_object, typeof(AudioClip), false);
            array.GetArrayElementAtIndex(i).objectReferenceValue = new_object;
        }
    }

    protected int Popup(string label, int selected, string [] options)
    {
        return EditorGUILayout.Popup(label, selected, options); 
    }

    protected void Label(string text)
    {
        EditorGUILayout.LabelField(text);
    }

    protected void Label(string text, GUIStyle s)
    {
        EditorGUILayout.LabelField(text, s);
    }

    /*protected SerializedProperty AttributeField(string label, string attribute_name, SerializedProperty base_attributes)
    {
        SerializedProperty attribute_dictionary = base_attributes.FindPropertyRelative("attributes");

        SerializedProperty keys = attribute_dictionary.FindPropertyRelative("keys");
        SerializedProperty values = attribute_dictionary.FindPropertyRelative("values");

        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty key = keys.GetArrayElementAtIndex(i);

            if(key.stringValue == label)
            {
                SerializedProperty attr = values.GetArrayElementAtIndex(i);

                SerializedProperty attribute_type = attr.FindPropertyRelative("attr_type");

                AttributeType cur_attr_type = (AttributeType)attribute_type.enumValueIndex;

                switch (cur_attr_type)
                {
                    case AttributeType.Int:
                        SerializedProperty int_prop = attr.FindPropertyRelative("_int_val");
                        IntField(label, int_prop);
                        break;
                    case AttributeType.Float:
                        SerializedProperty float_prop = attr.FindPropertyRelative("_float_val");
                        FloatField(label, float_prop);
                        break;
                    case AttributeType.Double:
                        SerializedProperty double_prop = attr.FindPropertyRelative("_double_val");
                        FloatField(label, double_prop);
                        break;
                    case AttributeType.Vector2:
                        SerializedProperty vec2_prop = attr.FindPropertyRelative("_vec2_val");
                        Vec2Field(label, vec2_prop);
                        break;
                    case AttributeType.Vector3:
                        SerializedProperty vec3_prop = attr.FindPropertyRelative("_vec3_val");
                        Vec3Field(label, vec3_prop);
                        break;
                    case AttributeType.Vector4:
                        SerializedProperty vec4_prop = attr.FindPropertyRelative("_vec4_val");
                        Vec4Field(label, vec4_prop);
                        break;
                    case AttributeType.String:
                        SerializedProperty string_prop = attr.FindPropertyRelative("_string_val");
                        StringField(label, string_prop);
                        break;
                    default:
                        break;
                }

                return attr;
            }
        }

        return null;
    }

    protected SerializedProperty EnumAttributeField(string name, string field, SerializedProperty base_attributes, Type enum_type)
    {
        SerializedProperty attribute_dictionary = base_attributes.FindPropertyRelative("attributes");

        SerializedProperty keys = attribute_dictionary.FindPropertyRelative("keys");
        SerializedProperty values = attribute_dictionary.FindPropertyRelative("values");

        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty key = keys.GetArrayElementAtIndex(i);

            if (key.stringValue == name)
            {
                SerializedProperty attr = values.GetArrayElementAtIndex(i);

                SerializedProperty attribute_type = attr.FindPropertyRelative("attr_type");

                AttributeType cur_attr_type = (AttributeType)attribute_type.enumValueIndex;

                switch (cur_attr_type)
                {
                    case AttributeType.Enum:
                        SerializedProperty enum_prop = attr.FindPropertyRelative("_enum_index");
                        EnumIntField(name, enum_prop, enum_type);
                        break;
                    default:
                        break;
                }

                return attr;
            }
        }

        return null;
    }*/
}
