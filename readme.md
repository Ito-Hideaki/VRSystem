# VRSystem with Unity by 2J

Unityバージョン: 6000.2.0f1

メインシーンはAssets/Scenes/GameSceneにある。
初回起動時のシーンとは違うので気を付けて。

## Pushについて
ひとつのシーンは原則一人だけが編集してPushする。面倒なので。

## Large Filesについて
ファイルサイズの都合上Githubにアップロードできないファイル群はAssets/Large Filesに入れてある。
**当然Githubにはない。**

## Pullする上での注意
まず、**ProjectおよびProject.meta以外のAssets以下のフォルダとファイルはgit上にはない。**
それらをまとめたものをここでは**外部アセットファイル**と呼び、コミットと紐づけて2J Discordで配布することとする。
Pushするたびに最新の外部アセットファイルを公開すること。

外部アセットファイルは次の要素からなる。
- Large Files
- Text Mesh Proなどの機能
- Unity Asset Storeなどからインストールしたアセット
よって基本的に、追加されることがあっても変更されることはない。既存のものを削除しなければプロジェクトが壊れることはないはずである。
２つのブランチをマージする場合は、両方の要素を含むようにファイルを構成する。


導入するときは、リポジトリをクローンした後、Unityを開く**前**にAssetsフォルダをエクスプローラーから開き、その時点での全てのアセットとファイルその他を張り付けること。**必要なものが揃っていない状態でUnityを開くとプロジェクトが壊れる。**（直せはする）

触る前には以下の知識を確認すること。ただし細かいものまで全部やってるわけではない。

[UnityをGitHubで共同開発するための快適な開発環境＋Tips：Ayaka Kawabe](https://qiita.com/ayakaintheclouds/items/c7022b393485db573bda)

[Unity開発者が複数人で開発を進める上で覚えておくと幸せになる９つの事：tsubaki_t1](https://tsubakit1.hateblo.jp/entry/20140613/1402670011#文字コードに気をつける)

改行コードはUnity側をCRLF、CommitおよびGithub側をLFで統一している。windowsのgitであればデフォルト設定でこのようになっているはずである。詳しくは[こちら](https://qiita.com/jun1s/items/739a01f381085b68f170)を参照のこと。