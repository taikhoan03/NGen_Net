using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
namespace NexGenFlow.NexGen
{

    public partial class package_assignment
    {
        static package_assignment()
        {
            DevExpress.Xpo.Metadata.XPClassInfo xpClass = (new Session()).GetClassInfo(typeof(package_assignment));

            DevExpress.Xpo.Metadata.XPMemberInfo mi = xpClass.GetMember("id");
            mi.RemoveAttribute(typeof(KeyAttribute));
            mi.AddAttribute(new KeyAttribute(true));
        }
        public package_assignment(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
