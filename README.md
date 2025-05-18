# ASP.NET Core 教師/學生身份驗證與簡易成績系統範例

這是一個基於 ASP.NET Core MVC (.NET 8.0) 開發的範例應用程式，旨在展示如何實現不同角色（教師和學生）的登入、登出功能，並基於角色進行頁面導向和權限控制。專案中也展示了使用不同的資料庫存取技術：登入相關功能使用 ADO.NET (`Microsoft.Data.Sqlite`)，而教師成績管理功能則利用輕量級 ORM **Dapper**。密碼部分採用 **BCrypt.Net-Next** 函式庫進行雜湊和驗證。系統使用 SQLite 資料庫存儲所有相關數據。

## 專案簡介

本專案模擬了一個簡單的教學管理系統部分功能。主要特色包括：

- **多角色身份驗證：** 區分教師和學生兩種用戶角色進行登入。
- **用戶註冊：** 提供用戶註冊功能 (目前註冊後預設為學生角色)。
- **用戶登出：** 提供安全登出功能。
- **基於角色的授權：** 成功登入後，根據用戶角色導向到對應的儀表板頁面，並控制特定功能的存取 (如教師成績輸入頁面)。
- **教師成績管理：** 教師可以查看自己教授的課程，並輸入/修改該課程下學生的成績。
- **資料庫操作技術比較：**
  - 登入相關資料存取 (`AccountRepos`) 直接使用 `Microsoft.Data.Sqlite` 提供的 ADO.NET API 進行原生的 SQL 查詢。
  - 教師成績管理資料存取 (`ScoreController`) 利用 **Dapper** 擴充方法來執行 SQL 語句和映射結果到 C# 物件，展示輕量級 ORM 的便捷性。
- **密碼安全：** 使用 **BCrypt.Net-Next** 函式庫對用戶密碼進行雜湊儲存和安全驗證。
- **資料庫：** 使用 **SQLite** 作為輕量級資料庫。
- **技術實踐：** 應用了 Service 層封裝業務邏輯、Repository 模式、內建依賴注入 (DI) 以及 Cookie 身份驗證等 ASP.NET Core 特性。
- **資料庫結構：** 包含完整的 SQLite 資料庫 Schema 定義和範例數據腳本。

## 已實現功能

- ✅ 教師登入功能
- ✅ 學生登入功能
- ✅ 用戶登出功能
- ✅ 基於 Cookie 的身份驗證
- ✅ 基於角色的基礎授權 (透過 ClaimTypes.Role)
- ✅ 教師成績輸入與修改功能 (使用 Dapper 進行資料庫操作)
- ✅ 教師可以查看自己教授課程的學生及成績
- ✅ 使用 SQLite 的 `ON CONFLICT DO UPDATE` 語法高效更新成績
- ✅ 包含完整的 SQLite 資料庫 Schema 和範例數據腳本 (`schema.sql`)
- ✅ 密碼使用 **BCrypt.Net-Next** 進行雜湊儲存與驗證
- ✅ 登入功能使用 ADO.NET (`Microsoft.Data.Sqlite`)，成績管理使用 Dapper
- ✅ 使用 Service 和 Repository 模式分離部分業務邏輯和資料存取

## 待辦/未來功能

- ❌ 用戶註冊功能
- ❌ 學生查詢自己成績的功能
- 強化密碼管理 (如：增加密碼複雜度要求)
- 更完善的錯誤處理和輸入驗證
- 前端介面 (View) 的優化和互動性加強
- 增加更多基於角色的授權規則 (如：使用 Policy-based Authorization)
- 教師管理課程功能
- 學生管理個人資料功能

## 使用技術

- ASP.NET Core MVC **(.NET 8.0)**
- C#
- Microsoft.AspNetCore.Authentication.Cookies
- Dapper (**Version 2.1.66**)
- Microsoft.Data.Sqlite (**Version 9.0.5**) - ADO.NET 實作
- **BCrypt.Net-Next** (**Version 4.0.3**)
- Dependency Injection
- SQLite 資料庫

## 如何設定與執行

1.  **先決條件：**

    - 安裝 **.NET 8.0 SDK**。
    - Git 客戶端。
    - 一個 SQLite 客戶端 (如 `sqlite3` 命令列工具、DB Browser for SQLite 等 GUI 工具) 用於創建資料庫和執行腳本。

2.  **複製專案：**

    ```bash
    git clone <您的專案 Git 位址>
    cd <您的專案目錄名稱>
    ```

3.  **還原相依套件：**
    專案的 `.csproj` 檔案中已經定義了所需的 NuGet 套件 (Dapper, Microsoft.Data.Sqlite, BCrypt.Net-Next)。執行以下命令即可還原所有套件：

    ```bash
    dotnet restore
    ```

