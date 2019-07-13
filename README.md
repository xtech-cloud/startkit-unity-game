
# 快速使用

## WebGL

构建到unigamekit文件夹

## PicoVR

选择菜单栏中的MegeXR/Import/PicoVR，等待SDK导入完成。
选中Hierarchy中的XRProxy，将ModeVR切换为ON。
在XRProxy.cs中找到以下代码：

```csharp
xr = new DummyXR();
```

改为 

```csharp
xr = new PicoVR();
```

开始构建APK。

`不需要PicoVR时记得使用菜单栏中的MegeXR/Clean/PicoVR，清理导入的SDK。`

# 定制

## WebGL

将WebGLTemplates/UniGameKit/index.html中的unigamekit.json改为自己的项目名

```javascript
<script>
      gameInstance = UnityLoader.instantiate("gameContainer", "Build/unigamekit.json", {onProgress: UnityProgress});
</script>
```

将WebGLTemplates/UniGameKit/index.html中的XTC - UniGameKit改为自己的标题

```javascript
<title>XTC - UniGameKit</title>
```
