# Hololens_XrayScanTracking
## Background
A Hololens project to test how quickly a physician can scan an entire X-ray.
And my program records how many pieces this physician missed, then produce a report showing the physician's eye movements.

## Build
### Build enviroments : 
* Unity 2019.3.14
* Microsoft Mixed Reality Toolkit (MRTK)
* Visual Studio 2019 

### Build Steps : 
* https://docs.google.com/document/d/12Yos1P32cHMhzrZiSXLxmNy_o4rdJCGI7W753XdnJ0E/edit?usp=sharing

## Platform
* Hololens 2

## SPEC
* QRCode 掃描
  * 透過掃描QRCode 定義X-ray 的四個角落, 建立測試範圍.

* State machine
  * 建立狀態機控制狀態循環
    * 測試前
    * 測試中
    * 測試失敗
    * 測試成功
    * 倒數計時結束
  
* 視線追蹤.
  * 開始測試時, 將測試區域拆分成 x * y 片區域.
  * 當醫師的視線掃瞄過一片區域時, 將該區域刪除, 當x * y 片區域都被刪除時, 受測者測試成功.
  * 當倒數計時結束, 剩餘的區域會以紅色醒目的方式呈現給使用者, 這些事尚未掃描過的區域.
  
* 產生報表
  * 將測試時醫師每掃描過一片區域的時間跟座標記錄下來, 並記錄醫師的視線軌跡.
  * 在測試結束後存放在Hololens的硬碟中.
  
* Configuration
  * 將設定黨以圖形介面供使用者調整, 並將調整結果紀錄於內部檔案.
  
## Change Log
* v0.0.1 enviorment 建立與架設.
* v0.0.2 QRCode scan Proof Of Concept
* v0.0.3 UI & Scene
* v0.0.4 implement state machine
* v0.0.5 record path and produce report
* v0.0.6 Localization & fix bugs.

## Demo

[![IMAGE ALT TEXT](http://img.youtube.com/vi/i92U5Zxd2pg/0.jpg)](https://www.youtube.com/watch?v=i92U5Zxd2pg "Hololens eye tracking.")