4.  **資料庫設定 (SQLite)：**

    - 本專案使用 SQLite 資料庫，其 Schema 定義和初始數據位於專案根目錄下的 `schema.sql` 檔案中。
    - 您需要先創建一個空的 SQLite 資料庫檔案。建議在專案根目錄下創建一個資料夾 (例如 `data`)，並在其中創建一個 `.db` 檔案 (例如 `app.db`)。
      ```bash
      mkdir data
      touch data/app.db # 在 Linux/macOS 或 Git Bash 中
      # 或在 Windows 命令提示字元中：
      # mkdir data
      # type nul > data\app.db
      ```
    - 使用您的 SQLite 客戶端工具打開剛創建的 `.db` 檔案，然後執行 `schema.sql` 檔案中的所有 SQL 語句，以建立資料表並插入範例數據。
      - 例如，使用 `sqlite3` 命令列工具 (需先安裝):
        ```bash
        sqlite3 data/app.db < schema.sql
        ```
      - 如果使用 GUI 工具，通常有菜單選項可以導入或執行 SQL 腳本。
    - 請注意 `schema.sql` 中的範例用戶密碼 (`PasswordHash`) 是使用 BCrypt 雜湊後的結果，與 `AccountService` 中的 `BCrypt.Net.BCrypt.Verify` 方法配合使用。
    - 在您的 `appsettings.json` 檔案中，找到 `ConnectionStrings` 部分，並確保 `Default` 連接字串指向您創建的 `.db` 檔案的路徑。範例：
      ```json
      {
        "ConnectionStrings": {
          "Default": "DataSource=./data/app.db" // 確保路徑正確指向您建立的 app.db
        }
        // ... 其他設定
      }
      ```

5.  **執行專案：**
    ```bash
    dotnet run
    ```
    專案啟動後，通常會監聽 `https://localhost:5001` 和 `http://localhost:5000`。

## 使用說明

- 開啟瀏覽器，前往應用程式的網址 (例如 `https://localhost:5001`)。
- **登入：**
  - 教師登入頁面：`/Account/TeacherLogin`
  - 學生登入頁面：`/Account/StudentLogin`
  - 使用範例帳號(teacher01 和 student01 的密碼皆為 123456)進行測試。
- **註冊：**
  - 註冊頁面：`/Account/Register` (新註冊用戶會被賦予學生角色)。
- **登入後導向：**
  - 成功登入後，教師會被導向至 `/Teacher/Dashboard` (假設您有此 Controller 和 View)。
  - 成功登入後，學生會被導向至 `/Student/Dashboard` (假設您有此 Controller 和 View)。
- **教師成績管理：**
  - 教師登入後，前往 `/Score/InputScores` 頁面。
  - 在此頁面，教師可以看到自己教授的課程列表 (從 Claim `FullName` 匹配 Teacher Name 查詢)。
  - 選擇一個課程後，頁面會透過非同步請求載入該課程的學生列表及他們目前的成績。
  - 教師可以修改成績並儲存 (使用 Dapper 執行 SQLite 的 `ON CONFLICT DO UPDATE`)。
- **登出：**
  - 登出功能通常透過一個觸發 `/Account/Logout` POST 請求的按鈕或連結實現。

## 檔案結構 (基於提供的程式碼)

- `.csproj`: 專案設定檔，包含目標框架和 NuGet 套件引用。
- `program.cs`: 應用程式的進入點，設定服務和請求處理管道。
- `Controllers/AccountController.cs`: 處理用戶登入、註冊、登出等 HTTP 請求 (使用 ADO.NET 進行資料查詢)。
- `Controllers/ScoreController.cs`: 處理教師成績輸入、學生列表載入、成績儲存等功能 (使用 Dapper)。
- `Repositorys/`: 包含資料庫存取邏輯，如 `AccountRepos.cs` (使用 ADO.NET)。
- `Services/`: 包含業務邏輯服務，如 `AccountService.cs` (包含 BCrypt 密碼驗證邏輯)。
- `Models/`: 包含各種 ViewModels, DTOs, Enum (`UserRole`) 等定義。
- `Utils/`: 包含自訂工具方法或擴充方法，如 `ServiceDI.cs` 用於依賴注入設定。
- `schema.sql`: SQLite 資料庫的 Schema 定義和初始數據腳本。
- `Views/`: 包含相關的 Razor View 文件 (例如 `Account/TeacherLogin.cshtml`, `Account/StudentLogin.cshtml`, `Score/InputScores.cshtml`, `Score/_StudentScoreForm.cshtml` 等)。
- `appsettings.json`: 應用程式設定檔案，包含資料庫連接字串等。

## License

本專案使用 **MIT License**。詳細內容請參閱專案根目錄下的 `LICENSE` 檔案。
