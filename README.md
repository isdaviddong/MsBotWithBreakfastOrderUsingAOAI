# Bot 早餐點餐系統

這是一個使用 Microsoft Bot Framework 和 Azure OpenAI 服務建立的智能點餐機器人系統。該系統能夠協助顧客點餐、回答問題，並提供餐點建議。

## 專案架構

專案使用 Bot Framework SDK v4 開發，主要包含以下組件：

- **BotController**: API 端點控制器
- **EchoBot**: 機器人主要邏輯處理
- **ChatGPT**: Azure OpenAI 服務整合

## 核心檔案說明

### BotController.cs

控制器負責處理 HTTP 請求並轉發給機器人進行處理：

- 提供 `/api/messages` 端點
- 支援 POST 和 GET 請求
- 使用依賴注入處理 Bot Framework Adapter 和 Bot 實例

### EchoBot.cs

機器人的核心邏輯實現：

- 繼承自 `ActivityHandler`
- 處理用戶訊息（OnMessageActivityAsync）
- 處理新成員加入（OnMembersAddedAsync）
- 整合 ChatGPT 服務回應用戶查詢

### ChatGPT.cs

Azure OpenAI 服務整合：

- 設定 Azure OpenAI 服務連線
- 實現與 GPT 模型的通訊
- 處理餐廳菜單系統提示（System Prompt）
- 維護對話歷史

## 菜單資訊

### 主餐
- 大亨堡（45元）- 豬肉
- 麥香雞（36元）- 雞肉
- 蛋餅（27元）- 蛋奶素
- 可麗餅（50元）- 蛋奶素
- 飯糰（45元）- 含肉鬆

### 飲料
- 可樂（55元）
- 紅茶（35元）
- 奶茶（45元）

### 特別優惠
- 周日早上大亨堡買一送一

## 系統設定

使用前需要在 ChatGPT.cs 中配置以下 Azure OpenAI 參數：

```csharp
const string AzureOpenAIEndpoint = "https://________.openai.azure.com";
const string AzureOpenAIModelName = "__________";
const string AzureOpenAIToken = "______________________";
const string AzureOpenAIVersion = "2024-02-15-preview";
```

## 開發環境需求

- .NET Core 3.1 或更高版本
- Bot Framework SDK v4.22.0
- Azure 訂閱（用於 OpenAI 服務）