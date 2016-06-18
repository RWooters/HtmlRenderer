﻿// 2015,2014 ,BSD, WinterDev   

namespace LayoutFarm.Composers
{
    class HtmlSharedDocument : HtmlDocument
    {
        //this is not document fragment ***
        HtmlDocument primaryHtmlDoc;
        internal HtmlSharedDocument(HtmlDocument primaryHtmlDoc)
            : base(primaryHtmlDoc.UniqueStringTable)
        {
            //share string table with primary html doc
            this.primaryHtmlDoc = primaryHtmlDoc;
        }
    }
}