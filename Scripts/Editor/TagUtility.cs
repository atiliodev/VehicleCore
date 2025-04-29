using UnityEditor;
using UnityEngine;

public static class TagUtility
{
    /// <summary>
    /// Verifica se as tags existem e cria se não existirem.
    /// </summary>
    /// <param name="tagName">VehicleCore</param>
    public static void CreateTagIfNotExists(string tagName)
    {
        if (string.IsNullOrEmpty(tagName))
        {
            Debug.LogError("Error in the tag name");
            return;
        }

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        // Verifica se a tag já existe
        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(tagName))
            {
                Debug.Log($"The '{tagName}' tag already exists.");
                return;
            }
        }

        // Se não existir, adiciona a nova tag
        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
        SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
        newTag.stringValue = tagName;

        tagManager.ApplyModifiedProperties();
        Debug.Log($"'{tagName}' tag was created successfully");
    }
}

