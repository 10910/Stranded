using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.Overlays;

[EditorToolbarElement("MyTools/Screenshot", typeof(SceneView))]
public class ScreenshotToolbarButton : EditorToolbarButton
{
    public const string id = "MyTools/Screenshot";

    public ScreenshotToolbarButton(){
        text = "S";
        tooltip = "截图当前 Game 画面";
        clicked += TakeScreenshot;
    }

    private void TakeScreenshot() {
        string path = EditorUtility.SaveFilePanel(
            "保存截图",
            "",
            "screenshot.png" ,
            "png"
        );

        if (!string.IsNullOrEmpty(path)) {
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log($"截图已保存到: {path}");
        }
    }
}

[Overlay(typeof(SceneView), "Screenshot Tools")]
public class ScreenshotToolbarOverlay : ToolbarOverlay
{
    public ScreenshotToolbarOverlay() : base(ScreenshotToolbarButton.id) { }
}