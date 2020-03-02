using System.Collections;
using System.Collections.Generic;
using Qonversion.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class QonversionUsage : MonoBehaviour
{
    public QonversionPurchases QonversionPurchases;
    public Text Text;

    void Start()
    {
        QonversionPurchases.Initialize("test", data => { Text.text = data; });
    }
}
