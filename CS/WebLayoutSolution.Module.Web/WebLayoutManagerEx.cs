using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WebLayoutSolution.Module.Web {

    public enum TextAlignModeGroup {
        UseParentOptions = 0,
        AlignLocal = 1,
        AutoSize = 2,
        //CustomSize = 3,
        AlignWithChildren = 4
    }
    public enum TextAlignModeItem {
        UseParentOptions = 0,
        //AlignLocal = 1,
        AutoSize = 2,
        //CustomSize = 3,
        //AlignWithChildren = 4
    }
    public interface IModelWebLayoutGroup {
        [Category("Behavior"), DefaultValue(TextAlignModeGroup.UseParentOptions)]
        TextAlignModeGroup TextAlignMode { get; set; }
    }
    public interface IModelWebLayoutItem {
        [Category("Behavior"), DefaultValue(TextAlignModeItem.UseParentOptions)]
        TextAlignModeItem TextAlignMode { get; set; }
    }
    public class WebLayoutManagerEx : WebLayoutManager {
        public WebLayoutManagerEx(bool simple, bool delayedItemsInitialization) : base(simple, delayedItemsInitialization) { }
        protected override LayoutItemTemplateContainer LayoutItem(ViewItemsCollection detailViewItems, IModelLayoutViewItem layoutItemInfo) {
            string key = (layoutItemInfo.ViewItem != null) ? layoutItemInfo.ViewItem.Id : layoutItemInfo.Id;
            ViewItem viewItem = detailViewItems[key];
            LayoutItemTemplateContainer layoutItemTemplateContainer = new LayoutItemTemplateContainer(this, detailViewItems, layoutItemInfo);
            layoutItemTemplateContainer.ID = WebIdHelper.GetCorrectedLayoutItemId(layoutItemInfo);
            layoutItemTemplateContainer.ShowCaption = this.GetIsLayoutItemCaptionVisible(layoutItemInfo, viewItem);
            if (viewItem != null) {
                layoutItemTemplateContainer.Caption = this.EnsureCaptionColon(viewItem.Caption);
            }
            layoutItemTemplateContainer.CaptionWidth = CalculateCaptionWidth(viewItem, detailViewItems, layoutItemInfo);
            Locations captionLocation = layoutItemInfo.CaptionLocation;
            HorzAlignment captionHorizontalAlignment = layoutItemInfo.CaptionHorizontalAlignment;
            VertAlignment captionVerticalAlignment = layoutItemInfo.CaptionVerticalAlignment;
            layoutItemTemplateContainer.CaptionLocation = (Equals(captionLocation, Locations.Default) ? this.DefaultLayoutItemCaptionLocation : captionLocation);
            layoutItemTemplateContainer.CaptionHorizontalAlignment = (Equals(captionHorizontalAlignment, HorzAlignment.Default) ? this.DefaultLayoutItemCaptionHorizontalAlignment : captionHorizontalAlignment);
            layoutItemTemplateContainer.CaptionVerticalAlignment = (Equals(captionVerticalAlignment, VertAlignment.Default) ? this.DefaultLayoutItemCaptionVerticalAlignment : captionVerticalAlignment);
            this.LayoutItemTemplate.InstantiateIn(layoutItemTemplateContainer);
            ItemCreatedEventArgs args = new ItemCreatedEventArgs(layoutItemInfo, viewItem, layoutItemTemplateContainer);
            this.OnLayoutItemCreated(args);
            this.OnCustomizeAppearance(new CustomizeAppearanceEventArgs(layoutItemInfo.Id, new WebLayoutItemAppearanceAdapter(layoutItemTemplateContainer), null));
            return layoutItemTemplateContainer;
        }
        private System.Web.UI.WebControls.Unit CalculateCaptionWidth(ViewItem viewItem, ViewItemsCollection detailViewItems, IModelLayoutViewItem layoutItemInfo) {
            var item = layoutItemInfo as IModelWebLayoutItem;
            if (item != null) {
                if (item.TextAlignMode == TextAlignModeItem.AutoSize) {
                    return this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                } else {
                    IModelViewLayoutElement current = layoutItemInfo;
                    while (current != null) {
                        var group = current.Parent as IModelWebLayoutGroup;
                        if (group != null) {
                            if (group.TextAlignMode == TextAlignModeGroup.AutoSize) {
                                return this.GetMaxStringWidth(new string[] { this.EnsureCaptionColon(viewItem.Caption) });
                            }
                            if (group.TextAlignMode == TextAlignModeGroup.AlignLocal) {
                                return CalculateLayoutItemCaptionWidth((IModelLayoutGroup)group, detailViewItems, false);
                            }
                            if (group.TextAlignMode == TextAlignModeGroup.AlignWithChildren) {
                                return CalculateLayoutItemCaptionWidth((IModelLayoutGroup)group, detailViewItems, true);
                            }
                        }
                        current = current.Parent as IModelViewLayoutElement;
                    }
                }
            }
            return this.LayoutItemCaptionWidth;
        }
        private new System.Web.UI.WebControls.Unit CalculateLayoutItemCaptionWidth(IEnumerable<IModelViewLayoutElement> layoutInfo, ViewItemsCollection detailViewItems, bool recursively) {
            List<string> list = new List<string>();
            CollectLayoutItemVisibleCaptions<IModelViewLayoutElement>(list, layoutInfo, detailViewItems, recursively);
            return this.GetMaxStringWidth(list);
        }
        private new void CollectLayoutItemVisibleCaptions<T>(IList<string> targetList, IEnumerable<T> layoutInfo, ViewItemsCollection detailViewItems, bool recursively) {
            foreach (T current in layoutInfo) {
                if (current is IModelLayoutViewItem) {
                    IModelLayoutViewItem modelLayoutViewItem = (IModelLayoutViewItem)((object)current);
                    string key = (modelLayoutViewItem.ViewItem != null) ? modelLayoutViewItem.ViewItem.Id : modelLayoutViewItem.Id;
                    ViewItem viewItem = detailViewItems[key];
                    if (viewItem != null && this.GetIsLayoutItemCaptionVisible((IModelLayoutViewItem)((object)current), viewItem) && this.GetIsItemForCaptionCalculation((IModelLayoutViewItem)((object)current), viewItem)) {
                        targetList.Add(this.EnsureCaptionColon(viewItem.Caption));
                    }
                } else if (recursively) {
                    if (current is IEnumerable<IModelViewLayoutElement>) {
                        CollectLayoutItemVisibleCaptions<IModelViewLayoutElement>(targetList, (IEnumerable<IModelViewLayoutElement>)((object)current), detailViewItems, recursively);
                    }
                    if (current is IEnumerable<IModelLayoutGroup>) {
                        CollectLayoutItemVisibleCaptions<IModelLayoutGroup>(targetList, (IEnumerable<IModelLayoutGroup>)((object)current), detailViewItems, recursively);
                    }
                }
            }
        }

    }
}
