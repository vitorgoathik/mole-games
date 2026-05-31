using UnityEngine;
using UnityEditor;
using Platformer.Evolution;

public static class EvolutionFormSetup
{
    [MenuItem("evo_dash/Create Default Evolution Forms")]
    public static void CreateDefaultForms()
    {
        CreateForm("Form_Swift", new FormTraits
        {
            maxSpeed = 12f,
            jumpTakeOffSpeed = 7f,
            jumpModifier = 1.5f,
            canDoubleJump = false,
            canDash = true,
            dashSpeed = 18f,
            dashCooldown = 1f
        }, new Color(0.4f, 0.8f, 1f));

        CreateForm("Form_Jumper", new FormTraits
        {
            maxSpeed = 6f,
            jumpTakeOffSpeed = 11f,
            jumpModifier = 1.8f,
            canDoubleJump = true,
            canDash = false,
            dashSpeed = 0f,
            dashCooldown = 0f
        }, new Color(0.6f, 1f, 0.4f));

        CreateForm("Form_Tank", new FormTraits
        {
            maxSpeed = 4f,
            jumpTakeOffSpeed = 5f,
            jumpModifier = 1.2f,
            canDoubleJump = false,
            canDash = false,
            dashSpeed = 0f,
            dashCooldown = 0f
        }, new Color(1f, 0.5f, 0.2f));

        CreateForm("Form_Apex", new FormTraits
        {
            maxSpeed = 10f,
            jumpTakeOffSpeed = 9f,
            jumpModifier = 1.6f,
            canDoubleJump = true,
            canDash = true,
            dashSpeed = 16f,
            dashCooldown = 0.8f
        }, new Color(1f, 0.3f, 0.9f));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[evo_dash] Default evolution forms created in Assets/Evolution/Forms/");
    }

    static void CreateForm(string assetName, FormTraits traits, Color tint)
    {
        const string folder = "Assets/Evolution/Forms";
        if (!AssetDatabase.IsValidFolder("Assets/Evolution"))
            AssetDatabase.CreateFolder("Assets", "Evolution");
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets/Evolution", "Forms");

        var form = ScriptableObject.CreateInstance<EvolutionForm>();
        form.formName = assetName.Replace("Form_", "");
        form.tintColor = tint;
        form.traits = traits;

        AssetDatabase.CreateAsset(form, $"{folder}/{assetName}.asset");
    }
}
