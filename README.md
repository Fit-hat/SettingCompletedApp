# PC Setup Audit Tool

Windows端末のセットアップ完了通知および証跡取得を目的とした Windows Forms アプリケーションです。

実行時に端末情報を収集し、ログファイルとして保存します。

---

# 機能

## 画面表示

- 設定完了メッセージ表示
- 最前面表示
- 実行日時表示
- HOST名表示
- IPアドレス表示
- OS情報表示
- ユーザー名表示
- ドメイン名表示

---

## ログ出力

実行ファイルと同じフォルダにログファイルを作成します。

例

```text
SettingCompletedApp.exe
SettingCompletedApp.log
```

ログは追記形式で保存され、過去の情報は削除されません。

---

## ログ出力内容

### 基本情報

- HOST名
- OSバージョン
- 製造元
- モデル名
- ユーザー名
- ドメイン名
- ログオンユーザー
- CPU名
- メモリ容量

---

### ネットワーク情報

ネットワークインターフェースごとに出力

- インターフェース名
- 説明
- MACアドレス
- IPv4 / IPv6アドレス

---

### BitLocker情報

ドライブごとに出力

例

```text
【BitLocker】

ドライブ : C:
状態 : 暗号化済み
保護状態 : 有効

ドライブ : D:
状態 : 未暗号化
保護状態 : 無効
```

---

### ブラウザ拡張機能

#### Microsoft Edge

- 拡張機能ID
- 名称
- バージョン

#### Google Chrome

- 拡張機能ID
- 名称
- バージョン

---

### Windows Update

インストール済み更新プログラム一覧

- KB番号
- インストール日

---

### プリンター

インストール済みプリンター一覧

---

### Win32アプリ

インストール済みアプリケーション一覧

---

# 実行方法

## 通常起動

```cmd
SettingCompletedApp.exe
```

画面表示およびログ出力を行います。

---

## ログ出力のみ

```cmd
SettingCompletedApp.exe /logonly
```

画面を表示せずログ出力のみ実行します。

---

# 開発環境

- Visual Studio 2022
- .NET 10
- Windows Forms

---

# 発行設定

- Release
- net10.0-windows
- Self Contained
- Single File
- win-x64

---

# プロジェクト構成

```text
SettingCompletedApp

├─ Assets
│  └─ app.ico
│
├─ Models
│  ├─ BrowserExtensionInfo.cs
│  ├─ NetworkAdapterInfo.cs
│  └─ SystemInfo.cs
│
├─ Services
│  ├─ ApplicationInventoryService.cs
│  ├─ BrowserExtensionService.cs
│  ├─ LogService.cs
│  ├─ PrinterService.cs
│  ├─ SystemInfoService.cs
│  └─ WindowsUpdateService.cs
│
├─ Form1.cs
├─ Program.cs
└─ SettingCompletedApp.csproj
```

---

# 今後の実装候補

## セキュリティ

- TPM状態
- Secure Boot状態
- Defender状態
- Windowsライセンス認証状態
- ローカル管理者一覧

## ハードウェア

- BIOSシリアル番号
- BIOSバージョン
- ディスク容量
- バッテリー情報

## ネットワーク

- DNSサーバー
- デフォルトゲートウェイ
- VPN情報

## Microsoft 365

- OneDrive状態
- Teamsインストール状況
- Officeバージョン

---

# ライセンス

Copyright © Fit-Hat Solutions
