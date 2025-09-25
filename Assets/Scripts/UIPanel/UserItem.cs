
using UnityEngine;
using UnityEngine.UI;

public class UserItem:MonoBehaviour
{
    public Text playerName;
    public void SetData(string name)
    {
        playerName.text = name;
    }
}
