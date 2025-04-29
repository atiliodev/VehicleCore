using UnityEditor;
using UnityEngine;

public class TagCreatorMenu
{
    [MenuItem("Tools/Building VehicleCore Tags")]
    public static void CriarMinhaTag()
    {
        TagUtility.CreateTagIfNotExists("VehicleCore");
    }
}
