using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NexGenFlow.NexGen
{

    public partial class doc_rin_rsd
    {
        static doc_rin_rsd()
        {
            DevExpress.Xpo.Metadata.XPClassInfo xpClass = (new Session()).GetClassInfo(typeof(doc_rin_rsd));

            DevExpress.Xpo.Metadata.XPMemberInfo mi = xpClass.GetMember("id");
            mi.RemoveAttribute(typeof(KeyAttribute));
            mi.AddAttribute(new KeyAttribute(true));
        }
        public doc_rin_rsd(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
