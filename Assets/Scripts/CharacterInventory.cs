using System.Collections;
using System.Collections.Generic;


public class CharacterInventory 
{
    private ArrayList doorkeys;
    

    public CharacterInventory()
    {
        doorkeys = new ArrayList();
    }

    public void addKey(string doorName)
    {
        doorkeys.Add(doorName);
    }
    public void removeKey(string doorName)
    {
        doorkeys.Remove(doorName);
    }

    public bool keyExistInInventory(string doorName)
    {
        return doorkeys.Contains(doorName);
    }

}
