using OVR.OpenVR;
using Paroxe.PdfRenderer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRInput;

public class BookBehavior : MonoBehaviour
{
    public Renderer cover, backcover, spine, leftPage, rightPage;

#if UNITY_ANDROID
    private string m_FileName = "sample.pdf";
    private string m_FilePath = "Android/Data/LibraryBooks";
#endif
#if UNITY_STANDALONE
    private string m_Path = @"N:\Not Illegal Books\Books\!Textbooks\Programming in Lua 3rd Edition.pdf";
#endif
    private Animator m_Animatior;
    private bool open = false;

    private void Start()
    {
        m_Animatior = GetComponent<Animator>();

        PDFDocument pdfDocument = new PDFDocument(m_Path);

        if (pdfDocument.IsValid)
        {
            int pageCount = pdfDocument.GetPageCount();
            PDFRenderer renderer = new PDFRenderer();

            // Get the cover
            int page = 0;
            Texture2D tex = renderer.RenderPageToTexture(pdfDocument.GetPage(page % pageCount), 1024, 1024);
            tex.filterMode = FilterMode.Bilinear;
            tex.anisoLevel = 8;
            cover.material.mainTexture = tex;

            // Get the left page
            page = 30;
            tex = renderer.RenderPageToTexture(pdfDocument.GetPage(page % pageCount), 1024, 1024);
            leftPage.material.mainTexture = tex;

            // Get the rightpage
            page++;
            tex = renderer.RenderPageToTexture(pdfDocument.GetPage(page % pageCount), 1024, 1024);
            rightPage.material.mainTexture = tex;

            // Get the backcover
            page = pageCount - 1;
            tex = renderer.RenderPageToTexture(pdfDocument.GetPage(page % pageCount), 1024, 1024);
            backcover.material.mainTexture = tex;
        }
    }

    private void Update()
    {
        // If the trigger is pressed and the player is holding the book, open/close the book
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            if (!GetComponent<OVRGrabbable>().grabbed)
                return;

            if (open)
                m_Animatior.SetTrigger("Close");
            else
                m_Animatior.SetTrigger("Open");
            open = !open;
        }
    }

}
