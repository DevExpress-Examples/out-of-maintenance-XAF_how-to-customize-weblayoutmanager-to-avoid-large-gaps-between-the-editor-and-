using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLayoutSolution.Module.BusinessObjects {
    [NavigationItem]
    public class BusinessObject : BaseObject {
        public BusinessObject(Session s) : base(s) { }
        [ModelDefault("Caption", "Abcd")]
        public string FieldA { get; set; }
        [ModelDefault("Caption", "Efghijklmn")]
        public string FieldB { get; set; }
        public string FieldC { get; set; }
        public string FieldD { get; set; }
        public string FieldE { get; set; }
        [ModelDefault("Caption", "A long caption")]
        public string FieldF { get; set; }
        public string FieldG { get; set; }
        public string FieldH { get; set; }
        [ModelDefault("Caption", "Another long caption")]
        public string FieldI { get; set; }
        public string FieldJ { get; set; }
        public string FieldK { get; set; }
        public string FieldL { get; set; }
        public string FieldM { get; set; }
        [ModelDefault("Caption", "A very very very very very long caption")]
        public string FieldN { get; set; }
    }
}
