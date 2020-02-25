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

    public void addKey(string doorName, Sprite sprite)
    {
        doorkeys.Add(doorName);
        addKeyInventory(doorName,sprite);
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

    private void addKeyInventory(string name,Sprite sprite)
    {
        int keys = getNumberOfKeys();
        GameObject img = null; 

        switch(sprite.name)
        {
            case "Tarjeta":
                img = (GameObject)Object.Instantiate(Resources.Load("CanvasKeyRed"), inventoryBar.transform);
                break;
            case "Tarjeta_azul":
                img = (GameObject)Object.Instantiate(Resources.Load("CanvasKeyBlue"), inventoryBar.transform);
                break;
            case "Tarjeta_verde":
                img = (GameObject)Object.Instantiate(Resources.Load("CanvasKeyGreen"), inventoryBar.transform);
                break;
            case "Tarjeta_amarilla":
                img = (GameObject)Object.Instantiate(Resources.Load("CanvasKeyYellow"), inventoryBar.transform);
                break;
        }

        if (img != null)
        {
            img.name = name + "Inventory";
            RectTransform rt = (RectTransform)img.transform;
            img.transform.localPosition = new Vector2(1100 - keys * rt.rect.width, 0);
        }
    }

    private void removeKeyInventory(string name)
    {
        int keys = getNumberOfKeys();
        
        GameObject.Destroy(GameObject.Find(name+ "Inventory"));
        RectTransform rt;
        int children = inventoryBar.transform.childCount;
        int index = 0;
        for (int i = 0; i < keys; ++i)
        {
            
            if (inventoryBar.transform.GetChild(i).name.Equals(name + "Inventory"))
            {
                index = i + 1;
            }
       
            Transform child = inventoryBar.transform.GetChild(index);
            rt = (RectTransform)child;
            child.localPosition = new Vector2(1100 - (i+1) * rt.rect.width, 0);
            index++;
        }
           

    }

}
