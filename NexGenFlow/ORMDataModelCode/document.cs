using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NexGenFlow.NexGen
{

    public partial class document
    {
        static document()
        {
            DevExpress.Xpo.Metadata.XPClassInfo xpClass = (new Session()).GetClassInfo(typeof(document));

            DevExpress.Xpo.Metadata.XPMemberInfo mi = xpClass.GetMember("id");
            mi.RemoveAttribute(typeof(KeyAttribute));
            mi.AddAttribute(new KeyAttribute(true));
        }
        public document(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
