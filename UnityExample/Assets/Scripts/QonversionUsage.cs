using System.Collections;
using System.Collections.Generic;
using Qonversion.Scripts;
using TMPro;
using UnityEngine;

public class QonversionUsage : MonoBehaviour
{
    public QonversionPurchases QonversionPurchases;
    public TextMeshPro Text;

    void Start()
    {
        QonversionPurchases.Initialize("test", data => { Text.text = data; });
    }
}
