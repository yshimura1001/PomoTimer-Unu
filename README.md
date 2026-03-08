# PomoTimer Unu

## 概要

休憩時間に通知画面が全画面で開き、強制的に作業を中断させることを目的としたWindows デスクトップ向けのポモドーロタイマーアプリです。

**CSVで管理する時刻表に従って自動進行する時刻表モード**や**固定タイマー**、**変動タイマー**モードを実装しています。

### 余談
- 「Unu」は、エスペラント語で「1」。旧版(Windowsフォーム)を「Nulo：0」としています。

## 開発背景

長時間作業を続けてしまう特性があり、「それなら強制的に休憩するよう強制するタイマーアプリを開発・利用しよう」という考えで開発しました。

自分自身が毎日使うことを前提とした機能を中心に実装しています。(休憩3分前通知など。)

## 主な機能

### 3つの動作モード

| モード | 概要 |
|---|---|
| **時刻表モード** | CSVファイルに定義した時刻表に従い、時間枠が自動進行します。各枠の休憩通知ON/OFFを個別に設定可能。 |
| **固定タイマーモード** | 毎時00～50分は作業、50～60分の10分間は休憩する固定タイマー。 |
| **変動タイマーモード** | 手動でポモドーロ（作業50分 + 休憩10分）を開始するタイマー。 |

### 通知

- **休憩タイマーウィンドウ**: 作業時間終了時に専用ウィンドウを全画面にポップアップ表示し、残り休憩時間をカウントダウン。
- **休憩前通知**: 休憩開始3分前にシステム通知(トースト)を表示。

### 自動処理

- **自動終了**: 17時にアプリを自動終了。
- **自動シャットダウン**: 21時にPCを強制的にシャットダウン。（21時以降の作業の強制停止。）

### ログ

- **Excelログ**: 休憩フラグをExcelファイル（`data.xlsx`）に自動記録。
- **テキストログ**: アプリのイベントをファイルに記録し、直近7日分より古いログを自動削除。

### 設定

- **設定画面(時刻表・固定タイマーモードのみ)**: 時刻表をGUI上で編集可能。(一時的。最初から変えたい場合は、CSVファイルを編集。)
- **ダークモード切替**: メインウィンドウのダークモードを切替可能。
- **最前面表示切替**: ウィンドウを常に最前面に表示するか切替可能。

## 技術スタック

| カテゴリ | 技術 |
|---|---|
| 言語 | C# |
| フレームワーク | .NET 10 / WPF |
| アーキテクチャ | MVVM |
| MVVMライブラリ | CommunityToolkit.Mvvm |
| DIコンテナ | Microsoft.Extensions.DependencyInjection |
| 通知 | Microsoft.Toolkit.Uwp.Notifications |
| テスト用時刻制御 | Microsoft.Extensions.TimeProvider.Testing |
| Excel操作 | ClosedXML |
| ターゲットOS | Windows 10 (10.0.17763) 以降 |

## アーキテクチャ

MVVMパターンを採用し、UI・ロジック・データアクセスを明確に分離しています。

```
View  (XAML / Window)
  │
ViewModel  (ObservableObject / RelayCommand)
  │
Model  (TimerModelBase / TimeTableTimerModel / VariableTimerModel)
  │
Domain/Services  (TimeTableServiceBase - 時刻表管理ロジック)
  │
Infrastructure  (ITimeTableRepository - CSV / InMemory)
  │
AppServices  (TimerService / LoggerService / ExcelService / ToastService 等)
```

### 設計上の工夫

- **`TimerServiceBase` の抽象化**: 本番用 `TimerService` とテスト用 `FakeTimerService` を差し替え可能にし、時刻をモックして単体テストを実施できる構造にしています。
- **リポジトリパターン**: 時刻表の取得元（CSV / インメモリ）を `ITimeTableRepository` インターフェースで抽象化し、切り替えが容易な構造にしています。
- **`PeriodicTimer` + `Dispatcher.InvokeAsync`**: バックグラウンドで動作する `PeriodicTimer` からUIスレッドへ安全にTickを届け、デッドロックを防いでいます。

## ディレクトリ構成

```
PomoTimer/
├── AppServices/          # アプリ横断的なサービス（タイマー、ログ、Excel 等）
├── Domain/
│   ├── Services/         # 時刻表管理ビジネスロジック
│   └── TimeTableRow.cs   # 時刻表1行を表すドメインオブジェクト
├── Infrastructure/       # データアクセス（CSV読み込み等）
├── Models/               # タイマーモデル（状態・判定ロジック）
├── ViewModels/           # 各ウィンドウのViewModel
├── *.xaml / *.xaml.cs    # Viewファイル
├── time_table.csv        # 時刻表データ
├── data.xlsx             # 作業ログ記録先
└── appsettings.json      # アプリ設定
```

## 動作環境

- Windows 10 (バージョン 1809 / ビルド 17763) 以降
- .NET 10 ランタイム

## ダウンロード
「Releases」からダウンロードしてください。 