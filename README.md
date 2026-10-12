# 花札ポーカー

## 概要
オンライン対戦対応の3D花札ポーカーゲームです。

## ゲーム画面
1. タイトル画面
![HanafudaPoker](img/HP_A_A.png)</br>

2. タイトル画面/接続中
![HanafudaPoker](img/HP_A_B.png)</br>

3. タイトル画面/部屋設定
![HanafudaPoker](img/HP_A_C.png)</br>

4. 待機画面/待機室
![HanafudaPoker](img/HP_B_A.png)</br>

5. ゲーム画面/最初のターン(Betも可能)
![HanafudaPoker](img/HP_C_A.png)</br>

6. ゲーム画面/札交換ターン
![HanafudaPoker](img/HP_C_B.png)</br>

7. ゲーム画面/こいこい無しで役を判定
![HanafudaPoker](img/HP_C_C.png)</br>

8. ゲーム画面/(7で勝った人)こいこいするか否か(しなかった場合、次のラウンドへ)
![HanafudaPoker](img/HP_C_D.png)</br>

9. ゲーム画面/こいこいした人は場に出す手札を選ぶ
![HanafudaPoker](img/HP_C_E.png)</br>

10. ゲーム画面/こいこいも含めて役判定。勝者決定後、次のラウンドへ
![HanafudaPoker](img/HP_C_F.png)</br>

## 開発環境
- Unity Universal 6000.0.44f1
- Visual Studio 2022
- Git / GitHub

## 使用技術
- C#
- Unity
- TextMesh Pro
- Photon Fusion
- Blender
- Krita
- MuseScore4

## セットアップ

1. このリポジトリをClone
2. Unity Hubからプロジェクトを開く
3. Package Managerから必要なPackageを導入
4. Photon Fusionの設定を行う（必要に応じて）
5. TitleSceneから起動

## ブランチ運用

master
- 安定版

develop
- Ryoncy用の開発ブランチ
  - 主にゲームシステム・ネットワークを開発

ho6ho6
- ho6ho6用の開発ブランチ
  - 主にUI・アニメーションを開発

## コーディングルール
- namespaceは `HanafudaPoker.xxx`
- クラス名・メソッド名は PascalCase
- privateメンバは SerializeField を優先する

```text
using System.Collections;

using namespace;

namespace HanafudaPoker.xxx
{
    class mono : Monobehaviour
    {
        public objectName;

        void FunctionName()
        {

        }
    }
}

```

## 現在の実装状況
- ✅ タイトル画面
- ✅ ロビーUI
- ✅ カード管理
- ✅ 役判定
- 🚧 アニメーション
- 🚧 ネットワーク

## 今後の予定
- エフェクト演出
- AI対戦 (実装未定)
