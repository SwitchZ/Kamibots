using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerrainHUD : MonoBehaviour {
    public Text TerrainField;

    public GameObject terrainPicture;

    public Sprite Plain;
    public Sprite Sand;
    public Sprite Tree;
    public Sprite Mountain;

    void Start () {
	
	}
	

	void Update () {
        //Terrain HUD events
        if (BattleManager.instance.currentTerrain != "")
        {
            //if (!terrainStatus.activeSelf) terrainStatus.SetActive(true);
            if (TerrainField.text != BattleManager.instance.currentTerrain) UpdateTerrain();
        }
        else
        {
            //if (terrainStatus.activeSelf) terrainStatus.SetActive(false);
        }
    }


    void UpdateTerrain()
    {
        TerrainField.text = BattleManager.instance.currentTerrain;


        switch (BattleManager.instance.currentTerrain)
        {
            case "Plain":
                terrainPicture.GetComponent<Image>().sprite = Plain;
                break;
            case "Sand":
                terrainPicture.GetComponent<Image>().sprite = Sand;
                break;
            case "Tree":
                terrainPicture.GetComponent<Image>().sprite = Tree;
                break;
            case "Mountain":
                terrainPicture.GetComponent<Image>().sprite = Mountain;
                break;
            default:
                terrainPicture.GetComponent<Image>().sprite = null;
                break;
        }
    }
}
