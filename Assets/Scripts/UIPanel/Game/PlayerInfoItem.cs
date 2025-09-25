
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItem:MonoBehaviour
{
    [SerializeField]
    private Text txtName;
    [SerializeField]
    private Text txtNowHp;
    [SerializeField]
    private Slider sliderHp;

    public void Set(string name, int hp)
    {
        txtName.text = name;
        UpdateHp(hp);
    }

    public void UpdateHp(int value)
    {
        sliderHp.value = value;
        txtNowHp.text = "Hp:"+value.ToString();
    }
}
