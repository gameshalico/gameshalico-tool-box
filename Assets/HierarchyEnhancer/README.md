# Hierarchy Enhancer

## 概要
ヒエラルキーを見やすくし、いくつかの便利機能を追加します。

## 導入方法
- PackageManagerの"Add package from git URL..."から以下のURLを指定してインストールします。
```
https://github.com/gameshalico/gameshalico-tool-box.git?path=Assets/HierarchyEnhancer
```

## 機能

### Colorful Indent Guide
ヒエラルキーウィンドウの親子関係による階層構造を視覚的に分かりやすくします。
また、行をストライプで色付けし見やすくします。

### Component Icon
ゲームオブジェクトの左にコンポーネントのアイコンを表示します。
ゲームオブジェクトにアタッチされている最初のコンポーネントのアイコンが表示されます。

### Side Utility View
ゲームオブジェクトの右側に切り替え可能なビューを表示します。

この表示は`Ctrl+T`で以下に切り替えることができます。
- コンポーネントの一覧表示
- レイヤー名表示
- タグ名表示
- ゲームオブジェクトのアクティブを切り替えられるチェックボックス表示

### Highlight and Separator
ゲームオブジェクトの名前が
- `+++`から始まる場合、そのゲームオブジェクトをハイライト表示します
- `---`から始まる場合、そのゲームオブジェクトを区切り線として表示します。

ハイライトは`Ctrl+H`、区切り線は`Ctrl+J`で切り替え可能です。

`Ctrl+Shift+H`でハイライトされたゲームオブジェクトを全て選択します。
