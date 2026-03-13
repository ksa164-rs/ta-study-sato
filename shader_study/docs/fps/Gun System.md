# Gun System (FPS)

## 概要

このスクリプトはFPSの銃システムを管理する。

主な機能

- 射撃入力
- 弾管理
- リロード
- リコイル
- Bloom
- Raycast命中判定
- エフェクト生成

---

# 処理フロー

クリック
↓
HandleShootInput()
↓
TryShoot()
↓
Shoot()

Shoot()

- ConsumeAmmo()
- ApplyRecoil()
- ApplyBloom()
- FireRay()
- PlayShootEffects()

---

# 各関数の役割

## HandleShootInput()

入力を処理する。

Semi
クリックした瞬間だけ発射

Auto
クリック押しっぱなしで発射

---

## TryShoot()

実際に撃てるか判定する。

チェック内容

- リロード中か
- 弾があるか
- 発射間隔

---

## Shoot()

射撃処理の中心。

処理順

1. 弾消費
2. リコイル
3. Bloom増加
4. Raycast
5. エフェクト

---

## FireRay()

弾の命中判定。

処理

1 カメラからRayを飛ばす  
2 狙い地点を決定  
3 銃口からRayを飛ばす  
4 命中判定  
5 ダメージ or 弾痕

---

# 技術ポイント

## bloom

弾の拡散を管理する。

連射すると値が増え、
撃たないと回復する。

---

## recoilPattern

リコイルパターンを配列で管理。

Vector2

x = 横反動  
y = 縦反動

---

## Queue

弾痕管理に使用。

最大数を超えたら
古い弾痕から削除する。

---

# 今後の改善案

・HitMarker  
・リロードアニメーション  
・弾UI  
・ADSズーム  