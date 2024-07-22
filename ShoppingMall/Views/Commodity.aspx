<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Commodity.aspx.cs" Inherits="ShoppingMall.Views.Ccommodity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>商品管理</title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>
    <script src="/js/commons/MenuComponent.js" type="text/javascript"></script>
    <script src="/js/pages/CommodityComponent.js" type="text/javascript"></script>

    <link href="/css/Commons/menu.css" rel="stylesheet" type="text/css"/>
    <link href="/css/Pages/commodity.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <commodity></commodity>
        </div>
    </div>
</body>
</html>

<script>
    new Vue({
        el: '#app',
    });
</script>