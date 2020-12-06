# Dessert
辦公室訂餐系統  
## 設計目的  
此項專案原本是公司內部提出的需求，雖然最後決定使用現有的平台。  
不過還是把當練習實作出來。  
* 學習 C# 與 ASP.NET WebForm 後端原理。
* 系統與資料表的規劃
* CRUD 操作

## 網站功能
* 使用者透過 session 機制登入
* 管理員可維護訂單、商品資料及商品類別管理
* 管理員可搜尋已新增的商家
* 管理員可將訂購資料匯出至Excel檔
* 管理員可建立新的訂單讓使用者訂購
* 使用者可以依開放的訂單選購商品
* 使用者可以瀏覽歷史訂單

## 結構
``` 
| ── |- login - 首頁: 登入/登出
|    |- 使用者登入
|    |       |
|    |       *- uOrder_List - 顯示所有訂單
|    |       *- uOrdered - 訂單明細
|    |       *- uOrder - 訂購商品
|    |- 管理員登入
|    |       |
|    |       *- aOrder_List - 顯示所有訂單
|    |       *- aOrder_New - 新增訂單
|    |       *- aOrderDetail - 訂單明細
|    |       *- aStore - 店家列表       
|    |       *- aStoreDetail - 商品管理  
|    |       *- aItemCategory_Mgt - 商品類別管理
|- 網站主版頁面
|    |- Site_user
│    |- Site_admin
|- dessert.accdb 資料庫
|- Web.config 資料庫連線字串設定
|- ErrorPages
|- css
      
``` 
## 工具
* ASP.NET WebForm  
* Access  
* Bootstrap
