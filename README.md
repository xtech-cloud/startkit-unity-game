
# 快速使用

## WebGL

构建到unigamekit文件夹

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
