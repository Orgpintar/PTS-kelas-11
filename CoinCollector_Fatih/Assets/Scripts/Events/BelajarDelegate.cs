using System;
using UnityEngine;

public class BelajarDelegate : MonoBehaviour
{
    delegate void AksiSederhana();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Langkah1_SimpanSatuMethod();
        Langkah2_BeberapaMethodSekaligus();
        // Langkah3_ActionSiapPakai();
    }

    void Langkah1_SimpanSatuMethod()
    {
        AksiSederhana kotak = TulisHalo;
        kotak();
    }

    void Langkah2_BeberapaMethodSekaligus()
    {
        AksiSederhana kotak = TulisHalo;
        kotak += TulisDunia;
        kotak();
    }

    void Langkah3_ActionSiapPakai()
    {
        // Action sudah disediakan C#. Sama seperti delegate void ...() di atas.
        Action kotak = TulisHalo;
        kotak += TulisDunia;
        kotak();
    }
    void ContohAksi1()
    {
        AksiSederhana aksi = TulisHalo;
        aksi();
    }
    void TulisHalo()
    {
        Debug.Log("Halo");
    }
    void TulisDunia()
    {
        Debug.Log("Dunia");
    }
}
