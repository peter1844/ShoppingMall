﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ShoppingMall.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>後臺管理系統</title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>
    <script src="/js/Pages/LoginComponent.js" type="text/javascript"></script>

    <link href="/css/Pages/Login.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <login></login>
    </div>
</body>
</html>

<script>
    new Vue({
        el: '#app',
    });
</script>