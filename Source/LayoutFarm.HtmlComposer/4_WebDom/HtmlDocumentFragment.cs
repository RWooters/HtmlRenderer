﻿//BSD, 2014-present, WinterDev 

namespace LayoutFarm.Composers
{
    class HtmlDocumentFragment : HtmlDocument
    {
        HtmlDocument _primaryHtmlDoc;
        internal HtmlDocumentFragment(HtmlDocument primaryHtmlDoc)
            : base(primaryHtmlDoc.Host, primaryHtmlDoc.UniqueStringTable)
        {
            //share string table with primary html doc
            _primaryHtmlDoc = primaryHtmlDoc;
        }
        public override bool IsDocFragment => true;
    }
}