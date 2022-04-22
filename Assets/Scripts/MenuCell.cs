using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCell : MonoBehaviour
{
    public Text label;
    public Text description;
    public Text price;
    public Text update;
    public Text statText;

    public int priceInt;
    public int scale;
    public int stat;

    private void Update()
    {
        price.text = "price: " + priceInt.ToString();
        statText.text = stat.ToString();
        update.text = "scale: " + scale.ToString();
    }
}
