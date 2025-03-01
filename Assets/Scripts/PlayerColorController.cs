using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor
{
    Red,
    Green,
    Blue,
    Yellow
}

public class PlayerColorController : MonoBehaviour
{
    public PlayerColor Color = PlayerColor.Red;

    private Dictionary<string, Material> materialSheet = new Dictionary<string, Material>();
    private PlayerColor currentColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetColor(PlayerColor color)
    {
        Color = color;
    }

    /// <summary>
    /// Sets the color of the player.
    /// </summary>
    /// <param name="colorIndex">The index of the color as defined in the PlayerColor enum.</param>
    public void SetColor(int colorIndex)
    {
        var colorType = typeof(PlayerColor);
        var colors = colorType.GetEnumValues();
        var playerColor = (PlayerColor)colors.GetValue(colorIndex % colors.Length);
        SetColor(playerColor);
    }

    private void LoadMaterialsheet()
    {
        var spritesheetName = string.Format("Fish{0}", Color.ToString());
        var materials = Resources.LoadAll<Material>(spritesheetName);
        materialSheet = materials.ToDictionary(x => x.name, x => x);
        currentColor = Color;
    }
}
