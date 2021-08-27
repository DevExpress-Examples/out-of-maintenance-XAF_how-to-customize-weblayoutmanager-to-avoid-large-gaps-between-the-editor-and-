<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128589217/20.1.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T228434)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Model.DesignedDiffs.xafml](./CS/WebLayoutSolution.Module.Web/Model.DesignedDiffs.xafml) (VB: [Model.DesignedDiffs.xafml](./VB/WebLayoutSolution.Module.Web/Model.DesignedDiffs.xafml))
* **[WebLayoutManagerEx.cs](./CS/WebLayoutSolution.Module.Web/WebLayoutManagerEx.cs) (VB: [WebLayoutManagerEx.vb](./VB/WebLayoutSolution.Module.Web/WebLayoutManagerEx.vb))**
* [WebModule.cs](./CS/WebLayoutSolution.Module.Web/WebModule.cs) (VB: [WebModule.vb](./VB/WebLayoutSolution.Module.Web/WebModule.vb))
* [WebApplication.cs](./CS/WebLayoutSolution.Web/WebApplication.cs) (VB: [WebApplication.vb](./VB/WebLayoutSolution.Web/WebApplication.vb))
<!-- default file list end -->
# How to customize WebLayoutManager to avoid large gaps between the editor and its caption


<strong>Scenario:</strong><br />
<p>When you have properties in your business class with captions of significantly different lengths, the detail view often looks awkward on the web. This happens because the built-in WebLayoutManager is relatively simple and doesn't support many layout properties that the WinLayoutManager supports (because of the underlying powerful XtraLayout control). Specifically, the item caption length of layout items on the web is calculated as a maximum of <strong>all visible items</strong> on the detail view. As a result, we have too large a gap between a short caption and the editor when there are long captions.<br /><br /><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-customize-weblayoutmanager-to-avoid-large-gaps-between-the-editor-and-its-caption-t228434/17.1.3+/media/6501880a-f405-11e4-80bf-00155d62480c.png"><br /><strong><br />Solution:</strong></p>
<p>This example demonstrates a possible solution to this common issue by introducing additional properties in the layout group and layout item model nodes.</p>
<p>We create a new model interface that extends the IModelLayoutGroup interface (similar to <a href="https://documentation.devexpress.com/#eXpressAppFramework/clsDevExpressExpressAppWinSystemModuleIModelWinLayoutGrouptopic">IModelWinLayoutGroup</a>) with the additional TextAlignMode property. Another model interface extends the IModelLayoutViewItem interface (similar to <a href="https://documentation.devexpress.com/#eXpressAppFramework/clsDevExpressExpressAppWinSystemModuleIModelWinLayoutItemtopic">IModelWinLayoutItem</a>) in the same manner. These properties allow you specify different modes of the caption width calculation for individual items and items in each group separately. Refer to the <a href="https://documentation.devexpress.com/#WindowsForms/DevExpressXtraLayoutTextAlignModeGroupEnumtopic">TextAlignModeGroup</a> and <a href="https://documentation.devexpress.com/#WindowsForms/DevExpressXtraLayoutTextAlignModeItemEnumtopic">TextAlignModeItem</a> enumerations documentation for options description. These new model interfaces are registered in the web module.</p>
<p>We create a custom layout manager derived from the WebLayoutManager class with the LayoutItem method overridden. In this method, the CaptionWidth is calculated according to new options specified in the item and parent groups it belongs to. The custom layout manager is instantiated in the overridden CreateLayoutManagerCore method of the WebApplication.</p>
<p>Now, the detail view layout can be fine tuned via the Model Editor in the web module in a similar manner as you would in the WinForms application model.<br /><br /><img src="https://raw.githubusercontent.com/DevExpress-Examples/how-to-customize-weblayoutmanager-to-avoid-large-gaps-between-the-editor-and-its-caption-t228434/17.1.3+/media/73c33883-f405-11e4-80bf-00155d62480c.png"><br />Note how some caption widths are set to local minimums, some set to a a common minimum calculated in the owner or outer group and some widths are calculated globally.<br /><br />See also:<br /><a href="https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113169.aspx">Extend and Customize the Application Model in Code</a></p>
<p> </p>

<br/>


