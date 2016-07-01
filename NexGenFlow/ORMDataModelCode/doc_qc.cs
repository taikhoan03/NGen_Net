using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NexGenFlow.NexGen
{

    public partial class doc_qc
    {
        static doc_qc()
        {
            DevExpress.Xpo.Metadata.XPClassInfo xpClass = (new Session()).GetClassInfo(typeof(doc_qc));

            DevExpress.Xpo.Metadata.XPMemberInfo mi = xpClass.GetMember("id");
            mi.RemoveAttribute(typeof(KeyAttribute));
            mi.AddAttribute(new KeyAttribute(true));
        }
        public doc_qc(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
