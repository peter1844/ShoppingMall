<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order.aspx.cs" Inherits="ShoppingMall.Views.Order" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>訂單管理</title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>
    <script src="/js/Commons/MenuComponent.js" type="text/javascript"></script>
    <script src="/js/Pages/orderComponent.js" type="text/javascript"></script>

    <link href="/css/Commons/menu.css" rel="stylesheet" type="text/css"/>
    <link href="/css/Pages/order.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <order></order>
        </div>
    </div>
</body>
</html>

<script>
    new Vue({
        el: '#app',
    });
</script>