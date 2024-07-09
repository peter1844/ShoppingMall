<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ShoppingMall.Views.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>首頁</title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>
    <script src="/js/Commons/MenuComponent.js" type="text/javascript"></script>
    <script src="/js/Pages/IndexComponent.js" type="text/javascript"></script>

    <link href="/css/Commons/Menu.css" rel="stylesheet" type="text/css"/>
    <link href="/css/Pages/Index.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <index></index>
        </div>
    </div>
</body>
</html>

<script>
    new Vue({
        el: '#app',
    });
</script>
