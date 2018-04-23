using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using WebLayoutSolution.Module.BusinessObjects;

namespace WebLayoutSolution.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (ObjectSpace.FindObject<BusinessObject>(null) == null) {
                var obj = ObjectSpace.CreateObject<BusinessObject>();
                obj.FieldA = "aaa";
                obj.FieldB = "bbb";
                obj.FieldC = "ccc";
                obj.FieldD = "ddd";
                obj.FieldE = "eee";
                obj.FieldF = "fff";
                obj.FieldG = "ggg";
                obj.FieldH = "hhh";
                obj.FieldI = "iii";
                obj.FieldJ = "jjj";
                obj.FieldK = "kkk";
                obj.FieldL = "lll";
                obj.FieldM = "mmm";
                obj.FieldN = "nnn";
            }
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
