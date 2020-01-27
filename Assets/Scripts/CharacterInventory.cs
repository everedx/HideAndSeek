using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventory 
{
    private ArrayList doorkeys;
    private GameObject inventoryBar;

    public CharacterInventory(GameObject inv)
    {
        doorkeys = new ArrayList();
        inventoryBar = inv;
    }

    public void addKey(string doorName)
    {
        doorkeys.Add(doorName);
        addKeyInventory(doorName);
    }
    public void removeKey(string doorName)
    {

        doorkeys.Remove(doorName);
        removeKeyInventory(doorName);
    }

    public bool keyExistInInventory(string doorName)
    {
        return doorkeys.Contains(doorName);
    }

    public int getNumberOfKeys()
    {
        return doorkeys.Count;
    }

    private void addKeyInventory(string name)
    {
        int keys = getNumberOfKeys();
        GameObject img = (GameObject)Object.Instantiate(Resources.Load("CanvasKey"), inventoryBar.transform);
        img.name = name + "Inventory";
        RectTransform rt = (RectTransform)img.transform;
        img.transform.localPosition = new Vector2( 1100 - keys * rt.rect.width,0);
        
    }

    private void removeKeyInventory(string name)
    {
        int keys = getNumberOfKeys();
        GameObject.Destroy(GameObject.Find(name+ "Inventory"));
        RectTransform rt;
        int children = inventoryBar.transform.childCount;

        for (int i = 0; i < children; ++i)
        {
            Transform child = inventoryBar.transform.GetChild(i);
            rt = (RectTransform)child;
            child.localPosition = new Vector2(1100 - i * rt.rect.width, 0);
        }
           

    }

}
